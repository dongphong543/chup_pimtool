using AutoMapper;
using PIMBackend.MappingProfiles;

namespace PIMBackend.Configurations
{
    public static class MapperFactory
    {
        private static readonly object s_lock = new object();
        private static IMapper _mapper;

        public static IMapper GetMapper()
        {
            if (_mapper == null)
            {
                lock (s_lock)
                {
                    if (_mapper == null)
                    {
                        var config =
                            new MapperConfiguration(
                                x => {
                                    x.AddProfiles(new[]
                                    {
                                        new ProjectMappingProfile()
                                    });
                                    x.AddProfiles(new[]
                                    {
                                        new EmployeeMappingProfile()
                                    });
                                    x.AddProfiles(new[]
                                    {
                                        new GroupMappingProfile()
                                    });
                                });
                        _mapper = config.CreateMapper();
                    }
                }
            }

            return _mapper;
        }
    }
}