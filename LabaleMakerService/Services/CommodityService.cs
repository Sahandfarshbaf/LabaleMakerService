using System.Reflection;
using Contracts;
using Entities.DataTransferObjects.Commodity;
using Entities.Models;
using LabaleMakerService.Services.Interfaces;
using Logger;
using Microsoft.EntityFrameworkCore;

namespace LabaleMakerService.Services
{
    public class CommodityService : ICommodityService
    {
        #region IOC & Constructor

        private readonly IRepositoryWrapper _repository;
        private readonly ILogHandler _logger;
        private readonly IConfiguration _configuration;

        public CommodityService(IRepositoryWrapper repository, ILogHandler logger, IConfiguration configuration)
        {
            _repository = repository;
            _logger = logger;
            _configuration = configuration;
        }

        #endregion

        public long InsertCommidity(long userId, CommodityDto input)
        {
            try
            {
                if (input is null)
                    throw new BusinessException("مشخصات کالا ارسال نشده است.", 4001);
                if (input.ConsumerPrice is < 0)
                    throw new BusinessException("قیمت مصرف کننده صحیح نیست.", 4002);
                if (input.MunufacturerPrice is < 0)
                    throw new BusinessException("قیمت تولید کننده صحیح نیست.", 4003);
                if (input.Name is not null && input.Name.Length > 250)
                    throw new BusinessException("طول نام کالا نمی تواند بیشتر از 250 کاراکتر باشد.", 4004);
                if (_repository.Commodity.FindByCondition(c => c.CommodityId == input.CommodityId).Any())
                    throw new BusinessException("کالا با شناسه وارد شده قبلا ثبت شده است.", 4005);

                var newCommodity = new Commodity
                {
                    Cdate = DateTime.Now.Ticks,
                    ConsumerPrice = input.ConsumerPrice,
                    CuserId = userId,
                    MunufacturerPrice = input.MunufacturerPrice,
                    Name = input.Name,
                    TrackingCode = input.TrackingCode,
                    CommodityId = input.CommodityId
                };
                _repository.Commodity.Create(newCommodity);
                _repository.Save();
                return newCommodity.Id;
            }
            catch (Exception ex)
            {
                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), userId, input);
                throw;
            }
        }
        public void DeleteCommidity(long userId, long id)
        {
            try
            {
                var commodity = _repository.Commodity.FindByCondition(c => c.Id == id).FirstOrDefault();
                if (commodity is null)
                    throw new BusinessException("کالا یافت نشد", 4001);

                commodity.DuserId = userId;
                commodity.Ddate = DateTime.Now.Ticks;
                _repository.Commodity.Update(commodity);
                _repository.Save();

            }
            catch (Exception ex)
            {
                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), userId, id);
                throw;
            }
        }
        public void ToggleActivationCommidity(long userId, long id)
        {
            try
            {
                var commodity = _repository.Commodity.FindByCondition(c => c.Id == id).FirstOrDefault();
                if (commodity is null)
                    throw new BusinessException("کالا یافت نشد", 4001);

                if (commodity.DaDate is null)
                {
                    commodity.DaDate = DateTime.Now.Ticks;
                    commodity.DaUserId = userId;
                }
                else
                {
                    commodity.DaDate = null;
                    commodity.DaUserId = null;
                }


                _repository.Commodity.Update(commodity);
                _repository.Save();

            }
            catch (Exception ex)
            {
                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), userId, id);
                throw;
            }
        }
        public void UpdateCommidity(long userId, CommodityDto input)
        {
            try
            {
                if (input is null)
                    throw new BusinessException("مشخصات کالا ارسال نشده است.", 4001);
                if (input.ConsumerPrice is < 0)
                    throw new BusinessException("قیمت مصرف کننده صحیح نیست.", 4002);
                if (input.MunufacturerPrice is < 0)
                    throw new BusinessException("قیمت تولید کننده صحیح نیست.", 4003);
                if (input.Name is not null && input.Name.Length > 250)
                    throw new BusinessException("طول نام کالا نمی تواند بیشتر از 250 کاراکتر باشد.", 4004);
                if (_repository.Commodity.FindByCondition(c => c.CommodityId == input.CommodityId && c.Id != input.Id).Any())
                    throw new BusinessException("کالا با شناسه وارد شده قبلا ثبت شده است.", 4005);

                var commodity = _repository.Commodity.FindByCondition(c => c.Id == input.Id).FirstOrDefault();
                if (commodity is null)
                    throw new BusinessException("کالا یافت نشد", 4006);





                commodity.MuserId = userId;
                commodity.Mdate = DateTime.Now.Ticks;
                commodity.Name = input.Name;
                commodity.CommodityId = input.CommodityId;
                commodity.ConsumerPrice = input.ConsumerPrice;
                commodity.MunufacturerPrice = input.MunufacturerPrice;
                commodity.TrackingCode = input.TrackingCode;
                _repository.Commodity.Update(commodity);
                _repository.Save();
            }
            catch (Exception ex)
            {
                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), userId, input);
                throw;
            }
        }
        public List<CommodityDto> GetCommoditiesList(GetCommoditiesListInputDto input, out int totalCount)
        {
            try
            {
                var result = _repository.Commodity.FindByConditionWithPagingOrder(c => c.Ddate == null &&
                    (!input.CommodityId.HasValue || c.CommodityId == input.CommodityId) &&
                    (!input.TrackingCode.HasValue || c.TrackingCode == input.TrackingCode) &&
                    (!input.MunufacturerPrice.HasValue || c.MunufacturerPrice == input.MunufacturerPrice) &&
                    (!input.ConsumerPrice.HasValue || c.ConsumerPrice == input.ConsumerPrice) &&
                    (string.IsNullOrWhiteSpace(input.Name) || c.Name.Contains(input.Name)),
                    c => c.OrderBy(x => x.Id), input.PageNumber, input.PageSize, out totalCount).Select(c => new CommodityDto
                    {
                        ConsumerPrice = c.ConsumerPrice,
                        MunufacturerPrice = c.MunufacturerPrice,
                        TrackingCode = c.TrackingCode,
                        CommodityId = c.CommodityId,
                        Name = c.Name,
                        Id = c.Id

                    }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), input, 0);
                throw;
            }
        }
        public CommodityFullInfoDto GetCommodityById(long id)
        {
            try
            {
                var res = _repository.Commodity.FindByCondition(c => c.Id == id).Include(c => c.Cuser).Select(c => new CommodityFullInfoDto
                {
                    CommodityId = c.CommodityId,
                    ConsumerPrice = c.ConsumerPrice,
                    CreateDate = c.Cdate,
                    CreateUserName = c.Cuser.UserName,
                    MunufacturerPrice = c.MunufacturerPrice,
                    Name = c.Name,
                    TrackingCode = c.TrackingCode,
                    Id = c.Id

                }).FirstOrDefault();
                if (res is null)
                    throw new BusinessException("کالا یافت نشد", 4001);

                return res;


            }
            catch (Exception ex)
            {
                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), id);
                throw;
            }
        }
        public List<UpdateCommodityTrackingCodeListDto> UpdateCommodityTrackingCodeList(long userId, List<UpdateCommodityTrackingCodeListDto> input)
        {
            try
            {
                if (input is null)
                    throw new BusinessException("اطلاعاتی ارسال نشده است", 4001);
                if (input.GroupBy(c => c.CommodityId).Select(c => new { c.Key, Count = c.Count() })
                    .Any(c => c.Count > 0))
                    throw new BusinessException("شناسه تکراری در لیست وجود دارد.", 4002);

                var toBeUpdatedList = new List<Commodity>();
                var res = input;
                res.ForEach(c =>
                {

                    var commodity = _repository.Commodity.FindByCondition(x => x.CommodityId == c.CommodityId)
                        .FirstOrDefault();
                    if (commodity is null)
                    {
                        c.IsSuccessFull = false;
                        c.ErrorDescription = "رکورد یافت نشد";
                    }
                    else
                    {
                        c.IsSuccessFull = true;
                        commodity.TrackingCode = c.TrackingCode;
                        commodity.Mdate = DateTime.Now.Ticks;
                        commodity.MuserId = userId;
                        toBeUpdatedList.Add(commodity);
                    }

                });

                _repository.Commodity.UpdateRange(toBeUpdatedList);
                _repository.Save();
                return res;
            }
            catch (Exception ex)
            {
                _logger.SaveError(ex, MethodBase.GetCurrentMethod(), userId, input);
                throw;
            }
        }
    }
}
