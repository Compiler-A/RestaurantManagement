using ContractsLayerRestaurant.DTOs.StatusTables;


namespace ContractsLayerRestaurant.DTOs.Tables
{
    public class DTOTables
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Seats { get; set; }
        public int StatusTableID { get; set; }
        public DTOStatusTables? StatusTable { get; set; }

        public DTOTables()
        {
            ID = -1;
            Name = string.Empty;
            Seats = -1;
            StatusTable = null;
            StatusTableID = -1;
        }
        public DTOTables(int iD, string table, int seats, int statusTableID)
        {
            ID = iD;
            Name = table;
            Seats = seats;
            StatusTableID = statusTableID;
        }
    }
}
