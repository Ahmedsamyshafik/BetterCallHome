using Core.Bases;
using Core.Features.Apartments.Commands.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Features.Apartments.Commands.Models
{
    public class AddApartmentCommand : IRequest<Response<AddApartmentResponse>>
    {

        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Room { get; set; }
        public int NumberOfUsers { get; set; }
        public IFormFile RoyalDocument { get; set; }
        public IFormFile? video { get; set; }
        public IFormFile CoverImage { get; set; }
        public ICollection<IFormFile> Pics { get; set; }
        public string? Address { get; set; }
        public string UserId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public string RequestScheme { get; set; }
        public HostString Requesthost { get; set; }
    }
}
