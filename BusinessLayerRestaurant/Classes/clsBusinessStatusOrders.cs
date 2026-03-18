using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{

    public class clsDTOBStatusOrders : IDTOBStatusOrders
    {
        private DTOStatusOrdersCRequest? _CRequest;
        private DTOStatusOrdersURequest? _URequest;

        public DTOStatusOrdersCRequest? CreateRequest
        {
            get => _CRequest;
            set => _CRequest = value;
        }
        public DTOStatusOrdersURequest? UpdateRequest
        {
            get => _URequest;
            set => _URequest = value;
        }
    }
    public class clsInterfaceBStatusOrders : IInterfaceBStatusOrders
    {
        private IDataStatusOrders _IDataStatusOrder;
        public clsInterfaceBStatusOrders(IDataStatusOrders IDataStatusOrder)
        {
            this._IDataStatusOrder = IDataStatusOrder;
        }
        public IDataStatusOrders IData
        {
            get => this._IDataStatusOrder;
            set => this._IDataStatusOrder = value;
        }
    }

    public class clsReadableBStatusOrders : IReadableBStatusOrders
    {
        private IInterfaceBStatusOrders _Interface;
        public clsReadableBStatusOrders(IInterfaceBStatusOrders Interface)
        {
            _Interface = Interface;
        }
        public async Task<List<DTOStatusOrders>> GetAllAsync(int page)
        {
            return await _Interface.IData.GetAllStatusOrdersAsync(page);
        }

        public async Task<DTOStatusOrders?> GetAsync(int ID)
        {
           var dto = await _Interface.IData.GetStatusOrderAsync(ID);
            return dto;
        }
    }
    public class clsWritableBStatusOrders : IWritableBStatusOrders
    {
        private IDTOBStatusOrders _dto;
        private IInterfaceBStatusOrders _Interface;
        public clsWritableBStatusOrders(IInterfaceBStatusOrders setting, IDTOBStatusOrders dto)
        {
            _Interface = setting;
            _dto = dto;
        }



        public async Task<DTOStatusOrders?> CreateAsync(DTOStatusOrdersCRequest Request)
        {
            if (Request == null)
            { return null; }
            var dto = await _Interface.IData.AddStatusOrderAsync(Request);
            if (dto != null)
            {
                return dto;
            }
            return null;
        }
        public async Task<DTOStatusOrders?> UpdateAsync(DTOStatusOrdersURequest Request)
        {
            if (Request == null || Request.ID <= 0)
            { return null; }
           
            var dto = await _Interface.IData.UpdateStatusOrderAsync(Request);
            if (dto != null)
            {
                return dto;
            }
            return null;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _Interface.IData.DeleteStatusOrderAsync(ID);
        }
    }


    public class clsBusinessStatusOrders : IBusinessStatusOrders
    {
        private IReadableBStatusOrders _IRead;
        private IWritableBStatusOrders _IWrite;
        private IDTOBStatusOrders _DTO;

        public clsBusinessStatusOrders(IReadableBStatusOrders read, IWritableBStatusOrders write, IDTOBStatusOrders @DTO)
        {
            _IRead = read;
            _IWrite = write;
            _DTO = @DTO;
        }

        public DTOStatusOrdersCRequest? CreateRequest
        {
            get => _DTO.CreateRequest;
            set => _DTO.CreateRequest = value;
        }
        public DTOStatusOrdersURequest? UpdateRequest
        {
            get => _DTO.UpdateRequest;
            set => _DTO.UpdateRequest = value;
        }

        public async Task<List<DTOStatusOrders>> GetAllStatusOrdersAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<DTOStatusOrders?> GetStatusOrdersAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }


        public async Task<DTOStatusOrders?> AddStatusOrdersAsync(DTOStatusOrdersCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<DTOStatusOrders?> UpdateStatusOrdersAsync(DTOStatusOrdersURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteStatusOrdersAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }

    }

}
