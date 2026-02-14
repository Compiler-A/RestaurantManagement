using DataLayerRestaurant;

namespace BusinessLayerRestaurant
{
    public interface IReadableStatusTablesBusiness
    {
        Task<List<DTOStatusTables>> GetAll(int page);
        Task<DTOStatusTables?> LoadByID(int id);
    }

    public interface IWritableStatusTablesBusiness
    {
        Task<bool> Save();
        Task<bool> Delete(int ID);

    }

    public interface IBusinessStatusTables : IReadableStatusTablesBusiness, IWritableStatusTablesBusiness {}

    public class clsBusinessStatusTables : IBusinessStatusTables
    {
        public enum enMode { Add = 0, Update }
        public enMode Mode { get; private set; }

        private DTOStatusTables? _StatusTable;
        public DTOStatusTables? StatusTable
        {
            get { return _StatusTable; }
            set { _StatusTable = value; }
        }

        private readonly IDataStatusTables _dataLayer;

        public clsBusinessStatusTables(IDataStatusTables dataLayer, DTOStatusTables? statusTable = null, enMode mode = enMode.Add)
        {
            _dataLayer = dataLayer; 
            _StatusTable = statusTable;
            Mode = mode;
        }

        public clsBusinessStatusTables() : this(new clsDataStatusTables())
        {
            Mode = enMode.Add;
        }

        public async Task<List<DTOStatusTables>> GetAll(int page)
        {
            return await _dataLayer.GetAllStatustables(page);
        }

        public async Task<DTOStatusTables?> LoadByID(int id)
        {
            var dto = await _dataLayer.GetStatusTableID(id);
            if (dto == null) 
                return null;

            _StatusTable = dto;
            Mode = enMode.Update;
            return _StatusTable;
        }

        public async Task<bool> Save()
        {
            if (_StatusTable == null) return false;

            if (Mode == enMode.Add)
            {
                int newID = await _dataLayer.AddStatusTable(_StatusTable);
                if (newID != -1)
                {
                    _StatusTable.StatusTableID = newID;
                    Mode = enMode.Update; 
                    return true;
                }
                return false;
            }
            else 
            {
                return await _dataLayer.UpdateStatusTable(_StatusTable);
            }
        }


        public async Task<bool> Delete(int ID)
        {
            return await _dataLayer.DeleteStatusTable(ID);
        }

    }
}