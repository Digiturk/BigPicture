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
using System.Diagnostics;

namespace BigPicture.Repository.Neo4j
{
    public class Repository : IRepository, IDisposable
    {
        private IDriver _Driver = GraphDatabase.Driver(CommonConfig.Instance.Repository);

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

        public T FindNode<T>(String id) where T : BigPicture.Core.INode
        {
            var result = this.Read($"MATCH (a) WHERE Id(a) = {id} RETURN a");

            var resultList = result.Select(a => this.ToNode(a, typeof(T))).ToList();
            if(resultList.Count > 0)
            {
                return (T) resultList[0];
            }


            return default(T);
        }

        public string CreateRelationship(String from, String to, String relationShip, dynamic dataObject = null)
        {
            var data = "";
            if (dataObject != null)
            {
                data = ToJson(dataObject);
            }

            var statement = $@"
                MATCH (s), (n)
                where ID(s) = {from} and ID(n) = {to}
                CREATE (s)-[r:{relationShip} {data}]->(n)
                return r";

            var result = this.Write(statement);
            return result.Peek()[0].ToString();
        }

        public string CreateNode(object node, params String[] nodeTypes)
        {
            var nodeType = String.Join(":", nodeTypes);
            var jsonData = ToJson(node);
            var statement = $"CREATE (a:{nodeType} {jsonData}) RETURN id(a)";

            var result = this.Write(statement);

            return result.Peek()[0].ToString();
        }

        public void UpdateNode<T>(T node, params String[] nodeTypes) where T : BigPicture.Core.INode
        {
            var nodeType = String.Join("", nodeTypes.Select(a => ":" + a));
            var typeAssign = "";
            if(String.IsNullOrEmpty(nodeType) == false)
            {
                typeAssign = $", n{nodeType}";
            }

            var jsonData = ToJson(node);
            var statement = $"MATCH (n) WHERE ID(n) = {node.Id} SET n = {jsonData} {typeAssign} RETURN n";

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

            var resultList = result.Select(a => this.ToNode(a, type)).ToList();            
            return resultList;
        }

        public String FindIdOrCreateSubNode(object node, String nodeType, String id, String relation, String subNodeType, dynamic filterObject = null)
        {
            var filter = "";
            if (filterObject != null)
            {
                filter = ToJson(filterObject);
            }

            var result = this.Read($"MATCH (a:{nodeType})-[:{relation}]->(b:{subNodeType} {filter}) WHERE Id(a) = {id} RETURN a");

            var resultList = result.Select(a => this.ToNode(a, node.GetType())).ToList();
            if(resultList.Count > 0)
            {
                return resultList[0].Id;
            }

            var subNodeId = this.CreateNode(node, subNodeType);
            this.CreateRelationship(id, subNodeId, relation);
            return subNodeId;            
        }

        public List<T> GetAllNodes<T>(String nodeType, object filterObject = null) where T : BigPicture.Core.INode
        {
            List<BigPicture.Core.INode> result = this.GetAllNodes(nodeType, typeof(T), filterObject);
            return result.Cast<T>().ToList();
        }

        public String FindIdOrCreate(object node, String nodeType, object filterObject)
        {
            return this.FindIdOrCreate(node, new string[] { nodeType }, filterObject);
        }

        public String FindIdOrCreate(object node, String[] nodeTypes, object filterObject)
        {
            var result = this.GetAllNodes(nodeTypes[0], node.GetType(), filterObject);
            if (result.Count > 0)
            {
                return result[0].Id;
            }
            else
            {
                lock (_Driver)
                {
                    result = this.GetAllNodes(nodeTypes[0], node.GetType(), filterObject);
                    if (result.Count > 0)
                    {
                        return result[0].Id;
                    }
                    else
                    {
                        var id = this.CreateNode(node, nodeTypes);
                        return id;
                    }
                }
            }
        }

        private IStatementResult Read(String statement)
        {
            using (var session = this._Driver.Session())
            {
                var result = session.Run(statement);
                return result;                    
            }
        }

        private IStatementResult Write(String statement)
        {
            using (var session = this._Driver.Session())
            {
                return session.WriteTransaction(tx =>
                {
                    var result = tx.Run(statement);
                    return result;
                });
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

        private Core.INode ToNode(IRecord record, Type type)
        {
            var nodeProps = JsonConvert.SerializeObject(record[0].As<NeoINode>().Properties);
            var node = (BigPicture.Core.INode)JsonConvert.DeserializeObject(nodeProps, type);
            node.Id = record[0].As<NeoINode>().Id.ToString();

            return node;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._Driver != null)
                {
                    this._Driver.Dispose();
                }
            }
        }
    }
}
