using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using SampleApi.Elastic.Configuration;
using SampleApi.Elastic.Models;
using System;

namespace SampleApi.Elastic.Services
{
	public class ElasticService : IelasticService
	{
		private readonly ElasticsearchClient _client;
		private readonly ElasticSettings _elasticSettings;

		public ElasticService(IOptions<ElasticSettings> optionsMonitor)
		{
			_elasticSettings = optionsMonitor.Value;

			var setting = new ElasticsearchClientSettings(new Uri(_elasticSettings.Url))
				//.Authentication()
				.DefaultIndex(_elasticSettings.DefultIndex);

			_client = new ElasticsearchClient(setting);
		}


		public async Task CreateIndexIfNotExistsAsync(string indexName)
		{
			if (!_client.Indices.Exists(indexName).Exists)
				await _client.Indices.CreateAsync(indexName);
		}

		public async Task<bool> AddorUpdate(User user)
		{
			var response = await _client.IndexAsync(user, idx =>
			idx.Index(_elasticSettings.DefultIndex)
			.OpType(OpType.Index));

			return response.IsValidResponse;
		}

		public async Task<bool> AddorUpdateBulk(IEnumerable<User> users, string indexName)
		{
			var response = await _client.BulkAsync(b => b.Index(_elasticSettings.DefultIndex)
			.UpdateMany(users, (ud, u) => ud.Doc(u).DocAsUpsert(true)));

			return response.IsValidResponse;
		}

		public async Task<User> Get(string key)
		{
			var response = await _client.GetAsync<User>(key, g => g.Index(_elasticSettings.DefultIndex));

			return response.Source;
		}

		public async Task<List<User>> GetAll()
		{
			var response = await _client.SearchAsync<User>(s => s.Index(_elasticSettings.DefultIndex));

			return response.IsValidResponse ? response.Documents.ToList() : default;
		}

		public async Task<bool> Remove(string key)
		{
			var response = await _client.DeleteAsync<User>(key, a => a.Index(_elasticSettings.DefultIndex));

			return response.IsValidResponse;
		}

		public async Task<long?> RemoveAll()
		{
			var response = await _client.DeleteByQueryAsync<User>(a => a.Indices(_elasticSettings.DefultIndex));

			return response.IsValidResponse ? response.Deleted : default;
		}
	}
}
