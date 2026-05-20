using ContractsLayerRestaurant.DTORequest.OrderDetails;
using DomainLayer.Entities;

namespace ContractsLayerRestaurant.Interfaces.Repositories
{

    public interface IOrderDetailsRepositoryReader : IRepositoryReader<OrderDetail>
    {
        Task<List<OrderDetail>> GetAllDataByOrderIDAsync(int orderID);
        Task<List<OrderDetail>> GetAllDataByOrderIdsAsync(List<int> orderIDs);
    }
    public interface IOrderDetailsRepositoryWriter : IRepositoryWriter<OrderDetail,DTOOrderDetailsCRequest, DTOOrderDetailsURequest>
    { }


    public interface IOrderDetailsRepository : IOrderDetailsRepositoryReader, IOrderDetailsRepositoryWriter
    {
    }
}
