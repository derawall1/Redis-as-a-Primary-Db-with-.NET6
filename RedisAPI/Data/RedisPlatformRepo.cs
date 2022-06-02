﻿using RedisAPI.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisAPI.Data
{
    public class RedisPlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisPlatformRepo(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentOutOfRangeException(nameof(platform));
            }

            var db = _redis.GetDatabase();

            var serialPlat = JsonSerializer.Serialize(platform);

            //db.StringSet(plat.Id, serialPlat);
            db.HashSet($"hashplatform", new HashEntry[]
                {new HashEntry(platform.Id, serialPlat)});
        }

        public Platform? GetPlatformById(string id)
        {
            var db = _redis.GetDatabase();

            //var plat = db.StringGet(id);

            var plat = db.HashGet("hashplatform", id);

            if (!string.IsNullOrEmpty(plat))
            {
                return JsonSerializer.Deserialize<Platform>(plat);
            }
            return null;
        }

        public IEnumerable<Platform?>? GetAllPlatforms()
        {
            var db = _redis.GetDatabase();

            var completeSet = db.HashGetAll("hashplatform");

            if (completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val =>
                    JsonSerializer.Deserialize<Platform>(val.Value)).ToList();
                return obj;
            }

            return null;
        }
    }
}
