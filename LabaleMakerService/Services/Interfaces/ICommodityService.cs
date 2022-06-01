using Entities.DataTransferObjects.Commodity;

namespace LabaleMakerService.Services.Interfaces
{
    public interface ICommodityService
    {
        long InsertCommidity(long userId, CommodityDto input);
        void DeleteCommidity(long userId, long id);
        void ToggleActivationCommidity(long userId, long id);
        void UpdateCommidity(long userId, CommodityDto input);
        List<CommodityDto> GetCommoditiesList(GetCommoditiesListInputDto input, out int totalCount);
        CommodityFullInfoDto GetCommodityById(long id);

        List<UpdateCommodityTrackingCodeListDto> UpdateCommodityTrackingCodeList(long userId,
            List<UpdateCommodityTrackingCodeListDto> input);
    }
}
