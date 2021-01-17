using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShowData.Model;
using ShowData.Model.DTO;
using ShowData.Repository.IRepository;

namespace ShowData.Controllers
{
  //  [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/Projects")]
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IMapper _map;
        public ProjectController(IProjectRepository projectRepo, IMapper map)
        {
            _projectRepo = projectRepo;
            _map = map;
        }

        /// <summary>
        /// Get list of Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ProjectDto>))]
        public IActionResult GetProjcets()
        {
            var modelsList = _projectRepo.GetProjectsList();

            var dtoModelsList = new List<ProjectDto>();

            foreach (var model in modelsList)
            {
                dtoModelsList.Add(_map.Map<ProjectDto>(model));
            }

            return Ok(dtoModelsList);
        }

        /// <summary>
        /// Get individual Project
        /// </summary>
        /// <param name="projectId">Id of specific Project</param>
        /// <returns></returns>
        [HttpGet("{projectId:int}", Name = "GetProject")]
        [ProducesResponseType(200, Type = typeof(ProjectDto))]
        [ProducesResponseType(404)]
        public IActionResult GetProject(int projectId)
        {
            var projectInDb = _projectRepo.GetProject(projectId);

            if (projectInDb != null)
            {
                ProjectDto projectDto = _map.Map<ProjectDto>(projectInDb);

                return Ok(projectDto);
            }

            else return NotFound();
        }

        /// <summary>
        /// Creates a Project
        /// </summary>
        /// <param name="projectDto">Params requires to create Project</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateProject([FromBody] ProjectCreateDto projectDto)
        {
            if (projectDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_projectRepo.IsProjectExists(projectDto.Title))
            {
                ModelState.AddModelError("", "Show model already exists !");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectForDb = _map.Map<Project>(projectDto);

            if (!_projectRepo.CreateProject(projectForDb))
            {
                ModelState.AddModelError("", $"Ops, coudn't save object {projectDto.Title}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetProject", new { Version = HttpContext.GetRequestedApiVersion().ToString(), ProjectId = projectForDb.Id }, projectForDb);
        }

        /// <summary>
        /// Updates Project in database.
        /// </summary>
        /// <param name="projectId">Id of specific Project to update</param>
        /// <param name="projectDto">Required params to build Project</param>
        /// <returns></returns>
        [HttpPatch("{projectId:int}", Name = "UpdateProject")]
        public IActionResult UpdateProject(int projectId, [FromBody] ProjectUpdateDto projectDto)
        {
            if (projectDto == null || projectId != projectDto.Id)
            {
                return BadRequest(ModelState);
            }

            var projectObj = _map.Map<Project>(projectDto);

            if (!_projectRepo.UpdateProject(projectObj))
            {
                ModelState.AddModelError("", $"Ops, coudn't update object {projectDto.Title}");
                return StatusCode(500, ModelState);
            }
            else
                return NoContent();
        }

        /// <summary>
        /// Deletes Project from database
        /// </summary>
        /// <param name="projectId">Id of Project to delete</param>
        /// <returns></returns>
        [HttpDelete("{projectId:int}", Name = "DeleteProject")]
        public IActionResult DeleteProject(int projectId)
        {
            if (!_projectRepo.IsProjectExists(projectId))
            {
                return NotFound();
            }

            var projectFromDb = _projectRepo.GetProject(projectId);
            if (!_projectRepo.DeleteProject(projectFromDb))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {projectFromDb.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
