using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LabaleMakerService.Services.Interfaces;
using Contracts;
using Entities.DataTransferObjects.Account;
using Entities.DataTransferObjects.BaseInfo;
using Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;


namespace LabaleMakerService.Services
{
    public class BaseInfoService : IBaseInfoService
    {
        #region IOC & Constructor


        private readonly IRepositoryWrapper _repository;
        private readonly ILogHandler _logger;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;


        public BaseInfoService(IRepositoryWrapper repository, ILogHandler logger, IConfiguration configuration, IMemoryCache cache)
        {

            _repository = repository;
            _logger = logger;
            _configuration = configuration;
            _cache = cache;
        }

        #endregion
        public List<RoleDto> GetRoleList()
        {

            try
            {

                _cache.GetOrCreate("RoleListAll", c =>
                {
                    c.AbsoluteExpiration = DateTime.Now.AddDays(10);
                    return _repository.Role.GetRoleListAll().ToList();

                });
                var result = _cache.Get<List<RoleDto>>("RoleListAll");
                return result;
            }
            catch (Exception ex)
            {

                _logger.SaveError(ex, MethodBase.GetCurrentMethod());
                throw;
            }
        }
    }
}
