using BigPicture.Core.Resolver;
using System;
using System.Collections.Generic;
using System.Text;
using BigPicture.Core.Repository;
using BigPicture.Resolver.Oracle.Nodes;
using Oracle.ManagedDataAccess.Client;
using BigPicture.Core.Config;

namespace BigPicture.Resolver.Oracle.Resolvers
{
    public class DependencyResolver : IResolver<DbObject>
    {
        private IRepository _Repository { get; set; }

        public DependencyResolver(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Resolve(DbObject dbObject)
        {           
            var connectionString = CommonConfig.Instance.Options[dbObject.DatabaseName];

            using (var con = new OracleConnection(connectionString))
            {
                con.Open();
                using (var com = con.CreateCommand())
                {
                    com.CommandText = @"
                      SELECT REFERENCED_OWNER, REFERENCED_NAME 
                        FROM SYS.ALL_DEPENDENCIES
                       WHERE OWNER = :P_OWNER
                         AND NAME = :P_NAME
                    ";

                    com.Parameters.Add("P_OWNER", dbObject.SchemaName);
                    com.Parameters.Add("P_NAME", dbObject.Name);

                    using (var reader = com.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            var refOwner = reader.GetString(0);
                            var refName = reader.GetString(1);

                            this.ProcessRef(dbObject, refOwner, refName);
                        }
                    }
                }
            }
        }

        private void ProcessRef(DbObject dbObject, String refOwner, String refName)
        {
            var refNodes = this._Repository.GetAllNodes<DbObject>("DbObject", new
            {
                DatabaseName = dbObject.DatabaseName,
                SchemaName = refOwner,
                Name = refName
            });

            if(refNodes.Count == 0)
            {
                return;
            }

            var id = refNodes[0].Id;

            this._Repository.CreateRelationship(dbObject.Id, id, "REFERENCES");
        }
    }
}
