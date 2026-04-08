#pragma warning disable CA1416 // Validate platform compatibility
using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace BusinessLayerRestaurant
{

    public class clsSettingsRepositoryBridge : IInterfaceBSettings
    {
        private IDataSettings _IDataSetting;
        public IDataSettings IData 
        { 
            get => _IDataSetting; 
            set => _IDataSetting = value; 
        }

        public clsSettingsRepositoryBridge(IDataSettings dataSetting)
        {
            _IDataSetting = dataSetting;
        }
    }



    public class clsSettingsReader : IReadableBSettings
    {
        private IMyLogger _Logger;
        private IInterfaceBSettings _Interface;
        public clsSettingsReader(IInterfaceBSettings setting, IMyLogger logger)
        {
            _Interface = setting;
            _Logger = logger;
        }

        public async Task<List<DTOSettings>> GetAllAsync(int page)
        {

            var list = await _Interface.IData.GetAllSettingsAsync(page);
            if (list == null || list.Count == 0)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            _Logger.EventLogs($"Settings Found, Count: {list.Count}", EventLogEntryType.Information);
            return list;
        }
        public async Task<DTOSettings?> GetAsync(int ID)
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
    public class clsSettingsWriter : IWritableBSettings
    {
        private IMyLogger _Logger;
        private IInterfaceBSettings _Interface;
        public clsSettingsWriter(IInterfaceBSettings setting, IMyLogger Logger)
        {
            _Interface = setting;
            _Logger = Logger;
        }



        public async Task<DTOSettings?> CreateAsync(DTOSettingsCRequest setting)
        {

            var dto = await _Interface.IData.AddSettingAsync(setting);
            if (dto == null)
            {
                throw new InvalidOperationException("Not Created!");
            }
            _Logger.EventLogs($"Setting Created, Name: {dto.Name}", EventLogEntryType.Information);
            return dto;
        }
        public async Task<DTOSettings?> UpdateAsync(DTOSettingsURequest setting)
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


    public class clsBusinessSettings : IBusinessSettings
    {
        private IWritableBSettings _IWrite;
        private IReadableBSettings _IRead;

        public clsBusinessSettings(
            IWritableBSettings Write,
            IReadableBSettings read)
        {
            _IWrite = Write;
            _IRead = read;
        }


        public async Task<List<DTOSettings>> GetAllSettingsAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<DTOSettings?> GetSettingAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }


        public async Task<DTOSettings?> AddSettingAsync(DTOSettingsCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<DTOSettings?> UpdateSettingAsync(DTOSettingsURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteSettingAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}
