using Autofac;
using BigPicture.Core.Config;
using BigPicture.Core.IOC;
using BigPicture.Core.Repository;
using Replify.Net.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace BigPicture.Core.Resolver
{
    public class ResolveEngine : IResolveEngine
    {
        public IRepository Repository { get; set; }
        public ICodeRepository CodeRepository { get; set; }

        public ResolveEngine(IRepository repository, ICodeRepository codeRepository)
        {
            this.Repository = repository;
            this.CodeRepository= codeRepository;
        }

        public void LoadStartData()
        {
            foreach(var startData in ResolversConfig.Instance.StartData)
            {
                var id = this.Repository.CreateNode(startData.Data, startData.NodeTypeName);
                Console.WriteLine(startData.NodeTypeName + " saved to repository with " + id + " id");
            }
        }

        public void StartResolvers()
        {
            if(ResolversConfig.Instance.Options.RemoveAllOnStart)
            {
                Console.WriteLine("Removing all graph data...");
                this.Repository.DeleteAll();
                Console.WriteLine("Removing all code data...");
                this.CodeRepository.RemoveAllCodeBlocks();
            }
            LoadStartData();

            Console.WriteLine();
            foreach (var resolverDefinition in ResolversConfig.Instance.Resolvers)
            {
                Resolve(resolverDefinition);
            }
        }

        public void StartResolver(String name)
        {
            var resolverDefinition = ResolversConfig.Instance.Resolvers.Find(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if(resolverDefinition != null)
            {
                Resolve(resolverDefinition);
            }
            else
            {
                Console.Error.WriteLine("Resolver definition could not be found for " + name);
            }
        }

        public void Resolve(ResolverDefinition resolverDefinition)
        {
            Console.WriteLine($"Starting {resolverDefinition.Name} resolver...");
            var sw = Stopwatch.StartNew();

            try
            {
                var type = Type.GetType(resolverDefinition.NodeType);

                IEnumerable<Object> nodes = null;
                if(String.IsNullOrEmpty(resolverDefinition.CustomQuery))
                {
                    nodes = this.Repository.GetAllNodes(resolverDefinition.Resolves, type).OfType<Object>();
                }
                else
                {
                    nodes = this.Repository.RunCustomQuery(resolverDefinition.CustomQuery, type);
                }
                
                var resolverType = typeof(IResolver<>).MakeGenericType(type);
                var resolver = Container.ResolveWithKey(resolverDefinition.Name, resolverType);

                using (var progress = new ConsoleProgress())
                {
                    var progressCount = 0d;
                    var totalCount = nodes.Count();
                    progress.Report(progressCount);
                    
                    if(resolverDefinition.RunParallel)
                    {
                        Parallel.ForEach(nodes, new ParallelOptions() { MaxDegreeOfParallelism = resolverDefinition.MaxParallel??10 }, (object node) =>
                        {
                            resolverType.GetMethod("Resolve").Invoke(resolver, new object[] { node });
                            progressCount++;
                            progress.Report(progressCount / totalCount);
                        });
                    }
                    else
                    {
                        foreach(var node in nodes)
                        {
                            resolverType.GetMethod("Resolve").Invoke(resolver, new object[] { node });
                            progressCount++;
                            progress.Report(progressCount / totalCount);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine($"{resolverDefinition.Name} error: {ex.Message}");
            }

            sw.Stop();
            Console.WriteLine($"Finished {resolverDefinition.Name} resolver: {sw.ElapsedMilliseconds}ms");

            Console.WriteLine();
        }
    }
}
