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
    public class ShowModelController : Controller
    {
        private IShowModelRepository _showModelRepo;
        private readonly IMapper _map;
        public ShowModelController(IShowModelRepository showModelRepo, IMapper map)
        {
            _showModelRepo = showModelRepo;
            _map = map;
        }

        [HttpGet]
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

        [HttpGet("{showModelId:int}", Name = "GetShowModel")]
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

            return CreatedAtRoute("GetShowModel", new { showModelId = showModelForDb.Id}, showModelForDb);
        }

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
