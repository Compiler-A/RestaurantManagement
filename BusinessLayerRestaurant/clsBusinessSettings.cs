using DataLayerRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{

    public interface IReadableSettingsBusiness
    {
        Task<List<DTOSettings>> GetAllSettingsAsync(int page);
        Task<DTOSettings?> GetSettingAsync(int ID);
    }

    public interface IWritableSettingsBusiness
    {
        Task<bool> Save();
        Task<bool> Delete(int ID);
    }

    public interface IBusinessSettings : IReadableSettingsBusiness, IWritableSettingsBusiness
    {
        DTOSettings? DTOSetting { get; set; }
    }

    public class clsBusinessSettings : IBusinessSettings
    {

        public enum enMode
        {
            Add, Update
        }
        public enMode Mode { get; set; } = enMode.Add;

        private DTOSettings? _Settings { get; set; }
        public DTOSettings? DTOSetting
        {
            get => _Settings;
            set => _Settings = value;
        }
        private IDataSettings _dataSettings;

        public clsBusinessSettings(IDataSettings setting)
        {
            _dataSettings = setting;
            Mode = enMode.Add;
        }

        public async Task<List<DTOSettings>> GetAllSettingsAsync(int page)
        {
            var list = await _dataSettings.GetAllSettings(page);
           
            return list;
        }


        public async Task<DTOSettings?> GetSettingAsync(int ID)
        {
            var dto = await _dataSettings.GetSetting(ID);
            if (dto == null)
            {
                return null;
            }
            _Settings = dto;
            Mode = enMode.Update;
            return dto;
        }

        private async Task<bool> _Add()
        {
            if (_Settings == null)
            {
                return false;
            }
            _Settings.ID = await _dataSettings.Add(_Settings);
            if (_Settings.ID > 0)
            {
                Mode = enMode.Update;
                return true;
            }
            return false;
        }

        private async Task<bool> _Update()
        {
            if (_Settings == null || _Settings.ID <= 0)
            {
                return false;
            }
            return await _dataSettings.Update(_Settings);
        }

        public async Task<bool> Save()
        {
            bool result = false;
            if (Mode == enMode.Add)
            {
                result = await _Add();
            }
            else if (Mode == enMode.Update)
            {
                result = await _Update();
            }
            return result;
        }

        public async Task<bool> Delete(int ID)
        {
            return await _dataSettings.Delete(ID);
        }
    }
}
