
using Domin.Models;
using Domin.ViewModel;
using Google.Apis.Auth;
using Infrastructure.DTO;
using Infrastructure.IRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BetterCallHomeWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region inJect
        private readonly IAuthService _auth;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IAuthService auth, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _auth = auth;
            _configuration = configuration;
            _userManager = userManager;
        }
        #endregion
        //UserDto

        // Filteration for DTO !!  Done 

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _auth.RegisterAsync(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }


            return Ok(result);
        }
        //UserDto
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _auth.Login(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LoginForAdmins([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _auth.LoginForAdmin(model);

            if (!result.IsAuthenticated) { return BadRequest(result.Message); }
            return Ok(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> LoginForOwners([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _auth.LoginForOwners(model);
            if (!result.IsAuthenticated) { return BadRequest(result.Message); }
            return Ok(result);
        }

        // 404/ Redirect
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
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();
            var result = await _auth.ForgetPassword(email);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        //From Email!!
        ////404//UserMangerResponse{message} ?? Redirct From Email!
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

        // Normal Reset Password..! 
        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO reset)
        {
            if (ModelState.IsValid)
            {

                var result = await _auth.ResetPassword(reset);
                if (result.IsSuccess)
                {
                    return Ok(result.Message);
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }

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

        //[HttpGet("[action]")]
        //public IActionResult JustTest(string _name)
        //{
        // Anoynmous object replacing of DTO
        //    return Ok(new { name = _name, age = 12 });
        //}



    }
}
