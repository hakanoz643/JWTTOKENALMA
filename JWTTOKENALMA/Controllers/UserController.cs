using JWTTOKENALMA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTTOKENALMA.Controllers
{
	[Authorize]
	[ApiController]
	[Route("Api/[controller]")]
	public class UserController : Controller
	{
		[HttpGet("GetUsers")]
		public List<Users> Get()
		{
			var result = new List<Users>();
			using (var erp = new OrionDataContext())
			{
				result = erp.Users.ToList();
			}
			return result;
		}

		[HttpGet("GetUser/{id}")]
		public int Get(int id)
		{
			return id;
		}
	}
}
