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
    public class StructDeclarationAnalyser : IAnalyser<StructDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }

        public StructDeclarationAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, StructDeclarationSyntax node, SemanticModel model)
        {
            var strc = new Struct();
            var symbol = model.GetDeclaredSymbol(node);

            strc.Name = node.Identifier.Text;
            strc.Assembly = symbol.ContainingAssembly.Identity.Name;
            strc.NameSpace = symbol.ContainingNamespace.ToString();
            strc.Modifier = String.Join(", ", node.Modifiers.Select(a => a.Text));
            strc.IsAbstract = symbol.IsAbstract;
            strc.IsSealed = symbol.IsSealed;
            strc.IsStatic = symbol.IsStatic;

            strc.Id = this._Repository.FindIdOrCreate(strc, "Struct", new { Name = strc.Name, NameSpace = strc.NameSpace, Assembly = strc.Assembly });
            this._Repository.UpdateNode<Struct>(strc, "Type");
            this._Repository.CreateRelationship(parentId, strc.Id, "CONTAINS");

            if (node.BaseList != null)
            {
                CodeResolver.FindVisitorForNode(strc.Id, model, node.BaseList);
            }

            // TODO parse attibutes
            foreach (var attrList in node.AttributeLists)
            {
                CodeResolver.FindVisitorForNode(strc.Id, model, attrList);
            }

            foreach (var member in node.Members)
            {
                CodeResolver.FindVisitorForNode(strc.Id, model, member);
            }
        }
    }
}
