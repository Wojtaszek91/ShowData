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
    //[ApiExplorerSettings(GroupName = "DataOverviewApiSpec")]
    public class DataOverviewV2Controller : Controller
    {
        private readonly IDataOverviewRepository _dataOverviewRepo;
        private readonly IMapper _map;
        public DataOverviewV2Controller(IDataOverviewRepository dataOverviewRepo, IMapper map)
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
    }
}
