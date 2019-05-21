using BigPicture.Core.Repository;
using BigPicture.Resolver.CSharp.Nodes;
using BigPicture.Resolver.CSharp.Resolvers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace BigPicture.Resolver.CSharp.CodeAnalysers.Implementations
{
    public class BlockAnalyser : IAnalyser<BlockSyntax>
    {
        private IRepository _Repository { get; set; }
        public SemanticModel _Model { get; set; }

        public BlockAnalyser(IRepository repository)
        {
            this._Repository = repository;        
        }

        public void Analyse(string parentId, BlockSyntax node, SemanticModel model)
        {
            this._Model = model;

            var syntaxWalker = new MySyntaxWalker(model);
            syntaxWalker.Visit(node);

            var memberAccessList = syntaxWalker.GetMemberAccesses();
            foreach(var memberAccess in memberAccessList)
            {
                var memberId = "";

                var typeDef = CodeResolver.FindOrCreateType(memberAccess.Assembly, memberAccess.NameSpace, memberAccess.TypeName, "Type");

                if (memberAccess.Kind == "Method")
                {
                    var method = new Method();
                    method.Assemly = typeDef.Assembly;
                    method.NameSpace = typeDef.NameSpace;
                    method.OwnerName = typeDef.Name;
                    method.HasBody = false;
                    method.Name = memberAccess.Name;

                    memberId = this._Repository.FindIdOrCreate(method, "Method", new
                    {
                        Assembly = method.Assemly,
                        NameSpace = method.NameSpace,
                        OwnerName = method.OwnerName,
                        Name = method.OwnerName
                    });
                }
                else if (memberAccess.Kind == "Property")
                {
                    var property = new Property();
                    property.Name = memberAccess.Name;

                    memberId = this._Repository.FindIdOrCreateSubNode(property, "Type", typeDef.Id, "HAS", "Property", new { Name = memberAccess.Name });
                }
                else if (memberAccess.Kind == "Field")
                {
                    var field = new Field();
                    field.Name = memberAccess.Name;

                    memberId = this._Repository.FindIdOrCreateSubNode(field, "Type", typeDef.Id, "HAS", "Field", new { Name = memberAccess.Name });
                }
                else if(memberAccess.Kind == "Event")
                {
                    // TODO Event
                }
                else
                {
                }

                if (String.IsNullOrEmpty(memberId) == false)
                {
                    this._Repository.CreateRelationship(parentId, memberId, "ACCESS");
                }
            }
        }
    }
}
