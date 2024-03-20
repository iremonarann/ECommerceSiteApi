using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Northwind.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Northwind.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        readonly IConfiguration _configuration;
        readonly IDataProtector _dataProtector;

        public AccountsController(IConfiguration configuration, IDataProtectionProvider dataProtectorProvider)
        {
            _configuration = configuration;
            _dataProtector = dataProtectorProvider.CreateProtector("tse");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            ///TODO: Veri kaynağından kontrol edilecek
            bool isValidUser = loginModel.Username == "salihdemirog" && loginModel.Password == "12345";

            if (!isValidUser)
                return Problem(title: "Kullanıcı Bulunamadı", detail: "Kullanıcı adı veya şifre yanlış", statusCode: 400);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,"1"),
                new Claim(ClaimTypes.Name,loginModel.Username),
                new Claim(ClaimTypes.GivenName,"Salih Demiroğ"),
                new Claim(ClaimTypes.Email,"salihdemirog@gmail.com"),
                new Claim(ClaimTypes.Role,"moderator"),
                new Claim(ClaimTypes.Role,"admin"),
            };

            var key = _configuration.GetValue<string>("Authentication:Jwt:SecretKey");
            var issuer = _configuration.GetValue<string>("Authentication:Jwt:Issuer");

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credential = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credential,
                claims: claims);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var protectJwtToken = _dataProtector.Protect(jwtToken);
            return Ok(new
            {
                Token = protectJwtToken,
                Type = "Bearer"
            });
        }
    }
}
