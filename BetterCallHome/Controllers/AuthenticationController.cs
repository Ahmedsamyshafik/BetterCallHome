using BetterCallHomeWeb.Base;
using Core.Features.Users.Commands.Models;
using Core.Features.Users.Queries.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Services.Abstracts;
using System.Security.Claims;
namespace BetterCallHomeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : AppControllerBase
    {
        #region inJect
        private readonly IAuthService _auth;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IAuthService auth, IConfiguration configuration)
        {
            _auth = auth;
            _configuration = configuration;

        }
        #endregion

        #region Login-Register

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromForm] RegisterUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm] LoginUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        #endregion

        #region External Login!
        //Google+FaceBook!
        [HttpGet("[action]")]
        public async Task<IActionResult> GoogleLogin(string tok)
        {
            //try catch

            var Cid = _configuration["authentication:Google:client_id"];
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _configuration["authentication:Google:client_id"] }
            };
            // var d = await JsonWebSignature.VerifySignedTokenAsync(tok);

            var payload = await GoogleJsonWebSignature.ValidateAsync(tok, settings);
            // check payload Email?
            var email = payload.Email;



            return Ok();
        }

        #endregion

        #region Password-Email
        // For me not FrontEnd !! 
        [HttpGet("[action]")]          //confirmemailForBackEnd
        public async Task<IActionResult> confirmEmailForBackEnd([FromQuery] string userid, [FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(userid) || string.IsNullOrWhiteSpace(token))
                return NotFound();
            var result = await _auth.ConfirmEmail(userid, token);

            if (result.IsSuccess)
            {
                return Redirect($"{_configuration["AppUrl"]}/ConfirmEmail.html");
            }
            return BadRequest(result.Message);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> SendResetPassword(ForgetPasswordUserCommand command)//Send code to email
        {

            var response = await Mediator.Send(command);
            return NewResult(response);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> ConfirmResetPassword(ConfirmForgetPasswordUserCommand command)
        {

            var response = await Mediator.Send(command);
            return NewResult(response);


        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword(ResetPasswordUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

       
        //change Password 
        #endregion






    }
}
