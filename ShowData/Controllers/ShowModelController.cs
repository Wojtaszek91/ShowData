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
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ShowModelController : Controller
    {
        private readonly IShowModelRepository _showModelRepo;
        private readonly IMapper _map;
        public ShowModelController(IShowModelRepository showModelRepo, IMapper map)
        {
            _showModelRepo = showModelRepo;
            _map = map;
        }

        /// <summary>
        /// Get list of ShowData model
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ShowModelDto>))]
        public IActionResult GetShowModels()
        {
            var modelsList = _showModelRepo.GetShowModelList();

            var dtoModelsList = new List<ShowModelDto>();

            foreach (var model in modelsList)
            {
                dtoModelsList.Add(_map.Map<ShowModelDto>(model));
            }

            return Ok(dtoModelsList);
        }

        /// <summary>
        /// Get individual ShowModel
        /// </summary>
        /// <param name="showModelId">Id of specific ShowModel</param>
        /// <returns></returns>
        [HttpGet("{showModelId:int}", Name = "GetShowModel")]
        [ProducesResponseType(200, Type = typeof(ShowModelDto))]
        [ProducesResponseType(404)]
        public IActionResult GetShowModel(int showModelId)
        {
            var showModelInDb = _showModelRepo.GetShowModel(showModelId);

            if (showModelInDb != null)
            {
                ShowModelDto showModelDto = _map.Map<ShowModelDto>(showModelInDb);

                return Ok(showModelDto);
            }

            else return NotFound();
        }

        /// <summary>
        /// Creates a ShowModel
        /// </summary>
        /// <param name="showModelDto">Params requires to create ShowModel</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateShowModel([FromBody] ShowModelDto showModelDto)
        {
            if(showModelDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_showModelRepo.IsShowModelExists(showModelDto.DisplayName))
            {
                ModelState.AddModelError("", "Show model already exists !");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var showModelForDb = _map.Map<ShowModel>(showModelDto);

            if (!_showModelRepo.CreateShowModel(showModelForDb))
            {
                ModelState.AddModelError("", $"Ops, coudn't save object {showModelDto.DisplayName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetShowModel", new { showModelId = showModelForDb.ShowModelId}, showModelForDb);
        }

        /// <summary>
        /// Updates ShowModel in database
        /// </summary>
        /// <param name="showModelId">Id of specific ShowModel to update</param>
        /// <param name="showModelDto">Required params to build ShowModel</param>
        /// <returns></returns>
        [HttpPatch("{showModelId:int}", Name = "UpdateShowModel")]
        public IActionResult UpdateShowModel(int showModelId, [FromBody] ShowModelDto showModelDto)
        {
            if(showModelDto == null || showModelId != showModelDto.Id)
            {
                return BadRequest(ModelState);
            }

            var showModelObj = _map.Map<ShowModel>(showModelDto);

            if (!_showModelRepo.UpdateShowModel(showModelObj))
            {
                ModelState.AddModelError("", $"Ops, coudn't update object {showModelDto.DisplayName}");
                return StatusCode(500, ModelState);
            }
            else
                return NoContent();
        }

        /// <summary>
        /// Deletes ShowModel from database
        /// </summary>
        /// <param name="showModelId">Id of ShowModel to delete</param>
        /// <param name="showModelDto">Required params of ShowModel what should be deleted</param>
        /// <returns></returns>
        [HttpDelete("{showModelId:int}", Name = "DeleteShowModel")]
        public IActionResult DeleteShowModel(int showModelId, [FromBody] ShowModelDto showModelDto)
        {
            if (showModelDto == null || showModelId != showModelDto.Id)
            {
                return BadRequest(ModelState);
            }

            var showModelObj = _showModelRepo.GetShowModel(showModelId);

            if (!_showModelRepo.DeleteShowModel(showModelObj))
            {
                ModelState.AddModelError("", $"Ops, coudn't delete object {showModelDto.DisplayName}");
                return StatusCode(500, ModelState);
            }
            else
                return NoContent();
        }
    }
}
