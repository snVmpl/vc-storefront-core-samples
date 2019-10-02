using System.Linq;
using System.Threading.Tasks;
using PagedList.Core;
using VirtoCommerce.Storefront.AutoRestClients.CustomerReviewsModuleModuleApi;
using VirtoCommerce.Storefront.Infrastructure;
using VirtoCommerce.Storefront.Model.Caching;
using VirtoCommerce.Storefront.Model.Common.Caching;
using VirtoCommerce.Storefront.Model.CustomerReviews;

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

        public IPagedList<Model.CustomerReviews.CustomerReview> SearchReview(CustomerReviewSearchCriteria criteria)
        {
            return this.SearchReviewAsync(criteria).GetAwaiter().GetResult();
        }

        public async Task<IPagedList<Model.CustomerReviews.CustomerReview>> SearchReviewAsync(CustomerReviewSearchCriteria criteria)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(SearchReviewAsync), criteria.GetCacheKey());
            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.ExpirationTokens.Add(CustomerReviewCacheRegion.CreateChangeToken());
                cacheEntry.ExpirationTokens.Add(_apiChangesWatcher.CreateChangeToken());

                var result = await _customerReviews.SearchCustomerReviewsAsync(criteria.ToSearchCriteriaDto());
                return new StaticPagedList<Model.CustomerReviews.CustomerReview>(result.Results.Select(c => c.ToCustomerReview()),
                    criteria.PageNumber, criteria.PageSize, result.TotalCount.Value);
            }
            );
        }
    }
}
