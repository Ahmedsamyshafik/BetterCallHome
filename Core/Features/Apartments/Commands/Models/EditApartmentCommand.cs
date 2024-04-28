using Core.Bases;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Features.Apartments.Commands.Models
{
    public class EditApartmentCommand : IRequest<Response<String>>
    {
        public int ApartmentId { get; set; }    
        public string Title { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }
        public int numberOfUsers { get; set; }
        public decimal price { get; set; }
        public int Room { get; set; }
        public string gender { get; set; }
        public string City { get; set; }
        public IFormFile? NewCoverImage { get; set; }

        public List<string>? ApartmentsImagesUrl { get; set; }
        public List<IFormFile>? NewPics { get; set; }

        public IFormFile? NewVideo { get; set; }


        public string RequestScheme { get; set; }
        public HostString Requesthost { get; set; }

    }
}
