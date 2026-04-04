using DataLayerRestaurant;
using System.Globalization;


namespace BusinessLayerRestaurant
{
    public class clsStatusTablesDtoContainer : IDTOBStatusTables
    {
        private DTOStatusTablesCRequest? _CRequest;

        public DTOStatusTablesCRequest? CreateRequest
        {
            get => _CRequest;
            set => _CRequest = value;
        }

        private DTOStatusTablesURequest? _URequest;
        public DTOStatusTablesURequest? UpdateRequest
        {
            get => _URequest;
            set => _URequest = value;
        }
    }

    public class clsStatusTablesRepositoryBridge : IInterfaceBStatusTables
    {
        private IDataStatusTables _IStatusTable;
        public IDataStatusTables IData
        {
            get => _IStatusTable;
            set => _IStatusTable = value;
        }
        public clsStatusTablesRepositoryBridge(IDataStatusTables iStatusTable)
        {
            _IStatusTable = iStatusTable;
        }
    }

    public class clsStatusTablesReader : IReadableBStatusTables
    {
        private IInterfaceBStatusTables _Interface;
        public clsStatusTablesReader(IInterfaceBStatusTables iInterface)
        {
            _Interface = iInterface;
        }

        public async Task<DTOStatusTables?> GetAsync(int ID)
        {
            var result = await _Interface.IData.GetStatuTableAsync(ID);
            if (result == null)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return result;
        }

        public async Task<List<DTOStatusTables>> GetAllAsync(int page)
        {
            var result = await _Interface.IData.GetAllStatustablesAsync(page);
            if (result == null || result.Count == 0)
                throw new KeyNotFoundException("Not Found!");

            return result;
        }

        public async Task<bool> isFindAsync(int ID)
        {
            var result = await _Interface.IData.isFindAsync(ID);
            if (!result)
            {
                throw new KeyNotFoundException("Not Found!");
            }
            return result;
        }

    }

    public class clsStatusTablesWriter : IWritableBStatusTables
    {
        private IInterfaceBStatusTables _Interface;
        public clsStatusTablesWriter(IInterfaceBStatusTables setting)
        {
            _Interface = setting;
        }



        public async Task<DTOStatusTables?> CreateAsync(DTOStatusTablesCRequest Request)
        {

            var dto = await _Interface.IData.AddStatusTableAsync(Request);
            if (dto != null)
            {
                return dto;
            }
            throw new InvalidOperationException("Not Created!");
        }
        public async Task<DTOStatusTables?> UpdateAsync(DTOStatusTablesURequest Request)
        {

            var dto = await _Interface.IData.UpdateStatusTableAsync(Request);
            if (dto != null)
            {
                return dto;
            }
            throw new InvalidOperationException("Not Updated!");
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            var result = await _Interface.IData.DeleteStatusTableAsync(ID);
            if (!result)
            {
                throw new InvalidOperationException("Not Deleted!");
            }
            return await _Interface.IData.DeleteStatusTableAsync(ID);
        }
    }



    public class clsBusinessStatusTables : IBusinessStatusTables
    {
        private IReadableBStatusTables _IRead;
        private IWritableBStatusTables _IWrite;
        private IDTOBStatusTables _DTO;

        public clsBusinessStatusTables(IReadableBStatusTables read, IWritableBStatusTables write, IDTOBStatusTables @DTO)
        {
            _IRead = read;
            _IWrite = write;
            _DTO = @DTO;
        }

        public DTOStatusTablesCRequest? CreateRequest
        {
            get => _DTO.CreateRequest;
            set => _DTO.CreateRequest = value;
        }
        public DTOStatusTablesURequest? UpdateRequest
        {
            get => _DTO.UpdateRequest;
            set => _DTO.UpdateRequest = value;
        }

        public async Task<List<DTOStatusTables>> GetAllStatusTablesAsync(int page)
        {
            return await _IRead.GetAllAsync(page);
        }

        public async Task<DTOStatusTables?> GetStatusTableAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }

        public async Task<bool> isFindStatusTableAsync(int id)
        {
            return await _IRead.isFindAsync(id);
        }


        public async Task<DTOStatusTables?> AddStatusTableAsync(DTOStatusTablesCRequest Request)
        {
            return await _IWrite.CreateAsync(Request);
        }
        public async Task<DTOStatusTables?> UpdateStatusTableAsync(DTOStatusTablesURequest Request)
        {
            return await _IWrite.UpdateAsync(Request);
        }

        public async Task<bool> DeleteStatusTableAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}