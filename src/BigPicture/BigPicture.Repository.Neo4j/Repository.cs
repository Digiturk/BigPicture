using BigPicture.Core.Config;
using BigPicture.Core.Repository;
using Neo4j.Driver.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BigPicture.Repository.Neo4j
{
    public class Repository : IRepository
    {
        public string TestConnection()
        {
            try
            {
                using (var driver = GraphDatabase.Driver(CommonConfig.Instance.Repository))
                {
                    using (var session = driver.Session())
                    {
                        var result = session.Run("return 'Neo4j connection is tested successfully'");
                        var record = result.Peek();
                        return record.Values[record.Keys[0]].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CreateNode(String nodeType, object data)
        {
            using (var driver = GraphDatabase.Driver(CommonConfig.Instance.Repository))
            {
                using (var session = driver.Session())
                {
                    return session.WriteTransaction(tx =>
                    {
                        var jsonData = ToJson(data);
                        var statement = $"CREATE (a:{nodeType} {jsonData}) RETURN id(a)";

                        var result = tx.Run(statement);
                        return result.Peek()[0].ToString();
                    });
                }
            }
        }

        private string ToJson(object data)
        {
            var serializer = new JsonSerializer();
            var stringWriter = new StringWriter();
            using (var writer = new JsonTextWriter(stringWriter))
            {
                writer.QuoteName = false;
                serializer.Serialize(writer, data);
            }
            return stringWriter.ToString();
        }
    }
}
