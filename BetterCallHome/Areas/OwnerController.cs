using BetterCallHomeWeb.Base;
using Core.Features.Apartments.Commands.Models;
using Core.Features.Apartments.Queries.Models;
using Domin.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BetterCallHomeWeb.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    [Area("Owner")]
    public class OwnerController : AppControllerBase
    {

        [HttpPost("[action]")]
        [Authorize(Roles = Constants.OwnerRole)]
        public async Task<IActionResult> AddApartment([FromForm] AddApartmentDTO command)
        {
            var send = new AddApartmentCommand()
            {
                Address = command.Address,
                CoverImage = command.CoverImage,
                Description = command.Description,
                Name = command.Name,
                NumberOfUsers = command.NumberOfUsers,
                Pics = command.Pics,
                Price = command.Price,
                Requesthost = Request.Host,
                RequestScheme = Request.Scheme,
                Room = command.Room,
                RoyalDocument = command.RoyalDocument,
                UserId = command.UserId,
                video = command.video,
                CreatedAt = DateTime.UtcNow
            };
            var response = await Mediator.Send(send);
            return NewResult(response);
        }


        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationApartmentQuery Query)
        {
            var response = await Mediator.Send(Query);
            return NewResult(response);
        }
    }
}
