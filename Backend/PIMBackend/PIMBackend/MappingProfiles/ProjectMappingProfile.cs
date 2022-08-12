using AutoMapper;
using PIMBackend.Domain.Entities;
using PIMBackend.DTOs;

namespace PIMBackend.MappingProfiles
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile() : base(nameof(ProjectMappingProfile))
        {
            CreateMap<Project, ProjectDTO>().ReverseMap();
        }
    }
}