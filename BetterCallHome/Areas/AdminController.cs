using BetterCallHomeWeb.Base;
using Core.Features.Apartments.Queries.Models;
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
        public async Task<IActionResult> GetPendingApartments()
        {
            var query = new GetPendingApartmentsQuery();
            var response = await Mediator.Send(query);
            return NewResult(response);
        }
    }
}
