using Microsoft.AspNetCore.Mvc;

namespace MutualFundNAVTrigger.NETWebAPI.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using global::MutualFundNAVTrigger.NETWebAPI.Models;
    using global::MutualFundNAVTrigger.NETWebAPI.Models.Auth;

    namespace MutualFundNAVTrigger.NETWebAPI.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IConfiguration _config;

            public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config)
            {
                _userManager = userManager;
                _config = config;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register(RegisterRequest request)
            {
                var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                return Ok("User registered successfully");
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login(LoginRequest request)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                    return Unauthorized("Invalid credentials");

                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }

            private string GenerateJwtToken(ApplicationUser user)
            {
                var jwtKey = _config["Jwt:Key"];
                var jwtIssuer = _config["Jwt:Issuer"];

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var creds = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: jwtIssuer,
                    audience: jwtIssuer,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }

}
