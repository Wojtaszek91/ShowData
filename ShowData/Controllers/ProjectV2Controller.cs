using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShowData.Model;
using ShowData.Model.DTO;
using ShowData.Repository.IRepository;

namespace ShowData.Controllers
{
    [Route("api/v{version:apiVersion}/Project")]
    [ApiVersion("2.0")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ProjectV2Controller : ControllerBase
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IMapper _map;
        public ProjectV2Controller(IProjectRepository projectRepo, IMapper map)
        {
            _projectRepo = projectRepo;
            _map = map;
        }

        /// <summary>
        /// Get list of ShowData model
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ProjectDto))]
        public IActionResult GetProjects()
        {
            var modelsList = _projectRepo.GetProjectsList().FirstOrDefault();

            return Ok(_map.Map<ProjectDto>(modelsList));
        }


    }
}
