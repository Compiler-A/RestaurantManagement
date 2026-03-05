using DataLayerRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{
    public interface IReadableTablesBusiness
    {
        Task<List<DTOTables>> GetAll(int page);
        Task<List<DTOTables>> GetAll();
        Task<List<DTOTables>> GetMenuTables(int Page, int StatusTable);
        Task<List<DTOTables>> GetFilterTables(int Page, int Seats);
        Task<DTOTables?> LoadByID(int id);
        Task<DTOTables?> LoadByTableNumber(string tableNumber);
        Task<bool> IsFindStatus(int id);
        Task<List<DTOTables>?> GetTableWithFilteringData(int page, int StatusTable, int SeatNumber);
        
        Task<List<DTOTables>> GetAllTablesAvailables();
    }

    public interface IWritableTablesBusiness
    {
        Task<bool> Save();
        Task<bool> Delete(int ID);
    }

    public interface IDataTablesBusiness : IReadableTablesBusiness, IWritableTablesBusiness
    { 
    }

    public class clsBusinessTables : IDataTablesBusiness
    {
        public enum enMode { Add = 0, Update }
        public enMode Mode { get; private set; }

        private DTOTables? _dtoTables;

        public DTOTables? DTOTables
        {
            get { return _dtoTables; }
            set { _dtoTables = value; }
        }

        private readonly IDataTables _DataTables;
        private readonly IDataStatusTables _DataStatusTables;

        public clsBusinessTables(DTOTables? dtoTables = null, enMode Mode = enMode.Add)
        {
            _DataTables = new clsDataTables();
            _dtoTables = dtoTables;
            _DataStatusTables = new clsDataStatusTables();
            this.Mode = Mode;
        }

        public clsBusinessTables()
        {
            Mode = enMode.Add;
            _DataTables = new clsDataTables();
            _DataStatusTables = new clsDataStatusTables();
        }

        private async Task LoadRelatedObjectsAsync()
        {
            DTOTables!.StatusTable = await _DataStatusTables.GetStatusTableID(DTOTables.StatusTableID);
        }

        public async Task<bool> IsFindStatus(int id)
        {
            var status = await _DataStatusTables.isFindStatus(id);
            return status;
        }


        private async Task<List<DTOTables>> _GetStatusTableID(List<DTOTables> dto)
        {
            foreach (var item in dto)
            {
                item.StatusTable = await _DataStatusTables.GetStatusTableID(item.StatusTableID);
            }
            return dto;
        }

        public async Task<List<DTOTables>?> GetTableWithFilteringData(int page, int StatusTable, int SeatNumber)
        {
            List<DTOTables>? dto = await _DataTables.GetAllTablesWithFilteringData(page, StatusTable, SeatNumber);
            if (dto == null)
                return null;
            return await _GetStatusTableID(dto);
        }
        public async Task<List<DTOTables>> GetAll(int page)
        {
            List<DTOTables> dto = await  _DataTables.GetAlltables(page);
            return await _GetStatusTableID(dto);
            
        }
        public async Task<List<DTOTables>> GetAllTablesAvailables()
        {
            List<DTOTables> dto = await _DataTables.GetTablesAvailables();
            return await _GetStatusTableID(dto);
        }

        public async Task<List<DTOTables>> GetAll()
        {
            List<DTOTables> dto = await _DataTables.GetAlltables();
            return await _GetStatusTableID(dto);
        }

        public async Task<List<DTOTables>> GetMenuTables(int Page, int StatusTable)
        {
            List<DTOTables> dto = await _DataTables.GetMenuTables(Page, StatusTable);
            return await _GetStatusTableID(dto);
        }

        public async Task<List<DTOTables>> GetFilterTables(int page, int Seats)
        {
            List<DTOTables> dto = await _DataTables.GetFilterTables(page, Seats);
            return await _GetStatusTableID(dto);
        }

        public async Task<DTOTables?> LoadByID(int id)
        {
            _dtoTables = await _DataTables.GetTableID(id);

            if (_dtoTables == null)
            {
                return null;
            }
            await LoadRelatedObjectsAsync();
            Mode = enMode.Update;
            return _dtoTables;
        }

        public async Task<DTOTables?> LoadByTableNumber(string TableNumber)
        {
            _dtoTables = await _DataTables.GetTableByTableNumber(TableNumber);
            if (_dtoTables == null)
            {
                return null;
            }
            await LoadRelatedObjectsAsync();
            Mode = enMode.Update;
            return _dtoTables;
        }

        public async Task<bool> Delete(int ID)
        {
            return await _DataTables.DeleteTable(ID);
        }

        private async Task<bool> _Add()
        {
            (DTOTables!.ID,DTOTables!.Table) = await _DataTables.AddTable(DTOTables!);
            return DTOTables.ID != -1;
        }

        private async Task<(bool, string)> _Update()
        {
            return await _DataTables.UpdateTable(DTOTables!);
        }

        public async Task<bool> Save()
        {
            bool result = false;

            switch (Mode)
            {
                case enMode.Add:
                    if (await _Add())
                    {
                        Mode = enMode.Update; 
                        result = true;
                    }
                    break;
                case enMode.Update:
                    (result, DTOTables!.Table) = await _Update();
                    break;
            }

            if (result)
            {
                await LoadRelatedObjectsAsync();
            }
            return result;
        }
    }
}
