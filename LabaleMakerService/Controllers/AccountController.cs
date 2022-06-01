using System.Diagnostics;
using System.Reflection;
using Entities.DataTransferObjects.Account;
using Entities.UIResponse;
using LabaleMakerService.Services.Interfaces;
using LabaleMakerService.Tools;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabaleMakerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region IOC & Constructor

        private readonly ICurrentUserService _currentService;
        private readonly IAccountService _accountService;
        private readonly ILogHandler _logger;
        private readonly IBaseInfoService _baseService;


        public AccountController(ICurrentUserService currentService, IAccountService accountService, ILogHandler logger, IBaseInfoService baseService)
        {
            _currentService = currentService;
            _accountService = accountService;
            _logger = logger;
            _baseService = baseService;
        }

        #endregion

        #region Login & Register

        [HttpPost]
        [Route("Login")]
        public SingleResult<LoginResult> Login(LoginDto input)
        {

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var result = _accountService.Login(input);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), result, sw.Elapsed, input);

                return SingleResult<LoginResult>.GetSuccessfulResult(result);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), input);
                return SingleResult<LoginResult>.GetFailResult(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetToken")]
        public SingleResult<GetTokenResult> GetToken(Guid userLoginId, long roleId)
        {

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var res = _accountService.GetToken(userLoginId, roleId);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), res, sw.Elapsed, userLoginId, roleId);
                return SingleResult<GetTokenResult>.GetSuccessfulResult(res);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), userLoginId, roleId);
                return SingleResult<GetTokenResult>.GetFailResult(ex.Message);
            }


        }


        [HttpPost]
        [Route("SetPassword")]
        public VoidResult SetPassword(SetPasswordDto input)
        {

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _accountService.SetPassword(input);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), General.Results_.SuccessMessage(), sw.Elapsed, input);
                return VoidResult.GetSuccessfulResult(General.Results_.SuccessMessage());
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), input);
                return VoidResult.GetFailResult(ex.Message);
            }

        }


        [Authorize]
        [HttpPost]
        [Route("ReSetPassword")]
        public VoidResult ReSetPassword(string oldPassword, string newPassword)
        {

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _accountService.ReSetPassword(oldPassword, newPassword, _currentService.UserId);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), General.Results_.SuccessMessage(), sw.Elapsed, oldPassword, newPassword);
                return VoidResult.GetSuccessfulResult(General.Results_.SuccessMessage());
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), oldPassword, newPassword);
                return VoidResult.GetFailResult(ex.Message);
            }

        }

        #endregion

        [Authorize(policy: "Admin")]
        [HttpGet]
        [Route("GetUserListForAdmin")]
        public ListResult<UserListDto> GetUserListForAdmin()
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var res = _accountService.GetUserListForAdmin();
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), res, sw.Elapsed);
                return ListResult<UserListDto>.GetSuccessfulResult(res, res.Count);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod());
                return ListResult<UserListDto>.GetFailResult(ex.Message);
            }

        }

        [Authorize(policy: "Admin")]
        [HttpPut]
        [Route("ToggleUserActivationByAdmin")]
        public VoidResult ToggleUserActivationByAdmin(long userId)
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _accountService.ToggleUserActivationByAdmin(userId);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), null, sw.Elapsed, userId);
                return VoidResult.GetSuccessfulResult();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), userId);
                return VoidResult.GetFailResult(ex.Message);
            }

        }

        [Authorize(policy: "Admin")]
        [HttpPost]
        [Route("InsertUser")]
        public LongResult InsertUser(UserInsertDto input)
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var res = _accountService.InsertUser(input);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), res, sw.Elapsed, input);
                return LongResult.GetSingleSuccessfulResult(res);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), input);
                return LongResult.GetFailResult(ex.Message);
            }

        }

        [Authorize(policy: "Admin")]
        [HttpPut]
        [Route("UpdatetUser")]
        public VoidResult UpdatetUser(UserInsertDto input)
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _accountService.UpdatetUser(input);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), null, sw.Elapsed, input);
                return VoidResult.GetSuccessfulResult();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), input);
                return VoidResult.GetFailResult(ex.Message);
            }

        }

        [Authorize(policy: "Admin")]
        [HttpPut]
        [Route("ResetUserPassWordByAdmin")]
        public VoidResult ResetUserPassWordByAdmin(long userId)
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _accountService.ResetUserPassWordByAdmin(userId);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), null, sw.Elapsed, userId);
                return VoidResult.GetSuccessfulResult();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), userId);
                return VoidResult.GetFailResult(ex.Message);
            }

        }

        [Authorize(policy: "Admin")]
        [HttpGet]
        [Route("GetUserFullInfoById")]
        public SingleResult<UserFullInfoDto> GetUserFullInfoById(long userId)
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var res = _accountService.GetUserFullInfoById(userId);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), res, sw.Elapsed, userId);
                return SingleResult<UserFullInfoDto>.GetSuccessfulResult(res);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), userId);
                return SingleResult<UserFullInfoDto>.GetFailResult(ex.Message);
            }

        }

        [Authorize(policy: "Admin")]
        [HttpDelete]
        [Route("DeleteUserByAdmin")]
        public VoidResult DeleteUserByAdmin(long userId)
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _accountService.DeleteUserByAdmin(userId);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), null, sw.Elapsed, userId);
                return VoidResult.GetSuccessfulResult();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), userId);
                return VoidResult.GetFailResult(ex.Message);
            }

        }

        [Authorize(policy: "Admin")]
        [HttpGet]
        [Route("GetRoleList")]
        public ListResult<RoleDto> GetRoleList()
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var res = _baseService.GetRoleList();
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), res, sw.Elapsed);
                return ListResult<RoleDto>.GetSuccessfulResult(res, res.Count);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod());
                return ListResult<RoleDto>.GetFailResult(ex.Message);
            }

        }
    }
}
