using Homewrok1.Models;

namespace Homework1.Data
{
    public class ReqResClient
    {
        private readonly HttpClient _http;

        public ReqResClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<User?> GetUser(int id)
        {
            var response = await _http.GetFromJsonAsync<SingleUserResponse>($"api/users/{id}");
            return response?.Data;
        }

        public async Task<User?> CreateUser(User user)
        {
            var response = await _http.PostAsJsonAsync("api/users", user);
            var created = await response.Content.ReadFromJsonAsync<SingleUserResponse>();
            return created?.Data;
        }

        public async Task<User?> UpdateUser(int id, User user)
        {
            var response = await _http.PutAsJsonAsync($"api/users/{id}", user);
            var updated = await response.Content.ReadFromJsonAsync<SingleUserResponse>();
            return updated?.Data;
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

