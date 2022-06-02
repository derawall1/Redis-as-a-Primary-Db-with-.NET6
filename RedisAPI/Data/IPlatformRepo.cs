using RedisAPI.Models;

namespace RedisAPI.Data
{
    public interface IPlatformRepo
    {
        void CreatePlatform(Platform platform);
        Platform? GetPlatformById(string Id);
        IEnumerable<Platform> GetAllPlatforms();
    }
}
