using System.Collections.Specialized;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.CustomerReviews
{
    public partial class CustomerReviewSearchCriteria : PagedSearchCriteria
    {
        private static int _defaulePageSize = 20;

        public static int DefaulePageSize
        {
            get { return _defaulePageSize; }
            set { _defaulePageSize = value; }
        }

        public CustomerReviewSearchCriteria() : base(new NameValueCollection(), DefaulePageSize)
        {
        }

        public CustomerReviewSearchCriteria(NameValueCollection queryString) : base(queryString, DefaulePageSize)
        {
        }

        public string[] ProductIds { get; set; }

        public bool? IsActive { get; set; }


        public string Sort { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
