using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts.IRepositories;
using Entities.DataTransferObjects.Account;
using Repositories;


namespace Repositories.Repositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(LabelMaker_BP_DBContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public List<RoleDto> GetRoleListAll() => RepositoryContext.Roles.Where(r => r.Ddate == null).Select(c => new RoleDto { RoleID = c.Id, RoleName = c.Name }).ToList();
    }
}
