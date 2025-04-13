using System.Text.Json;
using AsyncDataFetcher.Models;

namespace AsyncDataFetcher.Services;

public class JsonPlaceholderClient
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public JsonPlaceholderClient(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<UserWithPosts> FetchUserDataAsync(int userId)
    {
        var httpClient = _clientFactory.CreateClient();

        var userTask = SendRequestAsync<User>($"https://jsonplaceholder.typicode.com/users/{userId}", httpClient);
        var postsTask = SendRequestAsync<List<Post>>($"https://jsonplaceholder.typicode.com/posts?userId={userId}", httpClient);

        await Task.WhenAll(userTask, postsTask);

        return new UserWithPosts
        {
            User = userTask.Result,
            Posts = postsTask.Result ?? new List<Post>()
        };
    }

    private async Task<T?> SendRequestAsync<T>(string url, HttpClient client)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, url);
        var res = await client.SendAsync(req);

        if (!res.IsSuccessStatusCode)
            return default;

        var content = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }
}