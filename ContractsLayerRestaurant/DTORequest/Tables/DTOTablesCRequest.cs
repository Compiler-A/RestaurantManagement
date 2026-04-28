using System.ComponentModel.DataAnnotations;


namespace ContractsLayerRestaurant.DTORequest.Tables
{
    public class DTOTablesCRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Seats must be a positive integer.")]
        public int Seats { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "StatusTableID must be a positive integer.")]
        public int StatusTableID { get; set; }

        public DTOTablesCRequest(int Seats, int StatusTableID)
        {
            this.Seats = Seats;
            this.StatusTableID = StatusTableID;
        }
    }
}
