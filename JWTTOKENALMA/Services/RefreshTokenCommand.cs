using JWTTOKENALMA.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace JWTTOKENALMA.Services
{
	public class RefreshTokenCommand
	{
		private readonly IConfiguration _configuration;
		public string RefreshToken { get; set; }
		public RefreshTokenCommand(IConfiguration configuration) {
			_configuration = configuration;
		}

		public Token Handle()
		{
			Users user = new Users();
			using (var _context = new OrionDataContext())
			{

				user = _context.Users.Where(x => x.RefreshToken == RefreshToken && x.RefreshTokenExpireDate > DateTime.Now).FirstOrDefault();

				if (user != null)
				{
					//Token Yarat
					TokenHandler handler = new TokenHandler(_configuration);
					Token token = handler.CreateAccessToken(user);
					user.RefreshTokenExpireDate = token.Expiration.AddMinutes(5);
					user.RefreshToken = token.RefreshToken;
					_context.SaveChanges();

					return token;
				}
				else
				{
					throw new InvalidOperationException("Kullanıcı Adı ve Şifre Hatalı");
				}
			}
		}
	}
}
