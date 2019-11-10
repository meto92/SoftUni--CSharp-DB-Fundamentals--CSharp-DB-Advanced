namespace Stations.Models
{
    public class TrainSeats
    {
        public int Id { get; private set; }

        public int Quantity { get; set; }

        public int TrainId { get; set; }

        public int SeatingClassId { get; set; }

        public virtual Train Train { get; set; }

        public virtual SeatingClass SeatingClass { get; set; }
    }
}