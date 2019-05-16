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
    public class EnumMemberDeclarationAnalyser : IAnalyser<EnumMemberDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }

        public EnumMemberDeclarationAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, EnumMemberDeclarationSyntax node, SemanticModel model)
        {
            var enumMember = new EnumMember();
            enumMember.Name = node.Identifier.Text;
            enumMember.Value = node.EqualsValue?.Value?.ToString()??"";

            #region Create Field definition

            enumMember.Id = this._Repository.CreateNode(enumMember, "EnumMember");
            this._Repository.CreateRelationship(parentId, enumMember.Id, "HAS");

            #endregion

            foreach(var attr in node.AttributeLists)
            {
                CodeResolver.FindVisitorForNode(enumMember.Id, model, attr);
            }
        }        


    }
}
