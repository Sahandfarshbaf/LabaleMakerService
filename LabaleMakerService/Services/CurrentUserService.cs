using System.Linq;
using LabaleMakerService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Contracts;
using Microsoft.EntityFrameworkCore;


namespace LabaleMakerService.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IRepositoryWrapper _repository;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repository)
        {
            _repository = repository;


            if (long.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue("Id"), out long res))
                UserId = res;
            else UserId = -1;

            if (long.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue("roleId"), out long res1))
                RoleId = res1;
            else RoleId = null;

            if (long.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue("UserRoleId"), out long res2))
                UserRoleId = res2;
            else UserRoleId = null;

            var user = _repository.Users.FindByCondition(c => c.Id == UserId)
                .Include(c => c.UserRoles).FirstOrDefault();

           
        }

        public long UserId { get; }
        public long? RoleId { get; }
        public long? UserRoleId { get; }

    }
}
