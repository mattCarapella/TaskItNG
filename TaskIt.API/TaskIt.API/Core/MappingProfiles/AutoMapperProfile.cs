using AutoMapper;
using TaskIt.API.DTO.Account;
using TaskIt.API.DTO.Project;
using TaskIt.API.Models;

namespace TaskIt.API.Core.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Project, ProjectDTO>().ReverseMap();
        CreateMap<Project, ProjectCreateDTO>().ReverseMap();
        CreateMap<Project, ProjectUpdateDTO>().ReverseMap();
    
        CreateMap<ApiUser, RegisterRequestDTO>().ReverseMap().ReverseMap();
    
        
    }
}
