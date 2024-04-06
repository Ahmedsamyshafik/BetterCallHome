using BetterCallHomeWeb.Base;
using Core.Features.Apartments.Commands.Models;
using Core.Features.Apartments.Queries.Models;
using Core.Features.Users.Commands.Models.ApartmentsRquests;
using Core.Features.Users.Queries.Models;
using Domin.Constant;
using Infrastructure.DTO.Owner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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


        [HttpGet("[action]")]
        [Authorize(Roles = Constants.OwnerRole)]
        public async Task<IActionResult> GetOwnerApartments([FromQuery] GetApartmentsForOwnerQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpDelete("[action]")] // Here ! 
        [Authorize(Roles = Constants.OwnerRole)]
        public async Task<IActionResult> DeleteApartment([FromQuery] DeleteApartmentDTO command)
        {
            var send = new DeleteApartmentCommand()
            {
                ApartmentId = command.ApartmentId,
                userID = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value
            };
            var response = await Mediator.Send(send);
            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetApartmentsRequests([FromQuery] GetApartmentsRequestsQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ApartmentsRequestsHandle(ActionsRequestedApartmentStudentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOwnerStudents([FromQuery] GetOwnerStudents query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> EditApartment([FromForm] EditApartmentDTO DTO)
        {
            //User Token !! id !
            var command = new EditApartmentCommand()
            {
                price = DTO.price,
                Address = DTO.Address,
                ApartmentId = DTO.ApartmentId,
                CoverImage = DTO.CoverImage,
                Description = DTO.Description,
                numberOfUsers = DTO.numberOfUsers,
                Pics = DTO.Pics,
                Video = DTO.Video,
                Title = DTO.Title,
                Requesthost = Request.Host,
                RequestScheme = Request.Scheme
            };
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

    }
}
