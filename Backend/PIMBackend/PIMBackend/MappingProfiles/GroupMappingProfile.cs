using AutoMapper;
using PIMBackend.Domain.Entities;
using PIMBackend.DTOs;

namespace PIMBackend.MappingProfiles
{
    public class GroupMappingProfile : Profile
    {
        public GroupMappingProfile() : base(nameof(GroupMappingProfile))
        {
            CreateMap<Group, GroupDTO>().ReverseMap();
        }
    }
}