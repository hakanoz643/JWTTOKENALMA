using JWTTOKENALMA.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTTOKENALMA.Services
{
	public class TokenHandler
	{
		private readonly IConfiguration _configuration;

		public TokenHandler(IConfiguration configuration) {
			_configuration = configuration;
		}

		public Token CreateAccessToken(Users user)
		{
			Token tokenModel = new Token();

			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

			SigningCredentials signingCredentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256); 

			tokenModel.Expiration = DateTime.Now.AddMinutes(3);
			var claims = new[]
			{
				new Claim("UserId",user.Id.ToString()),
				new Claim("Email",user.Email.ToString()),
			};

			JwtSecurityToken securityToken = new JwtSecurityToken(
				issuer: _configuration["Token:Issuer"],
				audience: _configuration["Token:Audience"],
				claims:claims,
				expires:tokenModel.Expiration,
				notBefore:DateTime.Now,
				signingCredentials: signingCredentials
			);

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

			tokenModel.AccessToken = tokenHandler.WriteToken(securityToken);
			tokenModel.RefreshToken = CreateRefrestToken();

			return tokenModel;
		}

		public string CreateRefrestToken()
		{
			return Guid.NewGuid().ToString();
		}
	}
}