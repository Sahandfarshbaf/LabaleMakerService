
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        ICommodityRepository Commodity { get; }
        IRoleRepository Role { get; }
        IUserRepository Users { get; }
        IUserLoginRepository UserLogin { get; }
        IUserRoleRepository UserRole { get; }




        void Save();


    }
}