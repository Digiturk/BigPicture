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
    public class AccessorDeclarationAnalyser : IAnalyser<AccessorDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }

        public AccessorDeclarationAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, AccessorDeclarationSyntax node, SemanticModel model)
        {
            var accessor = new Accessor();

            accessor.Name = node.Keyword.Text;
            accessor.Modifier = String.Join(", ", node.Modifiers.Select(a => a.Text));
            accessor.HasBody = node.Body != null;

            accessor.Id = this._Repository.CreateNode(accessor, "Accessor");
            this._Repository.CreateRelationship(parentId, accessor.Id, "HAS");

            foreach(var attr in node.AttributeLists)
            {
                CodeResolver.FindVisitorForNode(accessor.Id, model, attr);
            }

            if(node.Body != null)
            {
                CodeResolver.FindVisitorForNode(accessor.Id, model, node.Body);
            }
        }        
    }
}
