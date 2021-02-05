using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JokeGenerator
{
    public interface IJsonFeed
    {
        Task<string[]> GetRandomJokesAsync(string firstname, string lastname, string category);
        Task<dynamic> GetNameAsync();
        Task<string[]> GetCategoriesAsync();
    }

    public class JsonFeed : IJsonFeed
    {
        private readonly Uri _baseChuckNorrisUri = new("https://api.chucknorris.io");
        private readonly Uri _namesUri = new("https://www.names.privserv.com/api/");

        private readonly IHttpClientFactory _httpClientFactory;

        public JsonFeed(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string[]> GetRandomJokesAsync(string firstname, string lastname, string category)
		{
			HttpClient client = _httpClientFactory.CreateClient();
			client.BaseAddress = _baseChuckNorrisUri;
			string url = "jokes/random";
			if (category != null)
			{
				if (url.Contains('?'))
					url += "&";
				else url += "?";
				url += "category=";
				url += category;
			}

            string joke = await client.GetStringAsync(url);

            if (firstname != null && lastname != null)
            {
                joke = joke.Replace("Chuck Norris", $"{firstname} {lastname}");
            }

            return new string[] { JsonConvert.DeserializeObject<dynamic>(joke).value };
        }

		public async Task<dynamic> GetNameAsync()
		{
			var client = _httpClientFactory.CreateClient();
            var result = await client.GetStringAsync(_namesUri);
			return JsonConvert.DeserializeObject<dynamic>(result);
        }

		public async Task<string[]> GetCategoriesAsync()
		{
            var client = _httpClientFactory.CreateClient();

            var uri = new UriBuilder(_baseChuckNorrisUri) {Path = "categories"}.Uri;

            var result = await client.GetFromJsonAsync<string[]>(uri);

			return result;
		}
    }
}
