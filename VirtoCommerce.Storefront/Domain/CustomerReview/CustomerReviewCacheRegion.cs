using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Storefront.Model.Common.Caching;

namespace VirtoCommerce.Storefront.Domain.CustomerReview
{
    public class CustomerReviewCacheRegion : CancellableCacheRegion<CustomerReviewCacheRegion>
    {
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _reviewRegionTokenLookup =
          new ConcurrentDictionary<string, CancellationTokenSource>();

        public static IChangeToken CreateChangeToken(string productId)
        {
            if (productId == null)
            {
                throw new ArgumentNullException(nameof(productId));
            }
            var cancellationTokenSource = _reviewRegionTokenLookup.GetOrAdd(productId, new CancellationTokenSource());
            return new CompositeChangeToken(new[] { CreateChangeToken(), new CancellationChangeToken(cancellationTokenSource.Token) });
        }

        public static void ExpireReview(string productId)
        {
            if (!string.IsNullOrEmpty(productId) && _reviewRegionTokenLookup.TryRemove(productId, out var token))
            {
                token.Cancel();
                token.Dispose();
            }
        }
    }
}
