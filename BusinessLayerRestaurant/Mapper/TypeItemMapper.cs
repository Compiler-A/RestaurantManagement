using ContractsLayerRestaurant.DTOResponse;
using DomainLayer.Entities;


namespace BusinessLayerRestaurant.Mapper
{
    public static class TypeItemMapper
    {
        public static DTOTypeItemResponse ToResponse(this TypeItem typeItem)
        {
            if (typeItem == null)
                throw new ArgumentNullException(nameof(typeItem));
            return new DTOTypeItemResponse
            {
                ID = typeItem.TypeItemID,
                Name = typeItem.TypeName,
                Description = typeItem.TypeDescription
            };
        }
    }
}
