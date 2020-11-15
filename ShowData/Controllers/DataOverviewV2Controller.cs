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
    [Route("api/v{version:apiVersion}/DataOverview")]
    [ApiVersion("2.0")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ApiExplorerSettings(GroupName = "DataOverviewApiSpec")]
    public class DataOverviewV2Controller : ControllerBase
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
        [ProducesResponseType(200, Type = typeof(DataOverviewDto))]
        public IActionResult GetDataOverviews()
        {
            var modelsList = _dataOverviewRepo.GetDataOverviewList().FirstOrDefault();

            return Ok(_map.Map<DataOverviewDto>(modelsList));
        }


    }
}
