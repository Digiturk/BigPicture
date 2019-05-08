using BigPicture.Core.Config;
using BigPicture.Core.IOC;
using BigPicture.Core.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            LoadStartData();

            foreach(var resolverDefinition in ResolversConfig.Instance.Resolvers)
            {
                Resolve(resolverDefinition);
            }
        }

        public void Resolve(ResolverDefinition resolverDefinition)
        {
            Console.WriteLine($"Starting {resolverDefinition.Name} resolver...");
            var sw = Stopwatch.StartNew();

            try
            {
                var type = Type.GetType(resolverDefinition.NodeType);
                var nodes = this.Repository.GetAllNodes(resolverDefinition.Name, type);

                var resolverType = typeof(IResolver<>).MakeGenericType(type);
                var resolver = Container.Resolve(resolverType);

                foreach(var node in nodes)
                {
                    resolverType.GetMethod("Resolve").Invoke(resolver, new object[] { node });
                }
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine($"{resolverDefinition.Name} error: {ex.Message}");
            }


            sw.Stop();
            Console.WriteLine($"Finished {resolverDefinition.Name} resolver: {sw.ElapsedMilliseconds}ms");
        }
    }
}
