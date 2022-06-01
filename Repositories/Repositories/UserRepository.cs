using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.IRepositories;
using Entities.Models;

namespace Repositories.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(LabelMaker_BP_DBContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public User GetUserByUserName(string userName) => RepositoryContext.Users.FirstOrDefault(c => c.UserName == userName && c.Ddate == null);

    }
}
