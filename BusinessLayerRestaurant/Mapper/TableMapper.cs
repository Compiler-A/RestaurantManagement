using ContractsLayerRestaurant.DTOResponse;
using DomainLayer.Entities;


namespace BusinessLayerRestaurant.Mapper
{
    public static class TableMapper
    {
        public static DTOTableResponse ToResponse(this Table table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));
            return new DTOTableResponse
            {
                ID = table.ID,
                Name = table.Name,
                Seats = table.Seats,
                StatusTableID = table.StatusTableID,
                StatusTable = table.StatusTable?.ToResponse()
            };
        }
    }
}
