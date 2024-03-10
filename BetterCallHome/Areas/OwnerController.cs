using BetterCallHomeWeb.Base;
using Core.Features.Apartments.Commands.Models;
using Core.Features.Users.Commands.Models;
using Microsoft.AspNetCore.Mvc;

namespace BetterCallHomeWeb.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    [Area("Owner")]
    public class OwnerController : AppControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> LoginForOwner([FromBody] LoginUserOwnerCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> AddApartment([FromForm] AddApartmentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        //  [HttpGet]

    }
}
