using BigPicture.Core.Repository;
using BigPicture.Resolver.CSharp.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace BigPicture.Resolver.CSharp.CodeAnalysers.Implementations
{
    public class AttributeAnalyser : IAnalyser<AttributeSyntax>
    {
        private IRepository _Repository { get; set; }
        private ICodeRepository _CodeRepository { get; set; }

        public AttributeAnalyser(IRepository repository, ICodeRepository codeRepository)
        {
            this._Repository = repository;
            this._CodeRepository = codeRepository;
        }

        public void Analyse(string parentId, AttributeSyntax node, SemanticModel model)
        {
            SymbolInfo nameInfo = model.GetSymbolInfo(node);
            if(nameInfo.Symbol == null)
            {
                nameInfo = model.GetSymbolInfo(node.Name);                
            }

            if(nameInfo.Symbol == null)
            {
                // TODO log
                return;
            }

            var cls = new Class();
            cls.Name = nameInfo.Symbol.ContainingType.Name;
            cls.Assembly = nameInfo.Symbol.ContainingAssembly.Identity.Name;
            cls.NameSpace = nameInfo.Symbol.ContainingNamespace.ToString();

            cls.Id = this._Repository.FindIdOrCreate(cls, "Type", new { Name = cls.Name, NameSpace = cls.NameSpace, Assembly = cls.Assembly });
            this._Repository.CreateRelationship(parentId, cls.Id, "HAS");

            #region CodeRepository
            // Store code on document database (Elastic search)
            var codeBlock = new CodeBlock();
            codeBlock.Id = cls.Id;
            codeBlock.Language = "csharp";
            codeBlock.Name = cls.Name;
            codeBlock.Type = "Attribute";
            codeBlock.Code = node.ToFullString();

            this._CodeRepository.CreateCodeBlock(codeBlock);
            #endregion
        }
    }
}
