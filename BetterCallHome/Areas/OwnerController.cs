using BetterCallHomeWeb.Base;
using Core.Features.Apartments.Commands.Models;
using Core.Features.Apartments.Queries.Models;
using Microsoft.AspNetCore.Mvc;

namespace BetterCallHomeWeb.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    [Area("Owner")]
    public class OwnerController : AppControllerBase
    {

        [HttpPost("[action]")]
        // [Authorize(Roles = Constants.OwnerRole)]
        public async Task<IActionResult> AddApartment([FromForm] AddApartmentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        //  [HttpGet]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddReact([FromBody] AddReactApartmentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> AddComment([FromBody] AddCommentApartmentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetNotifications(GetNotificationApartmentQuery Query)
        {
            var response = await Mediator.Send(Query);
            return NewResult(response);
        }
    }
}
