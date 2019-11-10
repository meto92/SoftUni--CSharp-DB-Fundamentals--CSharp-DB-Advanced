using BusTicketsSystem.Models;

namespace BusTicketsSystem.Services.Interfaces
{
    public interface IReviewService
    {
        Review CreateReview(int customerId, float grade, int busCompanyId, string content);
    }
}