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
    public class SchemaResolver : IResolver<Schema>
    {
        private IRepository _Repository { get; set; }

        public SchemaResolver(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Resolve(Schema schema)
        {
            var connectionString = CommonConfig.Instance.Options[schema.DatabaseName];

            using (var con = new OracleConnection(connectionString))
            {
                con.Open();
                using (var com = con.CreateCommand())
                {
                    com.CommandText = @"
                      SELECT OBJECT_NAME, OBJECT_TYPE
                        FROM SYS.ALL_OBJECTS
                       WHERE OBJECT_TYPE IN ('TABLE', 'TYPE', 'PACKAGE', 'VIEW', 'PROCEDURE', 'FUNCTION', 'SEQUENCE')
                         AND OWNER = :P_OWNER     
                    ";

                    com.Parameters.Add("P_OWNER", schema.Name);

                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var objectName = reader.GetString(0);
                            var objectType = reader.GetString(1);

                            SaveObject(schema, objectName, objectType);
                        }
                    }
                }
            }
        }

        private void SaveObject(Schema schema, String objectName, String objectType)
        {
            var type = "";

            if(objectType == "TABLE")
            {
                type = "Table";
            }
            else if (objectType == "TYPE")
            {
                type = "OracleType";
            }
            else if (objectType == "PACKAGE")
            {
                type = "Package";
            }
            else if (objectType == "VIEW")
            {
                type = "View";
            }
            else if (objectType == "PROCEDURE")
            {
                type = "Procedure";
            }
            else if (objectType == "FUNCTION")
            {
                type = "Function";
            }
            else if (objectType == "SEQUENCE")
            {
                type = "Sequence";
            }

            var dbObject = new DbObject();
            dbObject.DatabaseName = schema.DatabaseName;
            dbObject.Name = objectName;
            dbObject.SchemaName = schema.Name;
            dbObject.Type = objectType;

            dbObject.Id = this._Repository.CreateNode(dbObject, "DbObject", type);
            this._Repository.CreateRelationship(schema.Id, dbObject.Id, "CONTAINS");
        }
    }
}
