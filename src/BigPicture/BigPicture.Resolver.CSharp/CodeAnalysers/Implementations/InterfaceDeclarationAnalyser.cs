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
    public class InterfaceDeclarationAnalyser : IAnalyser<InterfaceDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }

        public InterfaceDeclarationAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, InterfaceDeclarationSyntax node, SemanticModel model)
        {
            var interFace = new Interface();
            var symbol = model.GetDeclaredSymbol(node);

            interFace.Name = node.Identifier.Text;
            interFace.Assembly = symbol.ContainingAssembly.Identity.Name;
            interFace.NameSpace = symbol.ContainingNamespace.ToString();
            interFace.Modifier = String.Join(", ", node.Modifiers.Select(a => a.Text));

            interFace.Id = this._Repository.FindIdOrCreate(interFace, "Interface", new { Name = interFace.Name, NameSpace = interFace.NameSpace, Assembly = interFace.Assembly });
            this._Repository.UpdateNode<Interface>(interFace, "Type");
            this._Repository.CreateRelationship(parentId, interFace.Id, "CONTAINS");

            if(node.BaseList != null)
            {
                CodeResolver.FindVisitorForNode(interFace.Id, model, node.BaseList);                
            }

            // TODO parse attibutes
            foreach (var attrList in node.AttributeLists)
            {
                CodeResolver.FindVisitorForNode(interFace.Id, model, attrList);
            }                        

            foreach (var member in node.Members)
            {
                CodeResolver.FindVisitorForNode(interFace.Id, model, member);
            }

            // TODO parse members
        }        
    }
}
