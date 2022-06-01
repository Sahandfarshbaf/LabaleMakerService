using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.DataTransferObjects.Account;
using Entities.DataTransferObjects.BaseInfo;

namespace LabaleMakerService.Services.Interfaces
{
    public interface IBaseInfoService
    {
        List<RoleDto> GetRoleList();
    }
}
