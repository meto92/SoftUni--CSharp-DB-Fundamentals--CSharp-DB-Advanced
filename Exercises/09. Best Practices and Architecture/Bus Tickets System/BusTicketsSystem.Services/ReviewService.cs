using BusTicketsSystem.Data;
using BusTicketsSystem.Models;
using BusTicketsSystem.Services.Interfaces;

namespace BusTicketsSystem.Services
{
    public class ReviewService : IReviewService
    {
        private readonly BusTicketsSystemContext db;

        public ReviewService(BusTicketsSystemContext db)
        {
            this.db = db;
        }

        public Review CreateReview(int customerId, float grade, int busCompanyId, string content)
        {
            Review review = new Review
            {
                CustomerId = customerId,
                Grade = grade,
                BusCompanyId = busCompanyId,
                Content = content
            };

            this.db.Reviews.Add(review);

            this.db.SaveChanges();

            return review;
        }
    }
}