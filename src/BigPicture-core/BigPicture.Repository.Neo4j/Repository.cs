using BigPicture.Core;
using BigPicture.Core.Config;
using BigPicture.Core.Repository;
using Neo4j.Driver.V1;
using NeoINode = Neo4j.Driver.V1.INode;
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

        public string CreateRelationship(String from, String to, String relationShip)
        {
            using (var driver = GraphDatabase.Driver(CommonConfig.Instance.Repository))
            {
                using (var session = driver.Session())
                {
                    return session.WriteTransaction(tx =>
                    {
                        var statement = $@"
                            MATCH (s), (n)
                            where ID(s) = {from} and ID(n) = {to}
                            CREATE (s)-[r:{relationShip}]->(n)
                            return r";

                        var result = tx.Run(statement);
                        return result.Peek()[0].ToString();
                    });
                }
            }
        }

        public string CreateNode(String nodeType, object node)
        {
            using (var driver = GraphDatabase.Driver(CommonConfig.Instance.Repository))
            {
                using (var session = driver.Session())
                {
                    return session.WriteTransaction(tx =>
                    {
                        var jsonData = ToJson(node);
                        var statement = $"CREATE (a:{nodeType} {jsonData}) RETURN id(a)";

                        var result = tx.Run(statement);
                        return result.Peek()[0].ToString();                        
                    });
                }
            }
        }

        public List<BigPicture.Core.INode> GetAllNodes(String nodeType, Type type)
        {
            using (var driver = GraphDatabase.Driver(CommonConfig.Instance.Repository))
            {                
                using (var session = driver.Session())
                {                    
                    var result = session.Run($"MATCH (a:{nodeType}) RETURN a");


                    var resultList = new List<BigPicture.Core.INode>();
                    foreach (var record in result)
                    {
                        var nodeProps = JsonConvert.SerializeObject(record[0].As<NeoINode>().Properties);
                        var node = (BigPicture.Core.INode)JsonConvert.DeserializeObject(nodeProps, type);
                        node.Id = record[0].As<NeoINode>().Id.ToString();
                        resultList.Add(node);
                    }

                    return resultList;
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
