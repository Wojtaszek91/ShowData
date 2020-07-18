using AutoMapper;
using ShowData.Model;
using ShowData.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Mappings
{
    public class ShowMapper: Profile
    {
        public ShowMapper()
        {
            CreateMap<ShowModel, ShowModelDto>().ReverseMap();
            CreateMap<ShowModel, ShowModelUpdateDto>().ReverseMap();
            CreateMap<ShowModel, ShowModelUpdateDto>().ReverseMap();
            CreateMap<DataOverview, DataOverviewDto>().ReverseMap();
            CreateMap<DataOverview, DataOverviewUpdateDto>().ReverseMap();
            CreateMap<DataOverview, DataOverviewCreateDto>().ReverseMap();
        }
    }
}
