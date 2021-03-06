using Entities.DataTransferObjects.Account;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.IRepositories
{
    public interface IRoleRepository : IRepositoryBase<Role>
    {
        List<RoleDto> GetRoleListAll();
    }
}