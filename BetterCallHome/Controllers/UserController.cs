using BetterCallHomeWeb.Base;
using Core.Features.Users.Commands.Models;
using Core.Features.Users.Queries.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BetterCallHomeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : AppControllerBase
    {

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword(ChangePasswordUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> EditProfile([FromForm] EditProfileDTO command)
        {
            var send = new EditProfileUserCommand()
            {
                Email = command.Email,
                Name = command.Name,
                Phone = command.Phone,
                Picture = command.Picture,
                RequestScheme = Request.Scheme,
                Requesthost = Request.Host
            };
            var response = await Mediator.Send(send);
            return NewResult(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProfileData([FromQuery] GetProfileDTO query)
        {
            var send = new GetProfileDataQuery()
            {
                RequesterUserID = HttpContext.User.FindFirstValue("uid"),
                UserId = query.UserId
            };
            var response = await Mediator.Send(send);
            return NewResult(response);

            //return Ok();
        }

    }
}
