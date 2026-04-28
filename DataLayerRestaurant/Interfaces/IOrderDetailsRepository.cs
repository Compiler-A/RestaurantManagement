using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.OrderDetails;

namespace DataLayerRestaurant.Interfaces
{
    public interface IOrderDetailsRepositoryReader : IReadableDataBase<OrderDetail>
    {
        Task<List<OrderDetail>> GetAllDataByOrderIDAsync(int orderID);
    }
    public interface IOrderDetailsRepositoryWriter : IWritableDataBase<OrderDetail,DTOOrderDetailsCRequest, DTOOrderDetailsURequest>
    {
    }

    public interface IOrderDetailsRepositoryWritable
    {
        Task<OrderDetail?> AddOrderDetailAsync(DTOOrderDetailsCRequest Request);
        Task<OrderDetail?> UpdateOrderDetailAsync(DTOOrderDetailsURequest Request);
        Task<bool> DeleteOrderDetailAsync(int id);
    }

    public interface IOrderDetailsRepositoryReadable
    {
        Task<List<OrderDetail>> GetAllOrderDetailsAsync(int page);
        Task<List<OrderDetail>> GetAllOrderDetailsByOrderIDAsync(int orderID);
        Task<OrderDetail?> GetOrderDetailAsync(int ID);

    }

    public interface IOrderDetailsRepository : IOrderDetailsRepositoryReadable, IOrderDetailsRepositoryWritable
    {
    }
}
