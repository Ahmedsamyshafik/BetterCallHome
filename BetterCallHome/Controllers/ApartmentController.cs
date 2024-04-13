using BetterCallHomeWeb.Base;
using Core.Features.Apartments.Commands.Models;
using Core.Features.Apartments.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BetterCallHomeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentController : AppControllerBase
    {

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> AddReact([FromBody] AddReactDTO command)
        {
            var send = new AddReactApartmentCommand()
            {
                ApartmentID = command.ApartmentID,
                Date = DateTime.UtcNow,
                UserID = command.UserID
            };
            var response = await Mediator.Send(send);
            return NewResult(response);
        }


        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] AddCommentDTO command)
        {
            var send = new AddCommentApartmentCommand()
            {
                UserID = command.UserID,
                ApartmentID = command.ApartmentID,
                Date = DateTime.UtcNow,
                Comment = command.Comment,
            };
            var response = await Mediator.Send(send);
            return NewResult(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetApartmentDetails([FromQuery]GetApartmentDetailQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

    }
}
