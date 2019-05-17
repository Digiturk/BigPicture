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
    public class DestructorDeclarationAnalyser : IAnalyser<DestructorDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }

        public DestructorDeclarationAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, DestructorDeclarationSyntax node, SemanticModel model)
        {
            var method = new Destructor();

            method.Name = node.Identifier.Text;
            method.Modifier = String.Join(", ", node.Modifiers.Select(a => a.Text));

            method.Id = this._Repository.CreateNode(method, "Destructor");
            this._Repository.CreateRelationship(parentId, method.Id, "HAS");

            foreach(var prmNode in node.ParameterList.ChildNodes())
            {
                CodeResolver.FindVisitorForNode(method.Id, model, prmNode);
            }

            foreach (var attr in node.AttributeLists)
            {
                CodeResolver.FindVisitorForNode(method.Id, model, attr);
            }            

            if (node.Body != null)
            {
                CodeResolver.FindVisitorForNode(method.Id, model, node.Body);
            }
        }        
    }
}
