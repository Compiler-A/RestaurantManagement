using DataLayerRestaurant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{
    public interface IReadableStatusMenusBusiness
    {
        Task<List<DTOStatusMenus>> GetAll(int page);
        Task<DTOStatusMenus?> LoadByID(int id);
    }

    public interface IWritableStatusMenusBusiness
    {
        Task<bool> Save();
        Task<bool> Delete(int ID);
    }

    public interface IBusinessStatusMenus : IReadableStatusMenusBusiness, IWritableStatusMenusBusiness { }

    public class clsBusinessStatusMenus : IBusinessStatusMenus
    {
        public enum enMode
        {
            enAdd = 0,
            enUpdate = 1
        }

        private readonly IDataStatusMenus _dataStatusMenus;

        public enMode Mode { get; private set; } = enMode.enAdd;

        private DTOStatusMenus? _statusMenus;
        public DTOStatusMenus? StatusMenus
        {         
            get => _statusMenus;
            set => _statusMenus = value;
        }


        public clsBusinessStatusMenus(DTOStatusMenus? statusMenus = null,  enMode mode = enMode.enAdd)
        {
            _dataStatusMenus = new clsDataStatusMenus();
            _statusMenus = statusMenus;
            Mode = mode;
        }

        // ========================= GET =========================

        public async Task<List<DTOStatusMenus>> GetAll(int page)
        {
            return await _dataStatusMenus.GetAllStatusMenus(page);
        }

        public async Task<DTOStatusMenus?> LoadByID(int id)
        {
            var dto = await _dataStatusMenus.GetStatusMenusByID(id);

            if (dto == null)
                return null;

            _statusMenus = dto;
            Mode = enMode.enUpdate;
            return _statusMenus;
        }


        // ========================= SAVE =========================

        private async Task<bool> _Add()
        {
            if (_statusMenus == null)
                return false;

            int newID = await _dataStatusMenus.AddStatusMenus(_statusMenus);

            if (newID == -1)
                return false;

            _statusMenus.StatusMenuID = newID;
            return true;
        }

        private async Task<bool> _Update()
        {
            if (_statusMenus == null)
                return false;

            return await _dataStatusMenus.UpdateStatusMenu(_statusMenus);
        }

        public async Task<bool> Save()
        {
            bool result = false;

            switch (Mode)
            {
                case enMode.enAdd:
                    result = await _Add();
                    if (result)
                        Mode = enMode.enUpdate;
                    break;

                case enMode.enUpdate:
                    result = await _Update();
                    break;
            }

            return result;
        }

        // ========================= DELETE =========================

        public async Task<bool> Delete(int id)
        {
            return await _dataStatusMenus.DeleteStatusMenu(id);
        }
    }
}
