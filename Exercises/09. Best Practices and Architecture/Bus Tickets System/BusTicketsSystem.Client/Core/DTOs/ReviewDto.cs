using System;

namespace BusTicketsSystem.Client.Core.DTOs
{
    public class ReviewDto
    {
        public int Id { get; private set; }

        public float Grade { get; set; }

        public DateTime PublishedOn { get; set; }

        public string Content { get; set; }

        public string CustomerFullName { get; set; }
    }
}