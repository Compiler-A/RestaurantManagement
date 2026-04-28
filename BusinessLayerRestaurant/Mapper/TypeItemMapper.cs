using ContractsLayerRestaurant.DTOResponse;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ID = typeItem.ID,
                Name = typeItem.Name,
                Description = typeItem.Description
            };
        }
    }
}
