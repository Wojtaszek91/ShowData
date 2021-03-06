﻿using Newtonsoft.Json;
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
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpFactory;

        public Repository(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }
        public async Task<bool> CreateAsync(string url, T objToCreate, string token ="")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
                return false;

            var client = _httpFactory.CreateClient();

            if (token == null)
            {
                token = "";
            }

            if (token.Length > 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage respone = await client.SendAsync(request);

            if (respone.StatusCode == HttpStatusCode.Created)
                return true;
            else
                return false;
        }

        public async Task<bool> DeleteAsync(string url, int idOfObjToDelete, string token="")
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url+idOfObjToDelete);
            var client = _httpFactory.CreateClient();

            if (token == null)
            {
                token = "";
            }

            if (token.Length > 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage respone = await client.SendAsync(request);

            if (respone.StatusCode == HttpStatusCode.NoContent)
                return true;
            else
                return false;
        }

        
        public async Task<IEnumerable<T>> GetAllAsync(string url, string token="")
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
                return JsonConvert.DeserializeObject<IEnumerable<T>>(responeString);
            }
            else
                return null;
        }

        public async Task<T> GetAsync(string url, int id, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);
            var client = _httpFactory.CreateClient();

            if (token == null)
            {
                token = "";
            }

            if (token.Length > 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage respone = await client.SendAsync(request);

            if (respone.StatusCode == HttpStatusCode.OK)
            {
                var responeString = await respone.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responeString);
            }
            else
                return null;
        }

        public async Task<bool> UpdateAsync(string url, T ObjToUpdate, string token="")
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if (ObjToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(ObjToUpdate), Encoding.UTF8, "application/json");
            }
            else
                return false;

            var client = _httpFactory.CreateClient();

            if (token == null)
            {
                token = "";
            }

            if (token.Length > 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage respone = await client.SendAsync(request);

            if (respone.StatusCode == HttpStatusCode.NoContent)
                return true;
            else
                return false;

        }
    }
}
