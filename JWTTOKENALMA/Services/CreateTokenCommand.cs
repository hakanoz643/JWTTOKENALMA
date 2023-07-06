using JWTTOKENALMA.Models;

namespace JWTTOKENALMA.Services
{
	public class CreateTokenCommand
	{
		private readonly IConfiguration _configuration;

		public CreateTokenModel Model { get; set; }

		public CreateTokenCommand(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public Token Handle()
		{
			Users user = new Users();
			using (var _context = new OrionDataContext())
			{

				user = _context.Users.Where(x => x.Username == Model.Username && x.Password == Model.Password).FirstOrDefault();

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


		public class CreateTokenModel
		{
            public string Username { get; set; }
            public string Password { get; set; }
		}
	}
}
