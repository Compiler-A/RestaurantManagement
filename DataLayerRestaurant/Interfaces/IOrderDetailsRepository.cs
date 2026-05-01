using RestaurantDataLayer;
using ContractsLayerRestaurant.DTORequest.OrderDetails;
using DomainLayer.Entities;

namespace DataLayerRestaurant.Interfaces
{
    public interface IOrderDetailsRepositoryLoader
    {
        Task LoadDataAsync(List<OrderDetail> items);
    }

    public interface IOrderDetailsRepositoryReader : IRepositoryReader<OrderDetail>
    {
        Task<List<OrderDetail>> GetAllDataByOrderIDAsync(int orderID);
    }
    public interface IOrderDetailsRepositoryWriter : IRepositoryWriter<OrderDetail,DTOOrderDetailsCRequest, DTOOrderDetailsURequest>
    { }


    public interface IOrderDetailsRepository : IOrderDetailsRepositoryReader, IOrderDetailsRepositoryWriter
    {
    }
}
