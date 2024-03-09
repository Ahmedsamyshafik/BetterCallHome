using BetterCallHomeWeb.Base;
using Core.Features.Users.Commands.Models;
using Microsoft.AspNetCore.Mvc;

namespace BetterCallHomeWeb.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    [Area("Admin")]
    public class AdminController : AppControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> LoginForAdmins([FromBody] LoginUserAdminCommand model)
        {
            var response = await Mediator.Send(model);
            return NewResult(response);

        }
    }
}
