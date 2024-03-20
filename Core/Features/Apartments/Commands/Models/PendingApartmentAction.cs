using Core.Bases;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Apartments.Commands.Models
{
    public class PendingApartmentAction :IRequest<Response<string>>
    {
        [Required]
        public int ApartmentID { get; set; }
        [Required]
        public bool Accept { get; set; }
    }
}
