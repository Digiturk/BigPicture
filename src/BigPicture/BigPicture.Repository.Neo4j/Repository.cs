using BigPicture.Core;
using BigPicture.Core.Config;
using BigPicture.Core.Repository;
using Neo4j.Driver.V1;
using NeoINode = Neo4j.Driver.V1.INode;
using Newtonsoft.Json;
using System;
using System.Linq;
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
                var result = this.Read("return 'Neo4j connection is tested successfully'");
                var record = result.Peek();
                return record.Values[record.Keys[0]].ToString();                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void DeleteAll()
        {
            var statement = $@"MATCH(n) DETACH DELETE n";
            this.Write(statement);
        }

        public string CreateRelationship(String from, String to, String relationShip)
        {
            var statement = $@"
                MATCH (s), (n)
                where ID(s) = {from} and ID(n) = {to}
                CREATE (s)-[r:{relationShip}]->(n)
                return r";

            var result = this.Write(statement);
            return result.Peek()[0].ToString();
        }

        public string CreateNode(String nodeType, object node)
        {
            var jsonData = ToJson(node);
            var statement = $"CREATE (a:{nodeType} {jsonData}) RETURN id(a)";

            var result = this.Write(statement);
            return result.Peek()[0].ToString();
        }

        public void UpdateNode<T>(T node) where T : BigPicture.Core.INode
        {
            var jsonData = ToJson(node);
            var statement = $"MATCH (n) WHERE ID(n) = {node.Id} SET n = {jsonData} RETURN n";

            var result = this.Write(statement);
        }
        
        public List<BigPicture.Core.INode> GetAllNodes(String nodeType, Type type, dynamic filterObject = null)
        {
            var filter = "";
            if (filterObject != null)
            {
                filter = ToJson(filterObject);
            }

            var result = this.Read($"MATCH (a:{nodeType} {filter}) RETURN a");

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

        public List<T> GetAllNodes<T>(String nodeType, dynamic filterObject = null) where T : BigPicture.Core.INode
        {
            List<BigPicture.Core.INode> result = this.GetAllNodes(nodeType, typeof(T), filterObject);
            return result.Cast<T>().ToList();
        }

        private IStatementResult Read(String statement)
        {
            using (var driver = GraphDatabase.Driver(CommonConfig.Instance.Repository))
            {
                using (var session = driver.Session())
                {
                    var result = session.Run(statement);
                    return result;                    
                }
            }
        }

        private IStatementResult Write(String statement)
        {
            using (var driver = GraphDatabase.Driver(CommonConfig.Instance.Repository))
            {
                using (var session = driver.Session())
                {
                    return session.WriteTransaction(tx =>
                    {
                        var result = tx.Run(statement);
                        return result;
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
