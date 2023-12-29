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
    public class RedisDatabaseResolver : IResolver<RedisDatabase>
    {
        private IRepository _Repository { get; set; }

        public RedisDatabaseResolver(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Resolve(RedisDatabase database)
        {
            var connectionString = CommonConfig.Instance.Options[database.DatabaseName];
            RedisEndpoint conf = new RedisEndpoint { Host = connectionString, Port = 6379, Db = database.db };

            RedisClient redisClient = new RedisClient(conf);

            var keys = redisClient.GetAllKeys();

            List<string> nodes = new List<string>();

            foreach (var item in keys)
            {
                if (!item.Contains(":"))
                    continue;

                nodes.Add(item.Split(':')[0]);
            }

            foreach (var item in nodes.Distinct().ToList())
            {
                var table = new RedisTable();
                table.db = database.db;
                table.Name = item;
                table.DatabaseName = database.DatabaseName;
                table.Id = this._Repository.CreateNode(table, "RedisTable");
                this._Repository.CreateRelationship(database.Id, table.Id, "CONTAINS");
            }


        }
    }
}
