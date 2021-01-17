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
            CreateMap<task, taskDto>().ReverseMap();
            CreateMap<task, taskUpdateDto>().ReverseMap();
            CreateMap<task, taskUpdateDto>().ReverseMap();

            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Project, ProjectUpdateDto>().ReverseMap();
            CreateMap<Project, ProjectCreateDto>().ReverseMap();
        }
    }
}
