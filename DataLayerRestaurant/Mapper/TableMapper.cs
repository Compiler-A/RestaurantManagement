using DomainLayer.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

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
                StatusTable = new StatusTable
                {
                    Name = Reader.GetString(Reader.GetOrdinal("StatusTableName")),
                }
            };
        }
    }
}
