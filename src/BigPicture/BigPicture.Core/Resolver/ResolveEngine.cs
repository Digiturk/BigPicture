using BigPicture.Core.Config;
using BigPicture.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core.Resolver
{
    public class ResolveEngine : IResolveEngine
    {
        public IRepository Repository { get; set; }

        public ResolveEngine(IRepository repository)
        {
            this.Repository = repository;
        }

        public void LoadStartData()
        {
            foreach(var startData in ResolversConfig.Instance.StartData)
            {
                var id = this.Repository.CreateNode(startData.NodeTypeName, startData.Data);
                Console.WriteLine(startData.NodeTypeName + " save to repository with " + id + " id");
            }
        }

        public void StartResolvers()
        {
            Console.WriteLine("Resolvers starting...");
        }
    }
}
