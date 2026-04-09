using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTOs.StatusTables
{
    public class DTOStatusTables
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public DTOStatusTables()
        {
            ID = -1;
            Name = string.Empty;
        }
        public DTOStatusTables(int statusTableID, string statusTableName)
        {
            ID = statusTableID;
            Name = statusTableName;
        }
    }
}
