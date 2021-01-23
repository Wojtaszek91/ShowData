using Newtonsoft.Json;
using ShowDataWebApp.Models;
using ShowDataWebApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ShowDataWebApp.Repository
{
    public class UserAccountRepository : Repository<User>, IUserAccountRepository
    {
        private readonly IHttpClientFactory _httpFactory;

        public UserAccountRepository(IHttpClientFactory httpFactory) : base(httpFactory)
        {
            _httpFactory = httpFactory;
        }

        
        public async Task<IEnumerable<string>> GetUsernames(string url, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _httpFactory.CreateClient();

            if (token == null)
            {
                token = "";
            }

            if (token.Length > 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage respone = await client.SendAsync(request);

            if (respone.StatusCode == HttpStatusCode.OK)
            {
                var responeString = await respone.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<string>>(responeString);
            }
            else
                return null;
        }

        public async Task<User> LoginAsync(string url, User user)
        {
            if (user != null)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                var client = _httpFactory.CreateClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<User>(jsonContent);
                }
                else
                    return null;
            }
            else
            {
                return null;
            }


        }

        public async Task<bool> RegisterAsync(string url, User user)
        {
            if (user != null)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                var client = _httpFactory.CreateClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
    }
}
