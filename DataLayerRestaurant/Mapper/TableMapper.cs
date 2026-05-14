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
                TableID = Reader.GetInt32(Reader.GetOrdinal("TableID")),
                TableNumber = Reader.GetString(Reader.GetOrdinal("TableNumber")),
                Seats = Reader.GetInt32(Reader.GetOrdinal("Seats")),
                StatusTableID = Reader.GetInt32(Reader.GetOrdinal("StatusTableID"))
            };
        }

        public static Table ReaderToEntityResult(SqlDataReader Reader)
        {
            return new Table
            {
                TableID = Reader.GetInt32(Reader.GetOrdinal("TableID")),
                TableNumber = Reader.GetString(Reader.GetOrdinal("TableNumber")),
                Seats = Reader.GetInt32(Reader.GetOrdinal("Seats")),
                StatusTableID = Reader.GetInt32(Reader.GetOrdinal("StatusTableID")),
                StatusTable = new StatusTable
                {
                    StatusTableID = Reader.GetInt32(Reader.GetOrdinal("StatusTableID")),
                    StatusTableName = Reader.GetString(Reader.GetOrdinal("StatusTableName")),
                }
            };
        }
    }
}
