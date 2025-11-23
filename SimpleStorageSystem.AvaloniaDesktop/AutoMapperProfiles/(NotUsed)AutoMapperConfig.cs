using AutoMapper;
using System.Reflection;

namespace SimpleStorageSystem.AvaloniaDesktop.AutoMapperProfiles;

public static class AutoMapperConfig
{
    public static IMapper Initialize()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(Assembly.GetExecutingAssembly()); // Scans current assembly for profiles
        });
        return config.CreateMapper();
    }
}