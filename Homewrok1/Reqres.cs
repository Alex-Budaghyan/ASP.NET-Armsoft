using Homewrok1.Models;
using Homewrok1.Options;
using Microsoft.Extensions.Options;

namespace Homework1.Data
{
    public class ReqResClient
    {
        private readonly HttpClient _http;

        public ReqResClient(HttpClient http, IOptions<ReqResOptions> options)
        {
            _http = http;
            _http.BaseAddress = new Uri(options.Value.BaseUrl);
        }

        public async Task<User?> GetUser(int id)
        {
            var response = await _http.GetFromJsonAsync<SingleUserResponse>($"api/users/{id}");
            return response?.Data;
        }

        public async Task<User?> CreateUser(User user)
        {
            var response = await _http.PostAsJsonAsync("api/User", user);
            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<User?> UpdateUser(int id, User user)
        {
            var response = await _http.PutAsJsonAsync($"api/users/{id}", user);
            return await response.Content.ReadFromJsonAsync<User>();

        }

        public async Task<bool> DeleteUser(int id)
        {
            var response = await _http.DeleteAsync($"api/users/{id}");
            return response.IsSuccessStatusCode;
        }
    }

    public class SingleUserResponse
    {
        public User? Data { get; set; }
    }
}

