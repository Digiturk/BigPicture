﻿using BigPicture.Core.Resolver;
using BigPicture.Resolver.CSharp.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.MSBuild;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using BigPicture.Core.Repository;
using Microsoft.CodeAnalysis.CSharp;
using BigPicture.Core.IOC;
using BigPicture.Resolver.CSharp.CodeAnalysers;

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

        public void Resolve(Nodes.Project projectNode)
        {
            var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += Workspace_WorkspaceFailed;

            var project = workspace.OpenProjectAsync(projectNode.AbsolutePath).Result;
            var compilation = project.GetCompilationAsync().Result;

            _Compilation = compilation as CSharpCompilation;

            //foreach (var syntaxTree in compilation.SyntaxTrees)
            //{
            //    this.ProcessSyntaxTree(projectNode, syntaxTree);
            //}

            Parallel.ForEach<SyntaxTree>(compilation.SyntaxTrees, (SyntaxTree syntaxTree) =>
            {
                this.ProcessSyntaxTree(projectNode, syntaxTree);
            });
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
                //Console.Error.WriteLine("Compile Item could not found!");
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
            // Console.WriteLine("Workspace Failed: " + e.Diagnostic.Message);
        }        
    }
}
