using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CopyRecipeBookApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly IConfiguration _config;
        public LoginController(IConfiguration config, SignInManager<IdentityUser> signInManager)
        {
			_signInManager = signInManager; 
			_config = config;
        }
        [AllowAnonymous]
		[HttpPost]
		public IActionResult Login([FromBody] UserModel loginModel)
		{
			IActionResult respone = Unauthorized();
			var success = AuthenticateUser(loginModel);

			if (success) 
			{
				var tokenString = GenerateJsonWebToken(loginModel);
				respone = Ok(new {token = tokenString});
			}
			return respone;
		}
		private string GenerateJsonWebToken(UserModel loginModel)
		{
			var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credential = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], null, expires: DateTime.Now.AddMinutes(120), signingCredentials: credential);
			return new JwtSecurityTokenHandler().WriteToken(token);						
		}
		private bool AuthenticateUser(UserModel loginModel)
		{
			var result = _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, true, lockoutOnFailure: false).Result;
			return result.Succeeded;
		}
	}
}
