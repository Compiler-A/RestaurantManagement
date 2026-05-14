using ContractsLayerRestaurant.DTOResponse;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Mapper
{
    public static class StatusTableMapper
    {
        public static DTOStatusTableResponse ToResponse(this StatusTable statusTable)
        {
            if (statusTable == null)
                throw new ArgumentNullException(nameof(statusTable));
            return new DTOStatusTableResponse
            {
                ID = statusTable.StatusTableID,
                Name = statusTable.StatusTableName,
            };
        }
    }
}
