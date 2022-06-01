using System;
using Contracts;
using Contracts.IRepositories;
using Entities.Models;
using Repositories.Repositories;


namespace Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly LabelMaker_BP_DBContext _repoContext;

        public RepositoryWrapper(LabelMaker_BP_DBContext repoContext)
        {
            _repoContext = repoContext;

        }

        private ICommodityRepository _commodity;
        private IRoleRepository _role;
        private IUserRepository _user;
        private IUserLoginRepository _userLogin;
        private IUserRoleRepository _userRole;



        public ICommodityRepository Commodity => _commodity ??= new CommodityRepository(_repoContext);
        public IRoleRepository Role => _role ??= new RoleRepository(_repoContext);
        public IUserLoginRepository UserLogin => _userLogin ??= new UserLoginRepository(_repoContext);
        public IUserRoleRepository UserRole => _userRole ??= new UserRoleRepository(_repoContext);
        public IUserRepository Users => _user ??= new UserRepository(_repoContext);

       

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
