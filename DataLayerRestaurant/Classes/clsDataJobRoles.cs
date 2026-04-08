using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DataLayerRestaurant
{ 
    public class DTOJobRolesCRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOJobRolesCRequest(string Name, string Description) 
        {
            this.Name = Name;
            this.Description = Description;
        }
    }

    public class DTOJobRolesURequest : DTOJobRolesCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }

        public DTOJobRolesURequest(int ID , string Name, string Description) : base (Name, Description)
        {
            this.ID  = ID;
        }
    }

    public class DTOJobRoles
    {
        public int ID {  get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public DTOJobRoles(int ID, string name, string? description)
        {
            this.ID = ID;
            this.Name = name;
            this.Description = description;
        }
        public DTOJobRoles()
        {
            ID = -1;
            Name = string.Empty;
            Description = null;
        }
    }


    public class clsCompositionDJobRoles : ICompositionDataBase<DTOJobRoles>
    {
        public DTOJobRoles GetDataFromDataBase(SqlDataReader reader)
        {
            return new DTOJobRoles
            {
                ID = reader.GetInt32(reader.GetOrdinal("JobRoleID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Description"))
            };
        }
    }

    public class clsJobRolesReader: clsCompositionDJobRoles ,IReadableDJobRoles
    {
        private readonly clsMySettings _Settings;
        public clsJobRolesReader(IOptions<clsMySettings> mySettings)
        {
            _Settings = mySettings.Value;
        }

        public async Task<List<DTOJobRoles>> GetAllDataAsync(int page)
        {
            List<DTOJobRoles> result = new List<DTOJobRoles>();
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

        public async Task<DTOJobRoles?> GetDataAsync(int ID)
        {
            DTOJobRoles? result = null;
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
    
    public class clsJobRolesWriter : clsCompositionDJobRoles , IWritableDJobRoles
    {
        private readonly clsMySettings _Settings;
        public clsJobRolesWriter(IOptions<clsMySettings> mySettings)
        {
            _Settings = mySettings.Value;
        }

        public async Task<DTOJobRoles?> CreateDataAsync(DTOJobRolesCRequest DTO)
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

        public async Task<DTOJobRoles?> UpdateDataAsync(DTOJobRolesURequest DTO)
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

    public class clsDataJobRoles : IDataJobRoles
    {
        IWritableDJobRoles _IWrite;
        IReadableDJobRoles _IRead;

        public clsDataJobRoles(IWritableDJobRoles write,  IReadableDJobRoles read)
        {
            _IRead = read;
            _IWrite = write;
        }

        public async Task<List<DTOJobRoles>> GetAllJobRolesAsync(int page)
        {
           return await _IRead.GetAllDataAsync(page);
        }


        public async Task<DTOJobRoles?> GetJobRoleAsync(int ID)
        {
            return await _IRead.GetDataAsync(ID);
        }

        public async Task<DTOJobRoles?> AddJobRoleAsync(DTOJobRolesCRequest DTO)
        {
            return await _IWrite.CreateDataAsync(DTO);
        }

        public async Task<DTOJobRoles?> UpdateJobRoleAsync(DTOJobRolesURequest DTO)
        {
            return await _IWrite.UpdateDataAsync(DTO);
        }

        public async Task<bool> DeleteJobRoleAsync(int ID)
        {
            return await _IWrite.DeleteDataAsync(ID);
        }
    }
}
