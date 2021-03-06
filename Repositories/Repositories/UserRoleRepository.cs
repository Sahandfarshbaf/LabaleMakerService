using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts.IRepositories;
using Entities.DataTransferObjects.Account;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Repositories.Repositories
{
    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(LabelMaker_BP_DBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IQueryable<RoleDto> GetRoleListByUserId(long userId)
        {
            return RepositoryContext.UserRoles.Where(c => c.UserId == userId)
                .Include(c => c.Role).Select(c => new RoleDto { RoleID = c.RoleId.Value, RoleName = c.Role.Name });

        }
    }
}
