using BigPicture.Core.Repository;
using MongoDB.Driver;
using System;

namespace BigPicture.Repository.MongoDB
{
    public class RepositoryContext : IRepositoryContext
    {
        public string TestConnection()
        {
            try
            {
                var client = new MongoClient("mongodb://localhost:27017");
                var databaseNames = client.ListDatabaseNames();

                return "Connection is successful";
            }
            catch (Exception ex)
            {
                return ex.Message;           
            }
            
        }
    }
}
