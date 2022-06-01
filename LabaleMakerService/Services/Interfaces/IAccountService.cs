using Entities.DataTransferObjects.Account;

namespace LabaleMakerService.Services.Interfaces
{
    public interface IAccountService
    {
        #region Login & Register
        LoginResult Login(LoginDto input);
        GetTokenResult GetToken(Guid userLoginId, long roleId);
        void SetPassword(SetPasswordDto input);
        void ReSetPassword(string oldPassword, string newPassword, long userId);

        #endregion
        List<UserListDto> GetUserListForAdmin();
        void ToggleUserActivationByAdmin(long userId);
        long InsertUser(UserInsertDto input);
        void UpdatetUser(UserInsertDto input);
        void ResetUserPassWordByAdmin(long userId);
        UserFullInfoDto GetUserFullInfoById(long userId);
        void DeleteUserByAdmin(long userId);


    }
}
