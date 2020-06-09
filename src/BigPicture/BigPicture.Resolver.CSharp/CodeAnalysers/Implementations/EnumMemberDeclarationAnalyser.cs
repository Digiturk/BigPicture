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
        private ICodeRepository _CodeRepository { get; set; }

        public EnumMemberDeclarationAnalyser(IRepository repository, ICodeRepository codeRepository)
        {
            this._Repository = repository;
            this._CodeRepository = codeRepository;
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

            #region CodeRepository
            // Store code on document database (Elastic search)
            var codeBlock = new CodeBlock();
            codeBlock.Id = enumMember.Id;
            codeBlock.Language = "csharp";
            codeBlock.Name = enumMember.Name;
            codeBlock.Type = "EnumMember";
            codeBlock.Code = node.ToFullString();

            this._CodeRepository.CreateCodeBlock(codeBlock);
            #endregion

            foreach (var attr in node.AttributeLists)
            {
                CodeResolver.FindVisitorForNode(enumMember.Id, model, attr);
            }
        }        


    }
}
