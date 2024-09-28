using Microsoft.AspNetCore.Mvc;
using SampleApi.Elastic.Models;
using SampleApi.Elastic.Services;

namespace SampleApi.Elastic.Controller
{
	[Controller]
	[Route("[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly ILogger<UsersController> _logger;
		private readonly IelasticService _ielasticService;

		public UsersController(ILogger<UsersController> logger, IelasticService ielasticService)
		{
			_logger = logger;
			_ielasticService = ielasticService;
		}

		[HttpPost("craete-index")]
		public async Task<IActionResult> CreateIndex(string indexName)
		{
			await _ielasticService.CreateIndexIfNotExistsAsync(indexName);
			return Ok($"Index {indexName} created or alredy exist");
		}

		[HttpPost("add-user")]
		public async Task<IActionResult> AddUser([FromBody] User user)
		{
			var result = await _ielasticService.AddorUpdate(user);
			return result ? Ok("User addes or update successfuly.") : StatusCode(500 , "Error adding or updating user.");
		}

		[HttpPost("update-user")]
		public async Task<IActionResult> UpdateUser([FromBody] User user)
		{
			var result = await _ielasticService.AddorUpdate(user);
			return result ? Ok("User addes or update successfuly.") : StatusCode(500, "Error adding or updating user.");
		}

		[HttpGet("get-user/{key}")]
		public async Task<IActionResult> GetUser( string key)
		{
			var user = await _ielasticService.Get(key);
			return user != null ? Ok(user) : NotFound("users not found");
		}



		[HttpGet("get-all-user")]
		public async Task<IActionResult> GetAllUser()
		{
			var users = await _ielasticService.GetAll();
			return users != null ? Ok(users) : StatusCode(500, "error retriving users");
		}

		[HttpDelete("delate-user/{key}")]
		public async Task<IActionResult> DelateUser(string key)
		{
			var result = await _ielasticService.Remove(key);
			return result != null ? Ok("User delated successfully") : StatusCode(500, "Error delating user");
		}

		[HttpDelete("delate-all-user")]
		public async Task<IActionResult> DelateAllUser()
		{
			var result = await _ielasticService.RemoveAll();
			return result != null ? Ok() : StatusCode(500, "Error delating user");
		}

	}
}
