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
    internal class RedisTableResolver : IResolver<RedisTable>
    {
        private IRepository _Repository { get; set; }

        public RedisTableResolver(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Resolve(RedisTable table)
        {
            var connectionString = CommonConfig.Instance.Options[table.DatabaseName];
            RedisEndpoint conf = new RedisEndpoint { Host = connectionString, Port = 6379, Db = table.db };

            RedisClient redisClient = new RedisClient(conf);

            var keys = redisClient.GetAllKeys();

            if (keys.Count > 0)
            {
                List<string> hashKeys;
                List<string> hashValues;

                foreach (var key in keys)
                {
                    if (!key.StartsWith(table.Name))
                        continue;

                    try
                    {
                        hashKeys = redisClient.GetHashKeys(key);

                        foreach (var hashKey in hashKeys)
                        {
                            string value = redisClient.GetValueFromHash(hashKey, key);

                            var hash = new Hash();
                            hash.Key = hashKey;
                            hash.Value = value;
                            hash.Table = table.Id;

                            hash.Id = this._Repository.CreateNode(hash, "Hash");
                            this._Repository.CreateRelationship(table.Id, hash.Id, "CONTAINS");

                            //Hash Key'ler yazılır

                        }

                        hashValues = redisClient.GetHashValues(key);

                        foreach (var hashValue in hashValues)
                        {
                            var hash = new Hash();
                            hash.Key = key;
                            hash.Value = hashValue;
                            hash.Table = table.Id;

                            hash.Id = this._Repository.CreateNode(hash, "Hash");
                            this._Repository.CreateRelationship(table.Id, hash.Id, "CONTAINS");
                        }

                    }
                    catch (Exception)
                    {
                        try
                        {
                            string value = redisClient.GetValue(key);
                            var hash = new Hash();
                            hash.Key = key;
                            hash.Value = value;
                            hash.Table = table.Id;

                            hash.Id = this._Repository.CreateNode(hash, "Hash");
                            this._Repository.CreateRelationship(table.Id, hash.Id, "CONTAINS");
                        }
                        catch (Exception)
                        {

                            continue;
                        }

                        continue;
                    }
                }
            }
        }
    }
}
