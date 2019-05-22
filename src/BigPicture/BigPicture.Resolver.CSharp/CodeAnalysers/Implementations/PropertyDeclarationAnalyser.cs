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
    public class PropertyDeclarationAnalyser : IAnalyser<PropertyDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }

        public PropertyDeclarationAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, PropertyDeclarationSyntax node, SemanticModel model)
        {
            var owner = this._Repository.FindNode<Nodes.Type>(parentId);

            var typeSyntax = node.Type;            
            var symbol = model.GetSymbolInfo(typeSyntax);            

            #region Create Field definition

            var property = new Property();
            property.Assembly = owner.Assembly;
            property.NameSpace = owner.NameSpace;
            property.OwnerName = owner.Name;
            property.Name = node.Identifier.Text;
            property.Modifier = String.Join(", ", node.Modifiers.Select(a => a.Text));

            property.Id = this._Repository.FindIdOrCreate(property, "Property", new
            {
                Assembly = property.Assembly,
                NameSpace = property.NameSpace,
                OwnerName = property.OwnerName,
                Name = property.Name
            });
            this._Repository.CreateRelationship(parentId, property.Id, "HAS");

            #endregion

            #region Find Type and create relationship

            var typeId = CodeResolver.FindOrCreateType(symbol.Symbol)?.Id;
            if(String.IsNullOrEmpty(typeId) == false)
            {
                this._Repository.CreateRelationship(property.Id, typeId, "TYPEOF");
            }

            #endregion

            foreach(var attr in node.AttributeLists)
            {
                CodeResolver.FindVisitorForNode(property.Id, model, attr);
            }

            foreach (var accessor in node.AccessorList.ChildNodes())
            {
                CodeResolver.FindVisitorForNode(property.Id, model, accessor);
            }
        }        
    }
}
