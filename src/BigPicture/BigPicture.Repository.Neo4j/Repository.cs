using BigPicture.Core.Repository;
using Neo4j.Driver.V1;
using System;

namespace BigPicture.Repository.Neo4j
{
    public class Repository : IRepository
    {
        public string TestConnection()
        {
            try
            {
                using (var driver = GraphDatabase.Driver("bolt://localhost:7687"))
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
    }
}
