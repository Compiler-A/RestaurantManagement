

using ContractsLayerRestaurant.DTOResponse;
using DomainLayer.Entities;

namespace BusinessLayerRestaurant.Mapper
{
    public static class JobRoleMapper
    {
        public static DTOJobRoleResponse ToResponse(this JobRole jobRole)
        {
            if (jobRole == null)
                throw new ArgumentNullException(nameof(jobRole));

            return new DTOJobRoleResponse
            {
                ID = jobRole.JobRoleID,
                Name = jobRole.JobName,
                Description = jobRole.Description
            };
        }
    }
}
