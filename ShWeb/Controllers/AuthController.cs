using DataModels.Data;
using DataModels.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataModels.Services;

namespace ShWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] DataModels.Models.LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized("Invalid username or password.");

            var token = _jwtTokenService.GenerateToken(user.UserName, user.Email, user.UserId.ToString());
            return Ok(new { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] DataModels.Models.RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest("Password and Confirm Password do not match.");
            }

            if (string.IsNullOrWhiteSpace(request.UserName))
            {
                return BadRequest(new { Errors = new[] { "UserName is required." } });
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // Generate JWT token
                var token = _jwtTokenService.GenerateToken(user.UserName, user.Email, user.Id.ToString());

                // Return token in the response
                return Ok(new { Token = token });
            }

            // If registration failed, return errors
            return BadRequest(new { Errors = result.Errors.Select(e => e.Description).ToArray() });
        }
    }
}