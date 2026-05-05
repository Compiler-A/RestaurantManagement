using DomainLayer.Entities;
using Microsoft.Data.SqlClient;


namespace DataLayerRestaurant.Mapper
{
    public class TableMapper
    {
        public static Table ReaderToEntity(SqlDataReader Reader)
        {
            return new Table
            {
                ID = Reader.GetInt32(Reader.GetOrdinal("TableID")),
                Name = Reader.GetString(Reader.GetOrdinal("TableNumber")),
                Seats = Reader.GetInt32(Reader.GetOrdinal("Seats")),
                StatusTableID = Reader.GetInt32(Reader.GetOrdinal("StatusTableID"))
            };
        }

        public static Table ReaderToEntityResult(SqlDataReader Reader)
        {
            return new Table
            {
                ID = Reader.GetInt32(Reader.GetOrdinal("TableID")),
                Name = Reader.GetString(Reader.GetOrdinal("TableNumber")),
                Seats = Reader.GetInt32(Reader.GetOrdinal("Seats")),
                StatusTableID = Reader.GetInt32(Reader.GetOrdinal("StatusTableID")),
                StatusTable = new StatusTable
                {
                    ID = Reader.GetInt32(Reader.GetOrdinal("StatusTableID")),
                    Name = Reader.GetString(Reader.GetOrdinal("StatusTableName")),
                }
            };
        }
    }
}
