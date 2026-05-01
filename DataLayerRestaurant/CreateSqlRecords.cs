using Microsoft.Data.SqlClient.Server;
using System.Data;


namespace DataLayerRestaurant
{
    public static class CreateSqlRecords
    {
        public static IEnumerable<SqlDataRecord> CreateSqlRecord(IEnumerable<int> ids)
        {
            var metaData = new[]
            {
                new SqlMetaData("Id", SqlDbType.Int)
            };

            foreach (var id in ids)
            {
                var record = new SqlDataRecord(metaData);
                record.SetInt32(0, id);
                yield return record;
            }
        }
    }
}


