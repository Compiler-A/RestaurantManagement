using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{
    public interface IDTOBSettings : IDTOBase<DTOSettingsCRequest, DTOSettingsURequest>
    { }

    public interface IInterfaceBSettings : IInterfaceBase<IDataSettings>
    { }

    public interface IReadableBSettings : IReadableBusinessBase<DTOSettings>
    { }
    public interface IWritableBSettings : IWritableBusinessBase<DTOSettings, DTOSettingsCRequest, DTOSettingsURequest>
    { }

    public interface IReadableBusinessSettings
    {
        Task<List<DTOSettings>> GetAllSettingsAsync(int page);
        Task<DTOSettings?> GetSettingAsync(int ID);
    }
    public interface IWritableBusinessSettings
    {
        Task<DTOSettings?> AddSettingAsync(DTOSettingsCRequest setting);
        Task<DTOSettings?> UpdateSettingAsync(DTOSettingsURequest setting);
        Task<bool> DeleteSettingAsync(int ID);
    }
    public interface ICRUDBusinessSettings : IWritableBusinessSettings, IReadableBusinessSettings
    { }
    public interface IBusinessSettings : ICRUDBusinessSettings, IDTOBSettings
    {
    }
}
