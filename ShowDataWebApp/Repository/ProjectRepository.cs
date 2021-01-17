using ShowDataWebApp.Models;
using ShowDataWebApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShowDataWebApp.Repository
{

    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly IHttpClientFactory _httpFactory;

        public ProjectRepository(IHttpClientFactory httpFactory) : base(httpFactory)
        {
            _httpFactory = httpFactory;
        }
    }
}
