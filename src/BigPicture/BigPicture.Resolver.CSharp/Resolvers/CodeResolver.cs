using BigPicture.Core.IOC;
using BigPicture.Core.Repository;
using BigPicture.Core.Resolver;
using BigPicture.Resolver.CSharp.CodeAnalysers;
using BigPicture.Resolver.CSharp.Nodes;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp.Resolvers
{
    public class CodeResolver : IResolver<Nodes.Project>
    {
        private CSharpCompilation _Compilation { get; set; }

        private IRepository _Repository { get; set; }

        public CodeResolver(IRepository repository)
        {
            this._Repository = repository;
        }
        private static VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
        {            
            if (visualStudioInstances.Length == 1)
            {
                return visualStudioInstances[0];
            }

            Console.WriteLine("Multiple installs of MSBuild detected please select one:");
            for (int i = 0; i < visualStudioInstances.Length; i++)
            {
                Console.WriteLine($"Instance {i + 1}");
                Console.WriteLine($"    Name: {visualStudioInstances[i].Name}");
                Console.WriteLine($"    Version: {visualStudioInstances[i].Version}");
                Console.WriteLine($"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
            }

            while (true)
            {
                var userResponse = Console.ReadLine();
                if (int.TryParse(userResponse, out int instanceNumber) &&
                    instanceNumber > 0 &&
                    instanceNumber <= visualStudioInstances.Length)
                {
                    return visualStudioInstances[instanceNumber - 1];
                }
                Console.WriteLine("Input not accepted, try again.");
            }
        }
        public void Resolve(Nodes.Project projectNode)
        {
            VisualStudioInstance[] visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            VisualStudioInstance instance = visualStudioInstances.Length == 1
                // If there is only one instance of MSBuild on this machine, set that as the one to use.
                ? visualStudioInstances[0]
                // Handle selecting the version of MSBuild you want to use.
                : SelectVisualStudioInstance(visualStudioInstances);

            Console.WriteLine($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");
            try
            {
                MSBuildLocator.RegisterInstance(instance);

            }
            catch (Exception ex)
            {
                if(!ex.Message.Contains("MSBuild assemblies were already loaded"))
                {
                    throw ex;
                }
            }
            // NOTE: Be sure to register an instance with the MSBuildLocator 
            //       before calling MSBuildWorkspace.Create()
            //       otherwise, MSBuildWorkspace won't MEF compose.
           
            var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += Workspace_WorkspaceFailed;

            var project = workspace.OpenProjectAsync(projectNode.AbsolutePath).Result;
            var compilation = project.GetCompilationAsync().Result;

            _Compilation = compilation as CSharpCompilation;
#if DEBUG || Debug || debug
            foreach (var syntaxTree in compilation.SyntaxTrees)
            {
                this.ProcessSyntaxTree(projectNode, syntaxTree);
            }
#else

            Parallel.ForEach<SyntaxTree>(compilation.SyntaxTrees, (SyntaxTree syntaxTree) =>
            {
                this.ProcessSyntaxTree(projectNode, syntaxTree);
            });
#endif
        }

        private void ProcessSyntaxTree(Nodes.Project projectNode, SyntaxTree tree)
        {
            var rootSyntaxNode = tree.GetRootAsync().Result;
            var model = this._Compilation.GetSemanticModel(tree);

            var compileItemId = "";

            var list = this._Repository.GetAllNodes<CompileItem>("CompileItem", new { AbsolutePath = tree.FilePath });
            if (list.Count == 0)
            {
                // TODO Log
                Console.Error.WriteLine("Compile Item could not found!");
                return;
            }
            else
            {
                compileItemId = list[0].Id;
            }

            foreach (var node in rootSyntaxNode.ChildNodes())
            {
                FindVisitorForNode(compileItemId, model, node);
            }
        }

        public static void FindVisitorForNode(String parentId, SemanticModel model, SyntaxNode node)
        {
            var nodeType = node.GetType();
            var analyserType = typeof(IAnalyser<>).MakeGenericType(nodeType);
            var analyser = Container.TryResolve(analyserType);
            if (analyser != null)
            {
                var method = analyserType.GetMethod("Analyse");
                method.Invoke(analyser, new object[] { parentId, node, model });
            }
            else
            {
                if (nodeType != typeof(DelegateDeclarationSyntax) &&
                    nodeType != typeof(EventFieldDeclarationSyntax) &&
                    nodeType != typeof(IndexerDeclarationSyntax))
                {

                }
                // TODO log unknown syntax node
            }
        }

        public static Nodes.Type FindOrCreateType(ISymbol symbol)
        {
            var namedSymbol = symbol as INamedTypeSymbol;

            if (namedSymbol == null)
            {
                // TODO parse other types. Array ...
                return null;
            }

            String nodeType = "";

            if (namedSymbol.TypeKind == TypeKind.Class)
            {
                nodeType = "Class";
            }
            else if (namedSymbol.TypeKind == TypeKind.Enum)
            {
                nodeType = "Enum";
            }
            else if (namedSymbol.TypeKind == TypeKind.Interface)
            {
                nodeType = "Interface";
            }
            else if (namedSymbol.TypeKind == TypeKind.Struct)
            {
                nodeType = "Struct";
            }
            else
            {
                nodeType = "Type";
            }

            return FindOrCreateType(symbol.ContainingAssembly.Identity.Name, symbol.ContainingNamespace.ToString(), symbol.Name, nodeType, "Type");
        }

        public static Nodes.Type FindOrCreateType(String assembly, String nameSpace, String name, params String[] nodeTypes)
        {
            var node = new Nodes.Type();
            node.Assembly = assembly;
            node.Name = name;
            node.NameSpace = nameSpace;            

            var repository = Container.Resolve<IRepository>();
            node.Id = repository.FindIdOrCreate(node, nodeTypes, new { Name = name, NameSpace = nameSpace, Assembly = assembly });
            return node;
        }

        private void Workspace_WorkspaceFailed(object sender, WorkspaceDiagnosticEventArgs e)
        {
             Console.WriteLine("Workspace Failed: " + e.Diagnostic.Message);
        }        
    }
}
