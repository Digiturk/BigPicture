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
    public class TableResolver : IResolver<DbObject>
    {
        private IRepository _Repository { get; set; }

        public TableResolver(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Resolve(DbObject table)
        {
            FindColumns(table);
            FindIndexes(table);
        }

        private void FindColumns(DbObject table)
        {
            var connectionString = CommonConfig.Instance.Options[table.DatabaseName];

            using (var con = new OracleConnection(connectionString))
            {
                con.Open();
                using (var com = con.CreateCommand())
                {
                    com.CommandText = @"
                          SELECT COLUMN_ID, COLUMN_NAME, DATA_TYPE, 
                                 DATA_LENGTH, DATA_PRECISION, DATA_SCALE, 
                                 NULLABLE                  
                            FROM SYS.ALL_TAB_COLUMNS
                           WHERE OWNER = :P_OWNER
                             AND TABLE_NAME = :P_TABLE_NAME
                             AND COLUMN_ID IS NOT NULL
                        ORDER BY COLUMN_ID
                    ";

                    com.Parameters.Add("P_OWNER", table.SchemaName);
                    com.Parameters.Add("P_TABLE_NAME", table.Name);

                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var column = new Column();
                            column.ColumnId = reader.GetInt32(0);
                            column.Name = reader.GetString(1);
                            column.DataType = reader.GetString(2);
                            column.DataLength = reader.GetInt32(3);
                            column.DataPrecision = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                            column.DataScale = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                            column.Nullable = reader.GetString(6);

                            column.Id = this._Repository.CreateNode(column, "Column");
                            this._Repository.CreateRelationship(table.Id, column.Id, "CONTAINS");
                        }
                    }
                }
            }
        }

        private void FindIndexes(DbObject table)
        {
            var connectionString = CommonConfig.Instance.Options[table.DatabaseName];

            using (var con = new OracleConnection(connectionString))
            {
                con.Open();
                using (var com = con.CreateCommand())
                {
                    com.CommandText = @"
                          SELECT IND.OWNER, IND.INDEX_NAME, IND.INDEX_TYPE         
                            FROM SYS.ALL_INDEXES IND          
                           WHERE IND.TABLE_OWNER = :P_TABLE_OWNER
                             AND IND.TABLE_NAME = :P_TABLE_NAME
                    ";

                    com.Parameters.Add("P_TABLE_OWNER", table.SchemaName);
                    com.Parameters.Add("P_TABLE_NAME", table.Name);

                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var index = new DbObject();
                            index.SchemaName = reader.GetString(0);
                            index.Name = reader.GetString(1);
                            index.Type = reader.GetString(2);

                            index.Id = this._Repository.CreateNode(index, "DbObject", "Index");
                            this._Repository.CreateRelationship(table.Id, index.Id, "CONTAINS");
                        }
                    }
                }
            }
        }
    }
}
