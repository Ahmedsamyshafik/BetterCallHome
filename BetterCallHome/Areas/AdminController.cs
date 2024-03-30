using BetterCallHomeWeb.Base;
using Core.Features.Apartments.Commands.Models;
using Core.Features.Apartments.Queries.Models;
using Core.Features.Users.Commands.Models;
using Core.Features.Users.Queries.Models;
using Domin.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BetterCallHomeWeb.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    [Area("Admin")]
    public class AdminController : AppControllerBase
    {
        //Reviews-Panding
        [HttpGet("[action]")]
    [Authorize(Roles = Constants.AdminRole)]

        public async Task<IActionResult> GetPendingApartments([FromQuery] GetPendingApartmentsQuery query)
        {

            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = Constants.AdminRole)]

        public async Task<IActionResult> PendingResponse([FromBody] PendingApartmentAction command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = Constants.AdminRole)]

        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUserQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = Constants.AdminRole)]

        public async Task<IActionResult> DeleteUser([FromQuery] DeleteUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

    }
}
