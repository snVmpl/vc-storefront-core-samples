using System.Linq;
using System.Threading.Tasks;
using PagedList.Core;
using VirtoCommerce.Storefront.AutoRestClients.CustomerReviewsModuleModuleApi;
using VirtoCommerce.Storefront.Infrastructure;
using VirtoCommerce.Storefront.Model.Caching;
using VirtoCommerce.Storefront.Model.Common.Caching;
using VirtoCommerce.Storefront.Model.CustomerReviews;
using CustomerReviewDto = VirtoCommerce.Storefront.AutoRestClients.CustomerReviewsModuleModuleApi.Models.CustomerReview;
using CutomerReviewModel = VirtoCommerce.Storefront.Model.CustomerReviews.CustomerReview;

namespace VirtoCommerce.Storefront.Domain.CustomerReview
{
    public class CustomerReviewService : ICustomerReviewService
    {
        private readonly ICustomerReviewsModule _customerReviews;
        private readonly IStorefrontMemoryCache _memoryCache;
        private readonly IApiChangesWatcher _apiChangesWatcher;

        public CustomerReviewService(ICustomerReviewsModule customerReviews, IStorefrontMemoryCache memoryCache, IApiChangesWatcher apiChangesWatcher)
        {
            _customerReviews = customerReviews;
            _memoryCache = memoryCache;
            _apiChangesWatcher = apiChangesWatcher;
        }

        public IPagedList<CutomerReviewModel> SearchReview(CustomerReviewSearchCriteria criteria)
        {
            return this.SearchReviewAsync(criteria).GetAwaiter().GetResult();
        }

        public async Task<IPagedList<CutomerReviewModel>> SearchReviewAsync(CustomerReviewSearchCriteria criteria)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(SearchReviewAsync), criteria.GetCacheKey());
            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.ExpirationTokens.Add(CustomerReviewCacheRegion.CreateChangeToken(criteria.ProductIds.FirstOrDefault()));
                cacheEntry.ExpirationTokens.Add(_apiChangesWatcher.CreateChangeToken());

                var result = await _customerReviews.SearchCustomerReviewsAsync(criteria.ToSearchCriteriaDto());
                return new StaticPagedList<CutomerReviewModel>(result.Results.Select(c => c.ToCustomerReview()),
                    criteria.PageNumber, criteria.PageSize, result.TotalCount.Value);
            }
            );
        }

        public double? GetProductRating(string productId)
        {
            return this.GetProductRatingAsync(productId).GetAwaiter().GetResult();
        }

        public async Task<double?> GetProductRatingAsync(string productId)
        {
            double? result = null;
            var cacheKey = CacheKey.With(GetType(), nameof(GetProductRatingAsync), productId);
            result = await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.ExpirationTokens.Add(CustomerReviewCacheRegion.CreateChangeToken(productId));
                cacheEntry.ExpirationTokens.Add(_apiChangesWatcher.CreateChangeToken());
                return await _customerReviews.ProductRatingAsync(productId);
            });

            return result;
        }

        public void AddCustomerReview(CutomerReviewModel review)
        {
            this.AddCustomerReviewAsync(review).GetAwaiter().GetResult();
        }

        public async Task AddCustomerReviewAsync(CutomerReviewModel review)
        {
            await _customerReviews.UpdateAsync(new CustomerReviewDto[] { CustomerReviewConverter.ToCustomerReviewDto(review) });
            CustomerReviewCacheRegion.ExpireReview(review.ProductId);
        }
    }
}
