using RestaurantDataLayer;
using ContractsLayerRestaurant.DTOs.OrderDetails;

namespace DataLayerRestaurant.Interfaces
{
    public interface IOrderDetailsRepositoryReader : IReadableDataBase<DTOOrderDetails>
    {
        Task<List<DTOOrderDetails>> GetAllDataByOrderIDAsync(int orderID);
    }
    public interface IOrderDetailsRepositoryWriter : IWritableDataBase<DTOOrderDetails,DTOOrderDetailsCRequest, DTOOrderDetailsURequest>
    {
    }

    public interface IOrderDetailsRepositoryWritable
    {
        Task<DTOOrderDetails?> AddOrderDetailAsync(DTOOrderDetailsCRequest Request);
        Task<DTOOrderDetails?> UpdateOrderDetailAsync(DTOOrderDetailsURequest Request);
        Task<bool> DeleteOrderDetailAsync(int id);
    }

    public interface IOrderDetailsRepositoryReadable
    {
        Task<List<DTOOrderDetails>> GetAllOrderDetailsAsync(int page);
        Task<List<DTOOrderDetails>> GetAllOrderDetailsByOrderIDAsync(int orderID);
        Task<DTOOrderDetails?> GetOrderDetailAsync(int ID);

    }

    public interface IOrderDetailsRepository : IOrderDetailsRepositoryReadable, IOrderDetailsRepositoryWritable
    {
    }
}
