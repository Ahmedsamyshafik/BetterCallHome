
using BetterCallHomeWeb.Base;
using Core.Features.Users.Commands.Models;
using Core.Features.Users.Queries.Models;
using Domin.ViewModel;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Services.Abstracts;
using System.Security.Claims;

namespace BetterCallHomeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : AppControllerBase
    {
        #region inJect
        private readonly IAuthService _auth;
        private readonly IConfiguration _configuration;

        public AccountController(IAuthService auth, IConfiguration configuration)
        {
            _auth = auth;
            _configuration = configuration;

        }
        #endregion

        #region Login-Register

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterStudentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
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
        public async Task<IActionResult> confirmemailForBackEnd([FromQuery] string userid, [FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(userid) || string.IsNullOrWhiteSpace(token))
                return NotFound();
            var result = await _auth.ConfirmEmail(userid, token);

            if (result.IsSuccess)
            {
                return Redirect($"{_configuration["AppUrl"]}/ConfirmEmail.html");
            }
            return BadRequest(result);
        }

        //404//UserMangerResponse{message}
        [HttpPost("[action]")]
        public async Task<IActionResult> ForgetPassword(ChangePasswordUserCommand command) // Send Email
        {
            var response = await Mediator.Send(command);
            return NewResult(response);

        }

        //From Email!!
        ////?? Redirct From  page ForgetPassword !
        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPasswordFromEmailForBackEnd([FromForm] ReserPasswordVM model)
        {

            if (ModelState.IsValid)
            {

                var result = await _auth.ResetPasswordForEmail(model);
                if (result.IsSuccess)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }
            return BadRequest("Some Properties are not valid :(");
        }

        //// Normal Reset Password..! 
        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword(ResetPasswordUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        #endregion

        #region Edit && GetAccount
        [HttpPost("[action]")]
        public async Task<IActionResult> EditProfileStudentandOwner([FromForm] EditProfileStudentandOwnerCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetProfileData([FromQuery] GetProfileDataQuery query)
        {

            query.RequesterUserID = HttpContext.User.FindFirstValue("uid");
            var response = await Mediator.Send(query);
            return NewResult(response);

            //return Ok();
        }
        #endregion





    }
}
