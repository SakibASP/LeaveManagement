using LeaveManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LeaveManagement.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IConfiguration config) : ControllerBase
    {
        private readonly string _key = config["Jwt:Key"];

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            // Hardcoded username/password check for demo
            if (userLogin.Username == "admin" && userLogin.Password == "123")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                    [
                        new(ClaimTypes.Name, userLogin.Username)
                    ]),
                    Expires = DateTime.Now.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token) });
            }

            return Unauthorized();
        }
    }
}
