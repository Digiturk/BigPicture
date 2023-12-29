using BigPicture.Core.Config;
using BigPicture.Core.Repository;
using BigPicture.Core.Resolver;
using BigPicture.Resolver.Redis.Nodes;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.Redis.Resolvers
{
    public class CacheServerResolver : IResolver<CacheServer>
    {
        private IRepository _Repository { get; set; }

        public CacheServerResolver(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Resolve(CacheServer cacheServer)
        {
            var connectionString = CommonConfig.Instance.Options[cacheServer.Name];

            for (int db = 0; db <= 15; db++)
            {
                RedisEndpoint conf = new RedisEndpoint { Host = connectionString, Port = 6379, Db = db };

                RedisClient redisClient = new RedisClient(conf);

                var keys = redisClient.GetAllKeys();

                if (keys.Count > 0)
                {
                    var database = new RedisDatabase();
                    database.db = db;
                    database.Name = "db" + db;
                    database.DatabaseName = cacheServer.Name;
                    database.Id = this._Repository.CreateNode(database, "RedisDatabase");

                    this._Repository.CreateRelationship(cacheServer.Id, database.Id, "CONTAINS");
                }
            }

        }
    }
}
