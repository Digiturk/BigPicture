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
    public class MethodDeclarationAnalyser : IAnalyser<MethodDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }

        public MethodDeclarationAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, MethodDeclarationSyntax node, SemanticModel model)
        {
            var method = new Method();

            method.Name = node.Identifier.Text;
            method.Modifier = String.Join(", ", node.Modifiers.Select(a => a.Text));
            method.HasBody = node.Body != null;

            method.Id = this._Repository.CreateNode(method, "Method");
            this._Repository.CreateRelationship(parentId, method.Id, "HAS");

            var returnSymbol = model.GetSymbolInfo(node.ReturnType);
            var returnTypeId = CodeResolver.FindOrCreateType(returnSymbol.Symbol);
            if(String.IsNullOrEmpty(returnTypeId) == false)
            {
                this._Repository.CreateRelationship(method.Id, returnTypeId, "TYPEOF");
            }

            foreach(var prmNode in node.ParameterList.ChildNodes())
            {
                CodeResolver.FindVisitorForNode(parentId, model, prmNode);
            }

            if(node.Body != null)
            {
                CodeResolver.FindVisitorForNode(parentId, model, node.Body);
            }
        }        
    }
}
