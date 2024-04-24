using Core.Wrappers;

namespace Core.Features.Apartments.Queries.Results
{
    public class GetApartmentDetailResponse
    {
        public ICollection<string> ApartmentsImages { get; set; }
        public string? ApartmentsVideo { get; set; }
        public string ApartmentName { get; set; }
        public decimal ApartmentPrice { get; set; }
        public string ApartmentAddress { get; set; }
        public string ApartmentCity { get; set; }
        public string ApartmentDescription { get; set; }
        public int TotalCount { get; set; }
        public int StudnetExistingIn { get; set; }
        public int ApartmentLikes { get; set; }

        public string OwnerName { get; set; }
        public string OwnerImageUrl { get; set; }
        public int OwnerApartmentCount { get; set; }
        public PaginatedResult<ApartmentCommentsResponse> ApartmentComments { get; set; }
    }
    //list Of Comments
    public class ApartmentCommentsResponse
    {
        public int CommentID { get; set; }
        public string CommentValue { get; set; }
        public string UserCommentName { get; set; }

        public string UserCommentImageUrl { get; set; }

        public string? CurrentUserImage { get; set; }
    }
}
