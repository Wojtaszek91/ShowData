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
    [Authorize]
    [Route("api/v{version:apiVersion}/task")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    // [ApiExplorerSettings(GroupName = "ShowDataApiSpec")]
    public class taskController : ControllerBase
    {
        private readonly ItaskRepository _taskRepo;
        private readonly IMapper _map;
        public taskController(ItaskRepository taskRepo, IMapper map)
        {
            _taskRepo = taskRepo;
            _map = map;
        }

        [HttpGet("{projectId:int}/{isProject:bool}", Name = "Gettasks")]
        [ProducesResponseType(200, Type = typeof(List<taskDto>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult Gettasks(int projectId, bool isProject)
        {
            if (projectId != 0)
            {
                try
                {
                    int id = projectId;
                    IEnumerable<task> modelList;
                    if (id != 0)
                    {
                        modelList = _taskRepo.GettaskList().Where(t => t.ProjectId == id);
                    }
                    else
                    {
                        modelList = _taskRepo.GettaskList();
                    }

                    if (modelList == null)
                    {
                        return NotFound();
                    }

                    var dtoModelList = new List<taskDto>();

                    foreach (var model in modelList)
                    {
                        dtoModelList.Add(_map.Map<taskDto>(model));
                    }

                    return Ok(dtoModelList);
                }

                catch (Exception)
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get individual task
        /// </summary>
        /// <param name="taskId">Id of specific task</param>
        /// <returns></returns>
        [HttpGet("{taskId:int}", Name = "Gettask")]
        [ProducesResponseType(200, Type = typeof(taskDto))]
        [ProducesResponseType(404)]
        public IActionResult Gettask(int taskId)
        {
            var taskInDb = _taskRepo.Gettask(taskId);

            if (taskInDb != null)
            {
                taskDto taskDto = _map.Map<taskDto>(taskInDb);

                return Ok(taskDto);
            }

            else return NotFound();
        }

        /// <summary>
        /// Creates a task
        /// </summary>
        /// <param name="taskDto">Params requires to create task</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Createtask([FromBody] taskUpdateDto taskDto)
        {
            if (taskDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_taskRepo.IstaskExists(taskDto.DisplayName))
            {
                ModelState.AddModelError("", "Show model already exists !");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taskForDb = _map.Map<task>(taskDto);

            if (!_taskRepo.Createtask(taskForDb))
            {
                ModelState.AddModelError("", $"Ops, coudn't save object {taskDto.DisplayName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("Gettask", new { Version = HttpContext.GetRequestedApiVersion().ToString(), taskId = taskForDb.Id }, taskForDb);
        }

        /// <summary>
        /// Updates task in database
        /// </summary>
        /// <param name="taskId">Id of specific task to update</param>
        /// <param name="taskDto">Required params to build task</param>
        /// <returns></returns>
        [HttpPatch("{taskId:int}", Name = "Updatetask")]
        public IActionResult Updatetask(int taskId, [FromBody] taskUpdateDto taskDto)
        {
            if (taskDto == null || taskId != taskDto.Id)
            {
                return BadRequest(ModelState);
            }

            var taskObj = _map.Map<task>(taskDto);

            if (!_taskRepo.Updatetask(taskObj))
            {
                ModelState.AddModelError("", $"Ops, coudn't update object {taskDto.DisplayName}");
                return StatusCode(500, ModelState);
            }
            else
                return NoContent();
        }

        ///// <summary>
        ///// Deletes task from database
        ///// </summary>
        ///// <param name="taskId">Id of task to delete</param>
        ///// <param name="taskDto">Required params of task what should be deleted</param>
        ///// <returns></returns>
        //[HttpDelete("{taskId:int}", Name = "Deletetask")]
        //public IActionResult Deletetask(int taskId, [FromBody] taskDto taskDto)
        //{
        //    if (taskDto == null || taskId != taskDto.Id)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var taskObj = _taskRepo.Gettask(taskId);

        //    if (!_taskRepo.Deletetask(taskObj))
        //    {
        //        ModelState.AddModelError("", $"Ops, coudn't delete object {taskDto.DisplayName}");
        //        return StatusCode(500, ModelState);
        //    }
        //    else
        //        return NoContent();
        //}

        /// <summary>
        /// Deletes DataOverview from database
        /// </summary>
        /// <param name="taskId">Id of DataOverview to delete</param>
        /// <returns></returns>
        [HttpDelete("{taskId:int}", Name = "Deletetask")]
        public IActionResult Deletetask(int taskId)
        {
            if (!_taskRepo.IstaskExists(taskId))
            {
                return NotFound();
            }

            var taskFromDb = _taskRepo.Gettask(taskId);
            if (!_taskRepo.Deletetask(taskFromDb))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the record {taskFromDb.DisplayName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
