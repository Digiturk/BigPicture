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
    public class DatabaseResolver : IResolver<Database>
    {
        private IRepository _Repository { get; set; }

        public DatabaseResolver(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Resolve(Database db)
        {
            var connectionString = CommonConfig.Instance.Options[db.Name];

            using (var con = new OracleConnection(connectionString))
            {
                con.Open();
                using (var com = con.CreateCommand())
                {
                    com.CommandText = "SELECT USERNAME FROM SYS.ALL_USERS WHERE ORACLE_MAINTAINED = 'N'";

                    using (var reader = com.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            var schema = new Schema();
                            schema.Name = reader.GetString(0);
                            schema.DatabaseName = db.Name;

                            schema.Id = this._Repository.CreateNode(schema, "Schema");
                            this._Repository.CreateRelationship(db.Id, schema.Id, "CONTAINS");
                        }
                    }
                }
            }
        }
    }
}
