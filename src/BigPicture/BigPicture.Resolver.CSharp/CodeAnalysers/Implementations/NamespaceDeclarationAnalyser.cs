using BigPicture.Core.Repository;
using BigPicture.Resolver.CSharp.Nodes;
using BigPicture.Resolver.CSharp.Resolvers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp.CodeAnalysers.Implementations
{
    public class NamespaceDeclarationAnalyser : IAnalyser<NamespaceDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }

        public NamespaceDeclarationAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, NamespaceDeclarationSyntax node, SemanticModel model)
        {
            var ns = new NameSpace();

            var child = node.ChildNodes().First();
            ns.Text = child.GetText().ToString().Trim();

            ns.Id = this._Repository.FindIdOrCreate(ns, "NameSpace", new { Text = ns.Text });
            this._Repository.CreateRelationship(parentId, ns.Id, "CONTAINS");

            foreach (var childNode in node.ChildNodes())
            {
                if(childNode.GetType() == typeof(QualifiedNameSyntax))
                {
                    continue;
                }

                CodeResolver.FindVisitorForNode(ns.Id, model, childNode);
            }
        }
    }
}
