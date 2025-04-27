using LeaveManagement.Models;
using LeaveManagement.ViewModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace LeaveManagement.Web.Helper
{
    public class JWTTokenHelper
    {
        public static async Task<string> GetToken(HttpClient _httpClient, string username, string password)
        {
            var loginData = new
            {
                Username = "admin",
                Password = "123"
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            var loginResponse = await _httpClient.PostAsync("Auth/login", content);
            var loginResponseString = await loginResponse.Content.ReadAsStringAsync();

            // Deserialize
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(loginResponseString);
            string jwtToken = loginResult.Token;
            return jwtToken;
        }
    }
}
