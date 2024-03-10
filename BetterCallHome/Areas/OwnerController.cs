using BetterCallHomeWeb.Base;
using Core.Features.Apartments.Commands.Models;
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

    }
}
