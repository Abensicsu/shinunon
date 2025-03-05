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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ShWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
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

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not authenticated.");
            }

            return Ok(currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] DataModels.Models.ChangePasswordRequest  request)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not authenticated.");
            }

            var result = await _userManager.ChangePasswordAsync(currentUser, request.OldPassword, request.NewPassword);

            if (result.Succeeded)
            {
                return Ok("Password changed successfully.");
            }

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description).ToArray() });
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] DataModels.Models.LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized("Invalid username or password.");

            var token = _jwtTokenService.GenerateToken(user.UserName, user.Email, user.Id.ToString(), user.UserSettings);
            return Ok(new { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] DataModels.Models.RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserFullName))
            {
                return BadRequest(new { Errors = new[] { "UserName is required." } });
            }

            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { Errors = new[] { $"Email '{request.Email}' is already registered." } });
            }

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                UserFullName = request.UserFullName,
                UserSettings = new UserSettings
                {
                    DefaultQuestionCount = 10,
                    MemoryLevel = MemoryLevelEnum.Medium,
                }
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // Generate JWT token
                var token = _jwtTokenService.GenerateToken(user.UserName, user.Email, user.Id.ToString(), user.UserSettings);

                // Return token in the response
                return Ok(new { Token = token });
            }

            // If registration failed, return errors
            return BadRequest(new { Errors = result.Errors.Select(e => e.Description).ToArray() });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }


        [HttpGet]
        public async Task<IActionResult> GetUserSettings()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.Users
                .Include(u => u.UserSettings) // load UserSettings
                .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (user == null)
                return Unauthorized();

            return Ok(user.UserSettings ?? new UserSettings()); // Return default settings if null
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUserSettings([FromBody] UserSettings updatedSettings)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.Users
                .Include(u => u.UserSettings) // Ensure UserSettings is loaded
                .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (user == null)
                return Unauthorized();

            if (user.UserSettings == null)
            {
                user.UserSettings = new UserSettings(); // Initialize if missing
            }

            // Update user settings
            user.UserSettings.IsSingleChapterMode = updatedSettings.IsSingleChapterMode;
            user.UserSettings.MemoryLevel = updatedSettings.MemoryLevel;
            user.UserSettings.DefaultQuestionCount = updatedSettings.DefaultQuestionCount;
            user.UserSettings.DefaultTimeExam = updatedSettings.DefaultTimeExam;

            // Save changes
            await _userManager.UpdateAsync(user);

            return Ok();
        }
    }
}