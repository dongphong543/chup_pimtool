using AutoMapper;
using PIMBackend.Domain.Entities;
using PIMBackend.DTOs;

namespace PIMBackend.MappingProfiles
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile() : base(nameof(EmployeeMappingProfile))
        {
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
        }
    }
}