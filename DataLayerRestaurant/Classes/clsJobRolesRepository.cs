using ContractsLayerRestaurant.DTORequest.JobRoles;
using DataLayerRestaurant.Interfaces;
using DataLayerRestaurant.Mapper;
using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System.Data;



namespace DataLayerRestaurant.Classes
{ 

    public class clsJobRolesRepositoryReader: IJobRolesRepositoryReader
    {
        private readonly clsMySettings _Settings;
        public clsJobRolesRepositoryReader(IOptions<clsMySettings> mySettings)
        {
            _Settings = mySettings.Value;
        }
        

        public async Task<List<JobRole>> GetAllDataAsync(List<int> Ids)
        {
            List<JobRole> result = new List<JobRole>();
            using (SqlConnection Connection = new SqlConnection(_Settings.ConnectionString))
            {
                using (SqlCommand Command = new SqlCommand("JobRoles.SP_GetAllJobRolesByIds", Connection))
                {
                    Command.CommandType = System.Data.CommandType.StoredProcedure;

                    var param = new SqlParameter("@Ids", SqlDbType.Structured)
                    {
                        TypeName = "dbo.IntList",
                        Value = CreateSqlRecords.CreateSqlRecord(Ids)
                    };
                    Command.Parameters.Add(param);

                    await Connection.OpenAsync();
                    using (SqlDataReader reader = await Command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(JobRoleMapper.ReaderToEntity(reader));
                        }
                    }
                }
            }
            return result;
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
                            result.Add(JobRoleMapper.ReaderToEntity(reader));
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
                            result = (JobRoleMapper.ReaderToEntity(reader));
                        }
                    }
                }
            }
            return result;
        }
    }
    
    public class clsJobRolesRepositoryWriter : IJobRolesRepositoryWriter
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
                            return (JobRoleMapper.ReaderToEntity(reader));
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
                            return (JobRoleMapper.ReaderToEntity(reader));
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

        public async Task<List<JobRole>> GetAllDataAsync(int page)
        {
           return await _IRead.GetAllDataAsync(page);
        }

        public async Task<List<JobRole>> GetAllDataAsync(List<int> Ids)
        {
            return await _IRead.GetAllDataAsync(Ids);
        }

        public async Task<JobRole?> GetDataAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<JobRole?> CreateDataAsync(DTOJobRolesCRequest DTO)
        {
            return await _IWrite.CreateDataAsync(DTO);
        }

        public async Task<JobRole?> UpdateDataAsync(DTOJobRolesURequest DTO)
        {
            return await _IWrite.UpdateDataAsync(DTO);
        }

        public async Task<bool> DeleteDataAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }
    }
}
