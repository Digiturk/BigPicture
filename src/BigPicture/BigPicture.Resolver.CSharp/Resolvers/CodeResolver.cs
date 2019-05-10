using BigPicture.Core.Resolver;
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

namespace BigPicture.Resolver.CSharp.Resolvers
{
    public class CodeResolver : IResolver<Nodes.Project>
    {
        public void Resolve(Nodes.Project projectNode)
        {
            var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += Workspace_WorkspaceFailed;

            var project = workspace.OpenProjectAsync(projectNode.AbsolutePath).Result;
            var compilation = project.GetCompilationAsync().Result;

            foreach(var syntaxTree in compilation.SyntaxTrees)
            {
                this.ProcessSyntaxTree(projectNode, syntaxTree);
            }
        }

        private void ProcessSyntaxTree(Nodes.Project projectNode, SyntaxTree tree)
        {
            var rootSyntaxNode = tree.GetRootAsync().Result;

            foreach (var node in rootSyntaxNode.ChildNodes())
            {
                var nodeType = node.GetType();
                if (nodeType == typeof(UsingDirectiveSyntax))
                {
                    this.ParseUsingNode(node as UsingDirectiveSyntax);                    
                }
                else if(nodeType == typeof(QualifiedNameSyntax))
                {
                    var qualifiedNode = node as QualifiedNameSyntax;
                }
                else if(nodeType == typeof(NamespaceDeclarationSyntax))
                {
                    var namesNode = node as NamespaceDeclarationSyntax;
                }
                else
                {
                    Console.WriteLine("Unknown Node Type: " + nodeType);
                }
            }

        }

        private void ParseUsingNode(UsingDirectiveSyntax usingNode)
        {

        }

        private void Workspace_WorkspaceFailed(object sender, Microsoft.CodeAnalysis.WorkspaceDiagnosticEventArgs e)
        {
            // Console.WriteLine("Workspace Failed: " + e.Diagnostic.Message);
        }        
    }
}
