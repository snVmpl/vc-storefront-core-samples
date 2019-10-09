using System.Threading.Tasks;
using PagedList.Core;

namespace VirtoCommerce.Storefront.Model.CustomerReviews
{
    public interface ICustomerReviewService
    {
        IPagedList<CustomerReview> SearchReview(CustomerReviewSearchCriteria criteria);
        Task<IPagedList<CustomerReview>> SearchReviewAsync(CustomerReviewSearchCriteria criteria);

        double? GetProductRating(string productId);
        Task<double?> GetProductRatingAsync(string productId);

        void AddCustomerReview(CustomerReview review);

        Task AddCustomerReviewAsync(CustomerReview review);
    }
}
