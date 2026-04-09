using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTOs.StatusOrders
{
    public class DTOStatusOrders
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public DTOStatusOrders()
        {
            ID = -1;
            Name = string.Empty;
        }
        public DTOStatusOrders(int idStatusOrder, string statusOrderName)
        {
            this.ID = idStatusOrder;
            this.Name = statusOrderName;
        }
    }
}
