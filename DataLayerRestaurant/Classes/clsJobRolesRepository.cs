using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using DataLayerRestaurant.Interfaces;
using ContractsLayerRestaurant.DTORequest.JobRoles;



namespace DataLayerRestaurant.Classes
{ 

    public class clsJobRolesRepositoryComposition : ICompositionDataBase<JobRole>
    {
        public JobRole GetDataFromDataBase(SqlDataReader reader)
        {
            return new JobRole
            {
                ID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description"))
            };
        }
    }

    public class clsJobRolesRepositoryReader: clsJobRolesRepositoryComposition ,IJobRolesRepositoryReader
    {
        private readonly clsMySettings _Settings;
        public clsJobRolesRepositoryReader(IOptions<clsMySettings> mySettings)
        {
            _Settings = mySettings.Value;
        }

        public async Task<List<JobRole>> GetAllDataAsync(int page)
        {
            List<JobRole> result = new List<JobRole>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("JobRoles.SP_GetAllJobRoles", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Page", page);
                    Command.Parameters.AddWithValue("@Rows", _Settings.RowsPerPage);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(GetDataFromDataBase(reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<JobRole?> GetDataAsync(int ID)
        {
            JobRole? result = null;
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("JobRoles.SP_GetJobRoleByID", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = (GetDataFromDataBase(reader));
                        }
                    }
                }
            }
            return result;
        }
    }
    
    public class clsJobRolesRepositoryWriter : clsJobRolesRepositoryComposition , IJobRolesRepositoryWriter
    {
        private readonly clsMySettings _Settings;
        public clsJobRolesRepositoryWriter(IOptions<clsMySettings> mySettings)
        {
            _Settings = mySettings.Value;
        }

        public async Task<JobRole?> CreateDataAsync(DTOJobRolesCRequest DTO)
        {
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("JobRoles.SP_AddJobRole", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", DTO.Name);
                    Command.Parameters.AddWithValue("@Description", (object?)DTO.Description ?? DBNull.Value);

                    await Connection.OpenAsync(); 
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return (GetDataFromDataBase(reader));
                        }
                    }
                }
            }
            return null;
        }

        public async Task<JobRole?> UpdateDataAsync(DTOJobRolesURequest DTO)
        {
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("JobRoles.SP_UpdateJobRole", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@Name", DTO.Name);
                    Command.Parameters.AddWithValue("@Description", (object?)DTO.Description ?? DBNull.Value);
                    Command.Parameters.AddWithValue("@ID", DTO.ID);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return (GetDataFromDataBase(reader));
                        }
                    }
                }
            }
            return null;
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            bool Deleted = false;
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("JobRoles.SP_DeleteJobRole", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ID", ID);

                    await Connection.OpenAsync();
                    int Row = await Command.ExecuteNonQueryAsync();

                    Deleted = Row > 0;
                }
            }
            return Deleted;
        }
    }

    public class clsJobRolesRepository : IJobRolesRepository
    {
        IJobRolesRepositoryWriter _IWrite;
        IJobRolesRepositoryReader _IRead;

        public clsJobRolesRepository(IJobRolesRepositoryWriter write,  IJobRolesRepositoryReader read)
        {
            _IRead = read;
            _IWrite = write;
        }

        public async Task<List<JobRole>> GetAllJobRolesAsync(int page)
        {
           return await _IRead.GetAllDataAsync(page);
        }


        public async Task<JobRole?> GetJobRoleAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<JobRole?> AddJobRoleAsync(DTOJobRolesCRequest DTO)
        {
            return await _IWrite.CreateDataAsync(DTO);
        }

        public async Task<JobRole?> UpdateJobRoleAsync(DTOJobRolesURequest DTO)
        {
            return await _IWrite.UpdateDataAsync(DTO);
        }

        public async Task<bool> DeleteJobRoleAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }
    }
}
