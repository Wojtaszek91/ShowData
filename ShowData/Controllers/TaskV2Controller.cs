using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShowData.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Controllers
{
    [Authorize]
    [Microsoft.AspNetCore.Components.Route("api/v{version:apiVersion}/task")]
    [ApiVersion("2.0")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class TaskV2Controller : ControllerBase
    {
        private readonly ItaskRepository _taskRepo;
        private readonly IProjectRepository _projectRepo;

        public TaskV2Controller(ItaskRepository taskRepo, IProjectRepository projectRepo)
        {
            _taskRepo = taskRepo;
            _projectRepo = projectRepo;
        }

        [HttpGet("api/v{version:apiVersion}/task/{taskId:int}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult ReportFinishTask(int taskId)
        {
            if (taskId != 0)
            {
                try
                {
                    bool response = false;
                    var task = _taskRepo.Gettask(taskId);
                    if(task != null)
                    {
                        task.isAvailsable = false;

                        var project = _projectRepo.GetProject(task.ProjectId);
                        if(project != null)
                        {
                            project.TasksIncluded--;
                            _projectRepo.UpdateProject(project);
                            response = _taskRepo.Updatetask(task);
                        }

                    }
                    if(response)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return NotFound(false);
                    }
                }

                catch (Exception)
                {
                    return BadRequest(false);
                }
            }
            else
            {
                return BadRequest(false);
            }
        }
    }
}
