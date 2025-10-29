using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ToDo_List.PL.ViewModels;
using ToDO_List.DAL.Data.Models;

namespace ToDo_List.PL.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Tasks, TaskViewModel>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ReverseMap();

            CreateMap<Tasks , CreateEditTaskViewModel >().ReverseMap();

            CreateMap<Project, ProjectViewModel>();
               
            CreateMap<ProjectViewModel, Project>()
                .ForMember(dest => dest.Tasks, opt => opt.Ignore());


            CreateMap<IdentityUser , SignUpViewModel>().ReverseMap();


        }
    }
}
