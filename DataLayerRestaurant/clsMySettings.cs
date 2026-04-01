using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerRestaurant
{
    public class clsMySettings
    {
        public string ConnectionString { get; set; } = null!;
        public int RowsPerPage { get; set; }
    }
}
