using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Storefront.Infrastructure;
using VirtoCommerce.Storefront.Model.CustomerReviews;

namespace VirtoCommerce.Storefront.Controllers.Api
{
    [StorefrontApiRoute("customerreviews")]
    [ApiController]
    public class ApiCustomerReviewsController : ControllerBase
    {
        private readonly ICustomerReviewService _customerReviewService;
        public ApiCustomerReviewsController(ICustomerReviewService customerReviewService)
        {
            _customerReviewService = customerReviewService;
        }

        // storefrontapi/CustomerReviews/review
        [HttpPost("review")]
        public async Task<ActionResult> AddCustomerReview([FromBody] CustomerReview review)
        {
            await _customerReviewService.AddCustomerReviewAsync(review);
            return Ok();
        }
    }
}
