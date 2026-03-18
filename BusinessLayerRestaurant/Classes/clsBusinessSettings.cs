using Azure.Core;
using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{

    public class clsDTOBSettings : IDTOBSettings
    {
        private DTOSettingsCRequest? _CreateRequest;
        public DTOSettingsCRequest? CreateRequest 
        { 
            get => _CreateRequest; 
            set => _CreateRequest = value;
        }
        private DTOSettingsURequest? _UpdateRequest;
        public DTOSettingsURequest? UpdateRequest 
        {
            get => _UpdateRequest;
            set => _UpdateRequest = value;
        }
    }   
    public class clsInterfaceBSettings : IInterfaceBSettings
    {
        private IDataSettings _IDataSetting;
        public IDataSettings IData 
        { 
            get => _IDataSetting; 
            set => _IDataSetting = value; 
        }

        public clsInterfaceBSettings(IDataSettings dataSetting)
        {
            _IDataSetting = dataSetting;
        }
    }



    public class clsReadableBSettings : IReadableBSettings
    {
        private IInterfaceBSettings _Interface;
        public clsReadableBSettings(IInterfaceBSettings setting)
        {
            _Interface = setting;
        }
        public async Task<List<DTOSettings>> GetAllAsync(int page)
        {
            var list = await _Interface.IData.GetAllSettingsAsync(page);
            return list;
        }
        public async Task<DTOSettings?> GetAsync(int ID)
        {
            var dto = await _Interface.IData.GetSettingAsync(ID);
            
            return dto;
        }
    }
    public class clsWritableBSettings : IWritableBSettings
    {
        private IDTOBSettings _dto;
        private IInterfaceBSettings _Interface;
        public clsWritableBSettings(IInterfaceBSettings setting, IDTOBSettings dto)
        {
            _Interface = setting;
            _dto = dto;
        }



        public async Task<DTOSettings?> CreateAsync(DTOSettingsCRequest setting)
        {
            if (setting == null)
            { return null; }
            var dto = await _Interface.IData.AddSettingAsync(setting);
            if (dto != null)
            {
                return dto;
            }
            return null;
        }
        public async Task<DTOSettings?> UpdateAsync(DTOSettingsURequest setting)
        {
            if (setting == null || setting.ID <= 0)
            { return null; }
            
            var dto = await _Interface.IData.UpdateSettingAsync(setting);
            if (dto != null)
            {
                return dto;
            }
            return null;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _Interface.IData.DeleteSettingAsync(ID);
        }
    }


    public class clsBusinessSettings : IBusinessSettings
    {
        private IDTOBSettings _IProperties;
        private IWritableBSettings _IWrite;
        private IReadableBSettings _IRead;

        public clsBusinessSettings(
            IDTOBSettings Properties,
            IWritableBSettings Write,
            IReadableBSettings read)
        {
            _IProperties = Properties;
            _IWrite = Write;
            _IRead = read;
        }

        public DTOSettingsCRequest? CreateRequest
        {
            get => _IProperties.CreateRequest;
            set => _IProperties.CreateRequest = value;
        }

        public DTOSettingsURequest? UpdateRequest
        {
            get => _IProperties.UpdateRequest;
            set => _IProperties.UpdateRequest = value;
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
