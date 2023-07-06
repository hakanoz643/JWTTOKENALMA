using JWTTOKENALMA.Models;
using JWTTOKENALMA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTTOKENALMA.Controllers
{
	[ApiController]
	[Route("Api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public AuthController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpPost("CreateToken")]
		public ActionResult<Token> CreateToken([FromBody] Users login)
		{
			CreateTokenCommand command = new CreateTokenCommand(_configuration);
			command.Model = new CreateTokenCommand.CreateTokenModel()
			{
				Username = login.Username,
				Password = login.Password,
			};
			var token = command.Handle();
			return token;
		}

		[HttpGet("RefreshToken")]
		public ActionResult<Token> RefreshToken([FromQuery]string token)
		{
			RefreshTokenCommand command = new RefreshTokenCommand(_configuration);
			command.RefreshToken = token;
			var resultToken = command.Handle();

			return resultToken;
		}
	}
}