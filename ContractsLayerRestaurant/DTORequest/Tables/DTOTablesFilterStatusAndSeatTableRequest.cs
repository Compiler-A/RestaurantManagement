using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTORequest.Tables
{
    public class DTOTablesFilterStatusAndSeatTableRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be a positive integer.")]
        public int Page { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "StatusTableID must be a positive integer.")]
        public int StatusTableID { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Seats must be a positive integer.")]
        public int Seats { get; set; }
    }
}
