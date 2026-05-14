using ContractsLayerRestaurant.DTOResponse;
using DomainLayer.Entities;


namespace BusinessLayerRestaurant.Mapper
{
    public static class SettingMapper
    {
        public static DTOSettingResponse ToResponse(this Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));
            return new DTOSettingResponse
            {
                ID = setting.SettingID,
                Name = setting.Name,
                Value = setting.Value
            };
        }
    }
}
