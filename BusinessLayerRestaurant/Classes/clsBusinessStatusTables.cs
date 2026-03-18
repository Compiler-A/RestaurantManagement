using DataLayerRestaurant;


namespace BusinessLayerRestaurant
{
    public class clsDTOBStatusTables : IDTOBStatusTables
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

    public class clsInterfaceBStatusTables : IInterfaceBStatusTables
    {
        private IDataStatusTables _IStatusTable;
        public IDataStatusTables IData
        {
            get => _IStatusTable;
            set => _IStatusTable = value;
        }
        public clsInterfaceBStatusTables(IDataStatusTables iStatusTable)
        {
            _IStatusTable = iStatusTable;
        }
    }

    public class clsReadableBStatusTables : IReadableBStatusTables
    {
        private IInterfaceBStatusTables _Interface;
        public clsReadableBStatusTables(IInterfaceBStatusTables iInterface)
        {
            _Interface = iInterface;
        }

        public async Task<DTOStatusTables?> GetAsync(int ID)
        {
            return await _Interface.IData.GetStatuTableAsync(ID);
        }

        public async Task<List<DTOStatusTables>> GetAllAsync(int page)
        {
            return await _Interface.IData.GetAllStatustablesAsync(page);
        }

        public async Task<bool> isFindAsync(int ID)
        {
            return await _Interface.IData.isFindAsync(ID);
        }

    }

    public class clsWritableBStatusTables : IWritableBStatusTables
    {
        private IDTOBStatusTables _dto;
        private IInterfaceBStatusTables _Interface;
        public clsWritableBStatusTables(IInterfaceBStatusTables setting, IDTOBStatusTables dto)
        {
            _Interface = setting;
            _dto = dto;
        }



        public async Task<DTOStatusTables?> CreateAsync(DTOStatusTablesCRequest Request)
        {
            if (Request == null)
            { return null; }
            var dto = await _Interface.IData.AddStatusTableAsync(Request);
            if (dto != null)
            {
                return dto;
            }
            return null;
        }
        public async Task<DTOStatusTables?> UpdateAsync(DTOStatusTablesURequest Request)
        {
            if (Request == null || Request.ID <= 0)
            { return null; }

            var dto = await _Interface.IData.UpdateStatusTableAsync(Request);
            if (dto != null)
            {
                return dto;
            }
            return null;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
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