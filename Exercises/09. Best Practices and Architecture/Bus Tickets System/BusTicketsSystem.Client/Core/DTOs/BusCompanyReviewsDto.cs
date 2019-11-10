using System.Collections.Generic;

namespace BusTicketsSystem.Client.Core.DTOs
{
    public class BusCompanyReviewsDto
    {
        public ICollection<ReviewDto> Reviews { get; set; }
    }
}