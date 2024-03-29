﻿namespace CarDealer.Models
{
    public class PartCar
    {
        public int PartId { get; set; }

        public int CarId { get; set; }

        public virtual Part Part { get; set; }

        public virtual Car Car { get; set; }
    }
}