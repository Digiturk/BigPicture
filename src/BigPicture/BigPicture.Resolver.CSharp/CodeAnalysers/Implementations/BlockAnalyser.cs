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
                    method.Assembly = typeDef.Assembly;
                    method.NameSpace = typeDef.NameSpace;
                    method.OwnerName = typeDef.Name;
                    method.HasBody = false;
                    method.Name = memberAccess.Name;

                    memberId = this._Repository.FindIdOrCreate(method, "Method", new
                    {
                        Assembly = method.Assembly,
                        NameSpace = method.NameSpace,
                        OwnerName = method.OwnerName,
                        Name = method.Name
                    });
                }
                else if (memberAccess.Kind == "Property")
                {
                    var property = new Property();
                    property.Assembly = typeDef.Assembly;
                    property.NameSpace = typeDef.NameSpace;
                    property.OwnerName = typeDef.Name;
                    property.Name = memberAccess.Name;

                    memberId = this._Repository.FindIdOrCreate(property, "Property", new
                    {
                        Assembly = property.Assembly,
                        NameSpace = property.NameSpace,
                        OwnerName = property.OwnerName,
                        Name = property.Name
                    });
                }
                else if (memberAccess.Kind == "Field")
                {
                    var field = new Field();
                    field.Assembly = typeDef.Assembly;
                    field.NameSpace = typeDef.NameSpace;
                    field.OwnerName = typeDef.Name;
                    field.Name = memberAccess.Name;

                    memberId = this._Repository.FindIdOrCreate(field, "Field", new
                    {
                        Assembly = field.Assembly,
                        NameSpace = field.NameSpace,
                        OwnerName = field.OwnerName,
                        Name = field.Name
                    });
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
                    this._Repository.CreateRelationship(parentId, memberId, "ACCESS", new {
                        Code = memberAccess.Code,
                        ParamNames = memberAccess.ParamNames,
                        ParamValues = memberAccess.ParamValues,
                        ParamCodes = memberAccess.ParamCodes
                    });
                }
            }
        }
    }
}
