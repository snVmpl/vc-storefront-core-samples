using reviewDto = VirtoCommerce.Storefront.AutoRestClients.CustomerReviewsModuleModuleApi.Models;
using reviewEntity = VirtoCommerce.Storefront.Model.CustomerReviews;

namespace VirtoCommerce.Storefront.Domain.CustomerReview
{
    public static partial class CustomerReviewConverter
    {
        public static reviewEntity.CustomerReview ToCustomerReview(this reviewDto.CustomerReview reviewDto)
        {
            var result = new reviewEntity.CustomerReview
            {
                AuthorNickname = reviewDto.AuthorNickname,
                Content = reviewDto.Content,
                Pros = reviewDto.Pros,
                Cons = reviewDto.Cons,
                CreatedBy = reviewDto.CreatedBy,
                CreatedDate = reviewDto.CreatedDate,
                IsActive = reviewDto.IsActive,
                ModifiedBy = reviewDto.ModifiedBy,
                ModifiedDate = reviewDto.ModifiedDate,
                ProductId = reviewDto.ProductId,
                RatingNumber = reviewDto.RatingNumber,
                Id = reviewDto.Id
            };

            return result;
        }

        public static reviewDto.CustomerReview ToCustomerReviewDto(this reviewEntity.CustomerReview reviewDto)
        {
            var result = new reviewDto.CustomerReview
            {
                AuthorNickname = reviewDto.AuthorNickname,
                Content = reviewDto.Content,
                Pros = reviewDto.Pros,
                Cons = reviewDto.Cons,
                CreatedBy = reviewDto.CreatedBy,
                CreatedDate = reviewDto.CreatedDate,
                IsActive = reviewDto.IsActive,
                ModifiedBy = reviewDto.ModifiedBy,
                ModifiedDate = reviewDto.ModifiedDate,
                ProductId = reviewDto.ProductId,
                RatingNumber = reviewDto.RatingNumber,
                Rating = reviewDto.Rating.ToString(),
                Id = reviewDto.Id
            };

            return result;
        }

        public static reviewDto.CustomerReviewSearchCriteria ToSearchCriteriaDto(this reviewEntity.CustomerReviewSearchCriteria criteria)
        {
            var result = new reviewDto.CustomerReviewSearchCriteria
            {
                IsActive = criteria.IsActive,
                ProductIds = criteria.ProductIds,

                Skip = criteria.Skip,
                Take = criteria.Take,
                Sort = criteria.Sort
            };

            return result;
        }
    }
}
