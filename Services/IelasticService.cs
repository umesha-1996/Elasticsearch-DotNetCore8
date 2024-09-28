using SampleApi.Elastic.Models;

namespace SampleApi.Elastic.Services
{
	public interface IelasticService
	{
		//create index
		Task CreateIndexIfNotExistsAsync(string indexName);

		//add or update user
		Task<bool> AddorUpdate(User user);

		// add or update bulk
		Task<bool> AddorUpdateBulk(IEnumerable<User> users, string indexName);

		//get user
		Task<User> Get(string key);

		// get All user
		Task<List<User>> GetAll();

		//remove
		Task<bool> Remove(string key);

		//removeall
		Task<long?> RemoveAll();
	}
}
