using DataLayerRestaurant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant
{

    public interface IReadableTypeItemsBusiness
    {
        Task<List<DTOTypeItems>> GetAllTypeItems(int page);
        Task<DTOTypeItems?> LoadByID(int id);
    }

    public interface IWritableTypeItemsBusiness
    {
        Task<bool> Save();
        Task<bool> Delete();
    }

    public interface IBusinessTypeItems : IReadableTypeItemsBusiness, IWritableTypeItemsBusiness { }



    public class clsBusinessTypeItems : IBusinessTypeItems
    {
        public enum enMode
        {
            enAdd = 0,
            enUpdate
        }

        public enMode Mode { get; private set; } = enMode.enAdd;

        private DTOTypeItems? _TypeItems;
        public DTOTypeItems? TypeItems
        {
            get => _TypeItems;
            set => _TypeItems = value;
        }

        private readonly IDataTypeItems _dataLayer;

        // Constructor with DataLayer injection
        public clsBusinessTypeItems(DTOTypeItems? typeItems = null, enMode mode = enMode.enAdd)
        {
            _dataLayer = new clsDataTypeItems();
            _TypeItems = typeItems;
            Mode = mode;
        }


        // Business Layer Methods

        public async Task<bool> _Add()
        {
            if (_TypeItems == null)
            {
                return false;
            }
            _TypeItems!.TypeItemID = await _dataLayer.AddTypeItem(_TypeItems);
            return _TypeItems.TypeItemID != -1;
        }
        public async Task<bool> _Update()
        {
            if (_TypeItems == null) return false;
            return await _dataLayer.UpdateTypeItem(_TypeItems);
        }

        public async Task<bool> Save()
        {
            if (_TypeItems == null) return false;

            bool success = false;
            switch (Mode)
            {
                case enMode.enAdd:
                    success = await _Add();
                    if (success) Mode = enMode.enUpdate;
                    break;

                case enMode.enUpdate:
                    success = await _Update();
                    break;
            }
            return success;
        }

        public async Task<bool> Delete()
        {
            if (_TypeItems == null) return false;
            return await _dataLayer.DeleteTypeItem(_TypeItems.TypeItemID);
        }


        public async Task<List<DTOTypeItems>> GetAllTypeItems(int page)
        {
            IDataTypeItems dataLayer = new clsDataTypeItems();
            return await dataLayer.GetAllTypeItems(page);
        }



        public  async Task<DTOTypeItems?> LoadByID(int id)
        {
            var t = await _dataLayer.GetTypeItemById(id);
            if (t == null)
                return null;

            Mode = enMode.enUpdate;
            return t;
        }
    }
}
