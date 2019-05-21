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
    public class FieldDeclarationAnalyser : IAnalyser<FieldDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }

        public FieldDeclarationAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, FieldDeclarationSyntax node, SemanticModel model)
        {
            var typeSyntax = node.Declaration.Type;
            var symbol = model.GetSymbolInfo(typeSyntax);
            var variableSyntax = node.Declaration.Variables[0];

            #region Create Field definition

            var field = new Field();
            field.Name = variableSyntax.Identifier.Text;
            field.Modifier = String.Join(", ", node.Modifiers.Select(a => a.Text));

            field.Id = this._Repository.CreateNode(field, "Field");
            this._Repository.CreateRelationship(parentId, field.Id, "HAS");

            #endregion


            #region Find Type and create relationship

            var typeId = CodeResolver.FindOrCreateType(symbol.Symbol)?.Id;
            if(String.IsNullOrEmpty(typeId) == false)
            {
                this._Repository.CreateRelationship(field.Id, typeId, "TYPEOF");
            }

            #endregion


            #region

            if (variableSyntax.Initializer != null && variableSyntax.Initializer.Value != null)
            {
                if(variableSyntax.Initializer.Value.GetType() == typeof(ObjectCreationExpressionSyntax))
                {
                    var valueTypeSyntax = (variableSyntax.Initializer.Value as ObjectCreationExpressionSyntax).Type;
                    var valueSymbol = model.GetSymbolInfo(valueTypeSyntax);

                    typeId = CodeResolver.FindOrCreateType(valueSymbol.Symbol)?.Id;
                    if(String.IsNullOrEmpty(typeId) == false)
                    {
                        this._Repository.CreateRelationship(field.Id, typeId, "INITIALIZEDWITH");
                    }
                }
                else
                {

                }
            }

            #endregion

            foreach(var attr in node.AttributeLists)
            {
                CodeResolver.FindVisitorForNode(field.Id, model, attr);
            }
        }        


    }
}
