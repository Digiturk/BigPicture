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
    public class EnumDeclarationAnalyser : IAnalyser<EnumDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }

        public EnumDeclarationAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, EnumDeclarationSyntax node, SemanticModel model)
        {
            var enumm = new Nodes.Enum();
            var symbol = model.GetDeclaredSymbol(node);

            enumm.Name = node.Identifier.Text;
            enumm.Assembly = symbol.ContainingAssembly.Identity.Name;
            enumm.NameSpace = symbol.ContainingNamespace.ToString();
            enumm.Modifier = String.Join(", ", node.Modifiers.Select(a => a.Text));

            enumm.Id = this._Repository.FindIdOrCreate(enumm, "Enum", new { Name = enumm.Name, NameSpace = enumm.NameSpace, Assembly = enumm.Assembly });
            this._Repository.UpdateNode<Nodes.Enum>(enumm, "Type");
            this._Repository.CreateRelationship(parentId, enumm.Id, "CONTAINS");

            if(node.BaseList != null)
            {
                CodeResolver.FindVisitorForNode(enumm.Id, model, node.BaseList);                
            }

            // TODO parse attibutes
            foreach (var attrList in node.AttributeLists)
            {
                CodeResolver.FindVisitorForNode(enumm.Id, model, attrList);
            }                        

            foreach (var member in node.Members)
            {
                CodeResolver.FindVisitorForNode(enumm.Id, model, member);
            }

            // TODO parse members
        }        
    }
}
