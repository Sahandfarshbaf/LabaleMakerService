namespace LabaleMakerService.Services.Interfaces
{
    public interface ICurrentUserService
    {
        public long UserId { get; }
        public long? RoleId { get; }
        public long? UserRoleId { get; }

    }
}
