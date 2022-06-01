using System.Diagnostics;
using System.Reflection;
using Entities.DataTransferObjects.Commodity;
using Entities.UIResponse;
using LabaleMakerService.Services.Interfaces;
using LabaleMakerService.Tools;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabaleMakerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommodityController : ControllerBase
    {
        #region IOC & Constructor

        private readonly ICurrentUserService _currentService;
        private readonly ILogHandler _logger;
        private readonly IBaseInfoService _baseService;
        private readonly ICommodityService _commodity;


        public CommodityController(ICurrentUserService currentService, ILogHandler logger, IBaseInfoService baseService, ICommodityService commodity)
        {
            _currentService = currentService;
            _logger = logger;
            _baseService = baseService;
            _commodity = commodity;
        }

        #endregion

        [Authorize]
        [HttpPost]
        [Route("InsertCommidity")]
        public SingleResult<long> InsertCommidity(CommodityDto input)
        {

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var res = _commodity.InsertCommidity(_currentService.UserId, input);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), res, sw.Elapsed, input);
                return SingleResult<long>.GetSuccessfulResult(res);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), input);
                return SingleResult<long>.GetFailResult(ex.Message);
            }

        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteCommidity")]
        public VoidResult DeleteCommidity(long id)
        {

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _commodity.DeleteCommidity(_currentService.UserId, id);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), null, sw.Elapsed, id);
                return VoidResult.GetSuccessfulResult();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), id);
                return VoidResult.GetFailResult(ex.Message);
            }

        }

        [Authorize]
        [HttpPut]
        [Route("ToggleActivationCommidity")]
        public VoidResult ToggleActivationCommidity(long id)
        {

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _commodity.ToggleActivationCommidity(_currentService.UserId, id);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), null, sw.Elapsed, id);
                return VoidResult.GetSuccessfulResult();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), id);
                return VoidResult.GetFailResult(ex.Message);
            }

        }

        [Authorize]
        [HttpPut]
        [Route("UpdateCommidity")]
        public VoidResult UpdateCommidity(CommodityDto input)
        {

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _commodity.UpdateCommidity(_currentService.UserId, input);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), null, sw.Elapsed, input);
                return VoidResult.GetSuccessfulResult();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), input);
                return VoidResult.GetFailResult(ex.Message);
            }

        }

        [Authorize]
        [HttpPost]
        [Route("GetCommoditiesList")]
        public ListResult<CommodityDto> GetCommoditiesList(GetCommoditiesListInputDto input)
        {

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var res = _commodity.GetCommoditiesList(input, out var totalCount);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), null, sw.Elapsed, input);
                return ListResult<CommodityDto>.GetSuccessfulResult(res, totalCount);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), input);
                return ListResult<CommodityDto>.GetFailResult(ex.Message);
            }

        }

        [Authorize]
        [HttpPost]
        [Route("GetCommodityById")]
        public SingleResult<CommodityFullInfoDto> GetCommodityById(long id)
        {

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var res = _commodity.GetCommodityById(id);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), res, sw.Elapsed, id);
                return SingleResult<CommodityFullInfoDto>.GetSuccessfulResult(res);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), id);
                return SingleResult<CommodityFullInfoDto>.GetFailResult(ex.Message);
            }

        }
        
        [Authorize]
        [HttpPost]
        [Route("UpdateCommodityTrackingCodeList")]
        public ListResult<UpdateCommodityTrackingCodeListDto> UpdateCommodityTrackingCodeList(List<UpdateCommodityTrackingCodeListDto> input)
        {

            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var res = _commodity.UpdateCommodityTrackingCodeList(_currentService.UserId, input);
                sw.Stop();
                _logger.LogData(MethodBase.GetCurrentMethod(), res, sw.Elapsed, input);
                return ListResult<UpdateCommodityTrackingCodeListDto>.GetSuccessfulResult(res);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, MethodBase.GetCurrentMethod(), input);
                return ListResult<UpdateCommodityTrackingCodeListDto>.GetFailResult(ex.Message);
            }

        }
    }
}
