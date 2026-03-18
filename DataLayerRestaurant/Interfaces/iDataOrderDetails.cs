using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{
    public interface IReadableDOrderDetails : IReadableDataBase<DTOOrderDetails>
    {
        Task<List<DTOOrderDetails>> GetAllDataByOrderIDAsync(int orderID);
    }
    public interface IWritableDOrderDetails : IWritableDataBase<DTOOrderDetails,DTOOrderDetailsCRequest, DTOOrderDetailsURequest>
    {
    }

    public interface IWritableDataOrderDetails
    {
        Task<DTOOrderDetails?> AddOrderDetailAsync(DTOOrderDetailsCRequest Request);
        Task<DTOOrderDetails?> UpdateOrderDetailAsync(DTOOrderDetailsURequest Request);
        Task<bool> DeleteOrderDetailAsync(int id);
    }

    public interface IReadableDataOrderDetails
    {
        Task<List<DTOOrderDetails>> GetAllOrderDetailsAsync(int page);
        Task<List<DTOOrderDetails>> GetAllOrderDetailsByOrderIDAsync(int orderID);
        Task<DTOOrderDetails?> GetOrderDetailAsync(int ID);

    }

    public interface IDataOrderDetails : IReadableDataOrderDetails, IWritableDataOrderDetails
    {
    }
}
