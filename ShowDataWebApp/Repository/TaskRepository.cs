using Newtonsoft.Json;
using ShowDataWebApp.Models;
using ShowDataWebApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ShowDataWebApp.Repository
{
    public class taskRepository : Repository<task>, ItaskRepository
    {
        private readonly IHttpClientFactory _httpFactory;

        public taskRepository(IHttpClientFactory httpFactory) : base(httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<IEnumerable<task>> GetAllProjectTasks(string url, string projectId, bool isProject, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + projectId + "/" + isProject);
            var client = _httpFactory.CreateClient();

            if(token == null)
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
                return JsonConvert.DeserializeObject<IEnumerable<task>>(responeString);
            }
            else
                return null;
        }

        public async Task<bool> ReportTaskFinish(string url, int taskId, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + taskId.ToString());
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
                return JsonConvert.DeserializeObject<bool>(responeString);
            }
            else
                return false;
        }
    }
}

