using BigPicture.Core;
using BigPicture.Core.Config;
using BigPicture.Core.Repository;
using Neo4j.Driver.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Repository.Neo4j
{
    public class GraphRepository : IGraphRepository, IDisposable
    {
        private const String NODE_BASE_QUERY = "{ Name: node.Name, Labels: labels(node), Id: id(node)}";
        private IDriver _Driver = GraphDatabase.Driver(CommonConfig.Instance.Repository);

        public Node GetNodeById(String id)
        {
            var query = $@"
                match(node)
                where ID(node) = {id}
                return node {NODE_BASE_QUERY}";

            var queryResult = this.Read(query);
            var record = queryResult.FirstOrDefault();
            if(record == null)
            {
                return null;
            }
            
            var properties = record[0].As<Dictionary<String, object>>();
            return ToNode<Node>(properties);
        }

        public IEnumerable<Node> FindNodesByName(String name, int limit = 5, int skip = 0)
        {
            var query = $@"
                match (node)
                where node.Name =~ '.*{name}.*'
	            return node {NODE_BASE_QUERY}
                skip {skip}
	            limit {limit}";

            var queryResult = this.Read(query);

            var result = queryResult.Select(record => {
                var properties = record[0].As<Dictionary<String, object>>();
                return ToNode<Node>(properties);
            });
            return result;
        }

        private T ToNode<T>(Dictionary<String, object> properties)
        {
            var nodeProps = JsonConvert.SerializeObject(properties);
            var node = JsonConvert.DeserializeObject<T>(nodeProps);                        
            return node;
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
