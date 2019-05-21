using Autofac;
using BigPicture.Core.Config;
using BigPicture.Core.IOC;
using BigPicture.Core.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                var id = this.Repository.CreateNode(startData.Data, startData.NodeTypeName);
                Console.WriteLine(startData.NodeTypeName + " saved to repository with " + id + " id");
            }
        }

        public void StartResolvers()
        {
            if(ResolversConfig.Instance.Options.RemoveAllOnStart)
            {
                this.Repository.DeleteAll();
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
                var nodes = this.Repository.GetAllNodes(resolverDefinition.Resolves, type);                

                var resolverType = typeof(IResolver<>).MakeGenericType(type);
                var resolver = Container.ResolveWithKey(resolverDefinition.Name, resolverType);

                using (var progress = new ProgressBar())
                {
                    var progressCount = 0d;
                    progress.Report(progressCount);
                    
                    if(resolverDefinition.RunParallel)
                    {
                        Parallel.ForEach<INode>(nodes, (INode node) =>
                        {
                            resolverType.GetMethod("Resolve").Invoke(resolver, new object[] { node });
                            progressCount++;
                            progress.Report(progressCount / nodes.Count);
                        });
                    }
                    else
                    {
                        foreach(var node in nodes)
                        {
                            resolverType.GetMethod("Resolve").Invoke(resolver, new object[] { node });
                            progressCount++;
                            progress.Report(progressCount / nodes.Count);
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

    public class ProgressBar : IDisposable, IProgress<double>
    {
        private const int blockCount = 10;
        private readonly TimeSpan animationInterval = TimeSpan.FromSeconds(1.0 / 8);
        private const string animation = @"|/-\";

        private readonly Timer timer;

        private double currentProgress = 0;
        private string currentText = string.Empty;
        private bool disposed = false;
        private int animationIndex = 0;

        public ProgressBar()
        {
            timer = new Timer(TimerHandler);

            // A progress bar is only for temporary display in a console window.
            // If the console output is redirected to a file, draw nothing.
            // Otherwise, we'll end up with a lot of garbage in the target file.
            if (!Console.IsOutputRedirected)
            {
                ResetTimer();
            }
        }

        public void Report(double value)
        {
            // Make sure value is in [0..1] range
            value = Math.Max(0, Math.Min(1, value));
            Interlocked.Exchange(ref currentProgress, value);
        }

        private void TimerHandler(object state)
        {
            lock (timer)
            {
                if (disposed) return;

                int progressBlockCount = (int)(currentProgress * blockCount);
                int percent = (int)(currentProgress * 100);
                string text = string.Format("[{0}{1}] {2,3}% {3}",
                    new string('#', progressBlockCount), new string('-', blockCount - progressBlockCount),
                    percent,
                    animation[animationIndex++ % animation.Length]);
                UpdateText(text);

                ResetTimer();
            }
        }

        private void UpdateText(string text)
        {
            // Get length of common portion
            int commonPrefixLength = 0;
            int commonLength = Math.Min(currentText.Length, text.Length);
            while (commonPrefixLength < commonLength && text[commonPrefixLength] == currentText[commonPrefixLength])
            {
                commonPrefixLength++;
            }

            // Backtrack to the first differing character
            StringBuilder outputBuilder = new StringBuilder();
            outputBuilder.Append('\b', currentText.Length - commonPrefixLength);

            // Output new suffix
            outputBuilder.Append(text.Substring(commonPrefixLength));

            // If the new text is shorter than the old one: delete overlapping characters
            int overlapCount = currentText.Length - text.Length;
            if (overlapCount > 0)
            {
                outputBuilder.Append(' ', overlapCount);
                outputBuilder.Append('\b', overlapCount);
            }

            Console.Write(outputBuilder);
            currentText = text;
        }

        private void ResetTimer()
        {
            timer.Change(animationInterval, TimeSpan.FromMilliseconds(-1));
        }

        public void Dispose()
        {
            lock (timer)
            {
                disposed = true;
                UpdateText(string.Empty);
            }
        }

    }
}
