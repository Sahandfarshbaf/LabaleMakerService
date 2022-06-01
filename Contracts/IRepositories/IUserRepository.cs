using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts.IRepositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        User GetUserByUserName(string userName);
    }
}
