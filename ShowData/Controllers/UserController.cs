using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShowData.Model;
using ShowData.Repository.IRepository;

namespace ShowData.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        /// <summary>
        /// Authenticating user.
        /// </summary>
        /// <param name="userDetails">Must contain user name and password.</param>
        /// <returns>Http Ok if success, http bad request if fail.</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationDetails userDetails)
        {
            var user = _userRepository.AuthenticateUser(userDetails.Username, userDetails.Password);
            if(user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest(new { message = "Invalid user details"});
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthenticationDetails userDetails)
        {
            if (!_userRepository.IsUserNameTaken(userDetails.Username))
            {
                _userRepository.RegisterUser(userDetails.Username, userDetails.Password);
                return Ok();
            }
            else
                return BadRequest(new { message = "Username is taken." });
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<string>))]
        public IActionResult GetUserNames()
        {
            var usernamesList = _userRepository.GetUsernames();
            return Ok(usernamesList);

        }
    }
}
