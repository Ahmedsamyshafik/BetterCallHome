using BetterCallHomeWeb.Base;
using Core.Features.Apartments.Queries.Models;
using Core.Features.Users.Commands.Models;
using Core.Features.Users.Commands.Models.ApartmentsRquests;
using Core.Features.Users.Commands.Models.PaymentRequest;
using Core.Features.Users.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BetterCallHomeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> GetProfileData([FromQuery] GetProfileDataQuery query)
        {

            var response = await Mediator.Send(query);
            return NewResult(response);

            //return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RequestApartment(RequestApartmentStudentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetApartmentsMain([FromQuery]GetApartmentPagintation query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Payment(FormPaymentCommand command)
        {
          
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
