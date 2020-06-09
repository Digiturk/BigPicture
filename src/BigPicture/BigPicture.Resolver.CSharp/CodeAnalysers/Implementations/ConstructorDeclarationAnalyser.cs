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
    public class ConstructorDeclarationAnalyser : IAnalyser<ConstructorDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }
        private ICodeRepository _CodeRepository { get; set; }

        public ConstructorDeclarationAnalyser(IRepository repository, ICodeRepository codeRepository)
        {
            this._Repository = repository;
            this._CodeRepository = codeRepository;
        }

        public void Analyse(string parentId, ConstructorDeclarationSyntax node, SemanticModel model)
        {
            var method = new Constructor();

            method.Name = node.Identifier.Text;
            method.Modifier = String.Join(", ", node.Modifiers.Select(a => a.Text));

            method.Id = this._Repository.CreateNode(method, "Constructor");
            this._Repository.CreateRelationship(parentId, method.Id, "HAS");

            #region CodeRepository
            // Store code on document database (Elastic search)
            var codeBlock = new CodeBlock();
            codeBlock.Id = method.Id;
            codeBlock.Language = "csharp";
            codeBlock.Name = method.Name;
            codeBlock.Type = "Constructor";
            codeBlock.Code = node.ToFullString();

            this._CodeRepository.CreateCodeBlock(codeBlock);
            #endregion

            foreach (var prmNode in node.ParameterList.ChildNodes())
            {
                CodeResolver.FindVisitorForNode(method.Id, model, prmNode);
            }

            foreach (var attr in node.AttributeLists)
            {
                CodeResolver.FindVisitorForNode(method.Id, model, attr);
            }            

            if (node.Body != null)
            {
                CodeResolver.FindVisitorForNode(method.Id, model, node.Body);
            }
        }        
    }
}
