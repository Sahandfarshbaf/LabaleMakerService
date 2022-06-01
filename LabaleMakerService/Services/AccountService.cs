using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System;
using Contracts;
using Entities.DataTransferObjects.Account;
using Entities.Models;
using LabaleMakerService.Services.Interfaces;
using LabaleMakerService.Tools;
using Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LabaleMakerService.Services
{
    public class AccountService : IAccountService
    {
        #region IOC & Constructor


        private readonly IRepositoryWrapper _repository;
        private readonly ILogHandler _logger;
        private readonly IConfiguration _configuration;


        public AccountService(IRepositoryWrapper repository, ILogHandler logger, IConfiguration configuration)
        {
            _repository = repository;
            _logger = logger;
            _configuration = configuration;
        }

        #endregion

        #region Login & Register

        public LoginResult Login(LoginDto input)
        {

            try
            {
                var user = _repository.Users.GetUserByUserName(input.UserName);

                if (user == null)
                    throw new BusinessException(XError.AuthenticationErrors_.UserNameOrPasswordIsWrong());

                if (user.DaDate is not null)
                    throw new BusinessException("حساب کاربری شما غیر فعال گردیده است", 4003);

                var ulogin = new UserLogin
                {
                    Id = Guid.NewGuid(),
                    LoginDate = DateTime.Now.Ticks,
                    UserId = user.Id,
                    LoginPassword = input.PassWord,

                };
                if (user.Hpassword != input.PassWord)
                {

                    ulogin.LoginType = 2;

                    _repository.UserLogin.Create(ulogin);
                    user.LoginTryCount = user.LoginTryCount++;
                    _repository.Users.Update(user);
                    _repository.Save();
                    throw new BusinessException(XError.AuthenticationErrors_.UserNameOrPasswordIsWrong());

                }

                ulogin.LoginType = 1;
                _repository.UserLogin.Create(ulogin);
                user.LoginTryCount = 0;
                _repository.Users.Update(user);
                _repository.Save();

                var roleList = _repository.UserRole.GetRoleListByUserId(user.Id).ToList();
                var result = new LoginResult { UserLoginID = ulogin.Id, RoleList = roleList };
                _logger.LogData(MethodBase.GetCurrentMethod(), result, null, input);
                return result;

            }
            catch (System.Exception ex)
            {

                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), input);
                throw;
            }



        }
        public GetTokenResult GetToken(Guid userLoginId, long roleId)
        {


            try
            {
                var userLogin = _repository.UserLogin.FindByCondition(c => c.Id == userLoginId).FirstOrDefault();

                if (userLogin is not { LoginType: 1 })
                    throw new BusinessException(XError.AuthenticationErrors_.NeedLoginAgain());

                var user = _repository.Users.FindByCondition(c => c.Id == userLogin.UserId)
                    .Include(c => c.UserRoles).FirstOrDefault();

                if (_repository.UserRole.FindByCondition(c => c.UserId == user.Id).All(c => c.RoleId != roleId))
                    throw new BusinessException(XError.AuthenticationErrors_.NotHaveRequestedRole());



                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("roleId", roleId.ToString()),
                    new Claim("UserRoleId", user.UserRoles.FirstOrDefault(c=>c.RoleId==roleId)?.Id.ToString() ?? string.Empty),
                };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, null, expires: DateTime.UtcNow.AddHours(2), signingCredentials: signIn);



                var res = new GetTokenResult
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    FullName = user.FullName,
                    Username = user.UserName,

                };


                return res;

            }
            catch (Exception ex)
            {

                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), userLoginId, roleId);
                throw;
            }

        }
        public void SetPassword(SetPasswordDto input)
        {
            try
            {
                var user = _repository.Users.FindByCondition(c => c.Id == input.UserId).Include(c => c.UserActivationCodes).FirstOrDefault();

                if (user == null)
                    throw new BusinessException(XError.GetDataErrors_.NotFound());

                var now = DateTime.Now.Ticks;
                if (!user.UserActivationCodes.Any(c => c.Code == input.Code && c.CodeType == 1 && c.EndDateTime > now))
                    throw new BusinessException(XError.BusinessErrors_.InvalidResetPasswordCode());

                user.Hpassword = input.Password;
                _repository.Users.Update(user);
                _repository.Save();
                _logger.LogData(MethodBase.GetCurrentMethod(), "OK", null, input);
            }
            catch (Exception ex)
            {


                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), input);
                throw;
            }

        }
        public void ReSetPassword(string oldPassword, string newPassword,long userId)
        {
            try
            {
      
                var user = _repository.Users.FindByCondition(c => c.Id == userId).FirstOrDefault();
                if (user == null)
                    throw new BusinessException(XError.GetDataErrors_.NotFound());

                if (user.Hpassword != oldPassword)
                    throw new BusinessException(XError.AuthenticationErrors_.PasswordIsWrong());

                user.Hpassword = newPassword;
                _repository.Users.Update(user);
                _repository.Save();
                _logger.LogData(MethodBase.GetCurrentMethod(), "OK", null, oldPassword, newPassword);
            }
            catch (Exception ex)
            {

                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), oldPassword, newPassword);
                throw;
            }

        }

        #endregion

        #region Admin

        public List<UserListDto> GetUserListForAdmin()
        {

            try
            {

                var res = _repository.Users.FindByCondition(c => c.Ddate == null)
                          .Include(c => c.UserRoles).ThenInclude(c => c.Role)
                          .Select(c => new UserListDto
                          {
                              Description = c.Description,
                              FullName = c.FullName,
                              Id = c.Id,
                              Mobile = c.Mobile,
                              NationalCode = c.NationalCode,
                              UserName = c.UserName,
                              IsActive = c.DaDate == null,
                              Roles = c.UserRoles.Select(x => x.Role.Name).ToList()

                          }).OrderByDescending(c => c.Id).ToList();
                return res;
            }
            catch (Exception ex)
            {
                _logger.SaveError(ex, MethodBase.GetCurrentMethod());
                throw;
            }

        }
        public void ToggleUserActivationByAdmin(long userId)
        {

            try
            {


                var user = _repository.Users.FindByCondition(c => c.Id == userId).FirstOrDefault();
                if (user is null)
                    throw new BusinessException("کاربر یافت نشد", 4004);
                user.DaDate = user.DaDate == null ? DateTime.Now.Ticks : null;
                _repository.Users.Update(user);
                _repository.Save();
            }
            catch (Exception ex)
            {

                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), userId);
                throw;
            }

        }
        public long InsertUser(UserInsertDto input)
        {
            try
            {

                if (_repository.Users.FindByCondition(c => (c.NationalCode == input.NationalCode || c.UserName == input.UserName || c.Mobile == input.Mobile) && c.Ddate == null).Any())
                    throw new BusinessException("کاربر با مشخصات وارد شده در سامانه وجود دارد.", 4002);

                if (input.RoleList is null || !input.RoleList.Any())
                    throw new BusinessException("نقشی برای کاربر انتخاب نشده است", 4003);

                
                var roleList = _repository.Role.FindByCondition(c => c.Ddate == null).Select(c => c.Id).ToList();

              
                input.RoleList.ForEach(c =>
                {
                    if (roleList.All(x => x != c))
                        throw new BusinessException("نقش های ارسالی صحیح نیست", 4005);

                });

                var code = Utilities.GenerateRandomPassword();
                var hpassword = new Encrypter().Encrypt(code);

                var newuser = new User
                {
                    Description = input.Description,
                    Cdate = DateTime.Now.Ticks,
                    FullName = input.FullName,
                    Hpassword = hpassword,
                    LoginTryCount = 0,
                    NationalCode = input.NationalCode,
                    UserName = input.UserName,
                    Mobile = input.Mobile,

                };

                input.RoleList.GroupBy(c => c).Select(c => c.Key).ToList().ForEach(c =>
                {

                    var newUserRole = new UserRole
                    {

                        Cdate = DateTime.Now.Ticks,
                        RoleId = c,

                    };

                  
                    newuser.UserRoles.Add(newUserRole);

                });

                _repository.Users.Create(newuser);
                _repository.Save();

                return newuser.Id;

            }
            catch (Exception ex)
            {

                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), input);
                throw;
            }

        }
        public void UpdatetUser(UserInsertDto input)
        {
            try
            {

                var user = _repository.Users.FindByCondition(c => c.Id == input.Id).FirstOrDefault();
                if (user is null)
                    throw new BusinessException("کاربر یافت نشد", 4004);

                if (_repository.Users.FindByCondition(c => c.Id != input.Id && c.Ddate == null && (c.NationalCode == input.NationalCode || c.UserName == input.UserName || c.Mobile == input.Mobile)).Any())
                    throw new BusinessException("کاربر با مشخصات وارد شده در سامانه وجود دارد.", 4002);

                if (input.RoleList is null || !input.RoleList.Any())
                    throw new BusinessException("نقشی برای کاربر انتخاب نشده است", 4003);

                var roleList = _repository.Role.FindByCondition(c => c.Ddate == null).Select(c => c.Id).ToList();


                input.RoleList.ForEach(c =>
                {
                    if (roleList.All(x => x != c))
                        throw new BusinessException("نقش های ارسالی صحیح نیست", 4005);

                });


                user.FullName = input.FullName;
                user.LoginTryCount = 0;
                user.NationalCode = input.NationalCode;
                user.UserName = input.UserName;
                user.Mobile = input.Mobile;
                user.Description = input.Description;

                var toBeDeletedRoleList = _repository.UserRole.FindByCondition(c => c.UserId == input.Id).ToList();
                _repository.UserRole.DeleteRange(toBeDeletedRoleList);


                input.RoleList.GroupBy(c => c).Select(c => c.Key).ToList().ForEach(c =>
                {

                    var newUserRole = new UserRole
                    {

                        Cdate = DateTime.Now.Ticks,
                        RoleId = c,

                    };
                    user.UserRoles.Add(newUserRole);

                });

                _repository.Users.Update(user);
                _repository.Save();


            }
            catch (Exception ex)
            {

                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), input);
                throw;
            }

        }
        public void ResetUserPassWordByAdmin(long userId)
        {

            try
            {
                var user = _repository.Users.FindByCondition(c => c.Id == userId).FirstOrDefault();
                if (user is null)
                    throw new BusinessException("کاربر یافت نشد", 4004);

                var code = Utilities.GenerateRandomPassword();
                var hpassword = new Encrypter().Encrypt(code);

                user.Hpassword = hpassword;
                _repository.Users.Update(user);
                _repository.Save();

                //var text = $"کلمه عبور حساب کاربری شما در سامانه سینا توسط ادمین بازنشانی شد.";
                //text += System.Environment.NewLine;
                //text += "نام کاربری : ";
                //text += user.UserName;
                //text += System.Environment.NewLine;
                //text += "کلمه عبور جدید : ";
                //text += code;


                //new SmsProvider().SendSms(text, user.Mobile.ToString());


            }
            catch (Exception ex)
            {

                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), userId);
                throw;
            }


        }
        public UserFullInfoDto GetUserFullInfoById(long userId)
        {

            try
            {
                var user = _repository.Users.FindByCondition(c => c.Id == userId)
                    .Include(c => c.UserRoles).ThenInclude(c => c.Role)
                    .Select(c => new UserFullInfoDto
                    {

                        Description = c.Description,
                        FullName = c.FullName,
                        Id = c.Id,
                        Mobile = c.Mobile,
                        NationalCode = c.NationalCode,
                        UserName = c.UserName,
                        RuleList = c.UserRoles.Select(x => new RoleDto { RoleID = x.RoleId.Value, RoleName = x.Role.Name }).ToList(),

                    })
                    .FirstOrDefault();

                if (user is null)
                    throw new BusinessException("کاربر یافت نشد", 4004);

                return user;


            }
            catch (Exception ex)
            {

                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), userId);
                throw;
            }


        }
        public void DeleteUserByAdmin(long userId)
        {

            try
            {


                var user = _repository.Users.FindByCondition(c => c.Id == userId).FirstOrDefault();
                if (user is null)
                    throw new BusinessException("کاربر یافت نشد", 4004);
                user.Ddate = DateTime.Now.Ticks;
                _repository.Users.Update(user);
                _repository.Save();
            }
            catch (Exception ex)
            {

                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), userId);
                throw;
            }

        }

        #endregion
    }
}
