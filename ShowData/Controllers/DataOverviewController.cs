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
    public class DataOverviewController : Controller
    {
        private readonly IDataOverviewRepository _dataOverviewRepo;
        private readonly IMapper _map;
        public DataOverviewController(IDataOverviewRepository dataOverviewRepo, IMapper map)
        {
            _dataOverviewRepo = dataOverviewRepo;
            _map = map;
        }

        /// <summary>
        /// Get list of ShowData model
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<DataOverviewDto>))]
        public IActionResult GetDataOverviews()
        {
            var modelsList = _dataOverviewRepo.GetDataOverviewList();

            var dtoModelsList = new List<DataOverviewDto>();

            foreach (var model in modelsList)
            {
                dtoModelsList.Add(_map.Map<DataOverviewDto>(model));
            }

            return Ok(dtoModelsList);
        }

        /// <summary>
        /// Get individual DataOverview
        /// </summary>
        /// <param name="dataOverviewId">Id of specific DataOverview</param>
        /// <returns></returns>
        [HttpGet("{dataOverviewId:int}", Name = "GetDataOverview")]
        [ProducesResponseType(200, Type = typeof(DataOverviewDto))]
        [ProducesResponseType(404)]
        public IActionResult GetDataOverview(int dataOverviewId)
        {
            var dataOverviewInDb = _dataOverviewRepo.GetDataOverview(dataOverviewId);

            if (dataOverviewInDb != null)
            {
                DataOverviewDto dataOverviewDto = _map.Map<DataOverviewDto>(dataOverviewInDb);

                return Ok(dataOverviewDto);
            }

            else return NotFound();
        }

        /// <summary>
        /// Creates a DataOverview
        /// </summary>
        /// <param name="dataOverviewDto">Params requires to create DataOverview</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateDataOverview([FromBody] DataOverviewDto dataOverviewDto)
        {
            if (dataOverviewDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_dataOverviewRepo.IsDataOverviewExists(dataOverviewDto.Title))
            {
                ModelState.AddModelError("", "Show model already exists !");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dataOverviewForDb = _map.Map<DataOverview>(dataOverviewDto);

            if (!_dataOverviewRepo.CreateDataOverview(dataOverviewForDb))
            {
                ModelState.AddModelError("", $"Ops, coudn't save object {dataOverviewDto.Title}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetDataOverview", new { dataOverviewId = dataOverviewForDb.DataOverviewId }, dataOverviewForDb);
        }

        /// <summary>
        /// Updates DataOverview in database
        /// </summary>
        /// <param name="dataOverviewId">Id of specific DataOverview to update</param>
        /// <param name="dataOverviewDto">Required params to build DataOverview</param>
        /// <returns></returns>
        [HttpPatch("{dataOverviewId:int}", Name = "UpdateDataOverview")]
        public IActionResult UpdateDataOverview(int dataOverviewId, [FromBody] DataOverviewDto dataOverviewDto)
        {
            if (dataOverviewDto == null || dataOverviewId != dataOverviewDto.Id)
            {
                return BadRequest(ModelState);
            }

            var dataOverviewObj = _map.Map<DataOverview>(dataOverviewDto);

            if (!_dataOverviewRepo.UpdateDataOverview(dataOverviewObj))
            {
                ModelState.AddModelError("", $"Ops, coudn't update object {dataOverviewDto.Title}");
                return StatusCode(500, ModelState);
            }
            else
                return NoContent();
        }

        /// <summary>
        /// Deletes DataOverview from database
        /// </summary>
        /// <param name="dataOverviewId">Id of DataOverview to delete</param>
        /// <param name="dataOverviewDto">Required params of DataOverview what should be deleted</param>
        /// <returns></returns>
        [HttpDelete("{dataOverviewId:int}", Name = "DeleteDataOverview")]
        public IActionResult DeleteDataOverview(int dataOverviewId, [FromBody] DataOverviewDto dataOverviewDto)
        {
            if (dataOverviewDto == null || dataOverviewId != dataOverviewDto.Id)
            {
                return BadRequest(ModelState);
            }

            var dataOverviewObj = _dataOverviewRepo.GetDataOverview(dataOverviewId);

            if (!_dataOverviewRepo.DeleteDataOverview(dataOverviewObj))
            {
                ModelState.AddModelError("", $"Ops, coudn't delete object {dataOverviewDto.Title}");
                return StatusCode(500, ModelState);
            }
            else
                return NoContent();
        }
    }
}
