#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using BusinessLayerRestaurant.Interfaces;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.Settings;



namespace BusinessLayerRestaurant.Classes
{

    public class clsSettingsContainer : ISettingsServiceContainer
    {
        private ISettingsRepository _IDataSetting;
        public ISettingsRepository IData 
        { 
            get => _IDataSetting; 
            set => _IDataSetting = value; 
        }

        public clsSettingsContainer(ISettingsRepository dataSetting)
        {
            _IDataSetting = dataSetting;
        }
    }



    public class clsSettingsReader : ISettingsServiceReader
    {
        private IMyLogger _Logger;
        private ISettingsServiceContainer _Interface;
        public clsSettingsReader(ISettingsServiceContainer setting, IMyLogger logger)
        {
            _Interface = setting;
            _Logger = logger;
        }

        public async Task<List<Setting>> GetAllAsync(int page)
        {

            var list = await _Interface.IData.GetAllSettingsAsync(page);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Settings Found, Count: {list.Count}", EventLogEntryType.Information);
            return list;
        }
        public async Task<Setting?> GetAsync(int ID)
        {
            var dto = await _Interface.IData.GetSettingAsync(ID);
            if (dto == null)
            {
                throw new KeyNotFoundException("Not Found!");   
            }
            _Logger.EventLogs($"Setting Found, Name: {dto.Name}", EventLogEntryType.Information);
            return dto;
        }
    }
    public class clsSettingsWriter : ISettingsServiceWriter
    {
        private IMyLogger _Logger;
        private ISettingsServiceContainer _Interface;
        public clsSettingsWriter(ISettingsServiceContainer setting, IMyLogger Logger)
        {
            _Interface = setting;
            _Logger = Logger;
        }



        public async Task<Setting?> CreateAsync(DTOSettingsCRequest setting)
        {

            var dto = await _Interface.IData.AddSettingAsync(setting);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            _Logger.EventLogs($"Setting Created, Name: {dto.Name}", EventLogEntryType.Information);
            return dto;
        }
        public async Task<Setting?> UpdateAsync(DTOSettingsURequest setting)
        {
           
            
            var dto = await _Interface.IData.UpdateSettingAsync(setting);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Updated!");
            }
            _Logger.EventLogs($"Setting Updated, Name: {dto.Name}", EventLogEntryType.Information);

            return dto;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var result = await _Interface.IData.DeleteSettingAsync(ID);
            if (!result)
            {
                throw new InvalidOperationException("Not Deleted!");
            }
            _Logger.EventLogs($"Setting Deleted, ID: {ID}", EventLogEntryType.Information);
            return result;
        }
    }


    public class clsSettingsService : ISettingsService
    {
        private ISettingsServiceWriter _IWrite;
        private ISettingsServiceReader _IRead;

        public clsSettingsService(
            ISettingsServiceWriter Write,
            ISettingsServiceReader read)
        {
            _IWrite = Write;
            _IRead = read;
        }


        public async Task<List<Setting>> GetAllSettingsAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<Setting?> GetSettingAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }


        public async Task<Setting?> AddSettingAsync(DTOSettingsCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<Setting?> UpdateSettingAsync(DTOSettingsURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteSettingAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}
