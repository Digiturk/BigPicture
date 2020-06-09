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
    public class MethodDeclarationAnalyser : IAnalyser<MethodDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }
        private ICodeRepository _CodeRepository { get; set; }

        public MethodDeclarationAnalyser(IRepository repository, ICodeRepository codeRepository)
        {
            this._Repository = repository;
            this._CodeRepository = codeRepository;
        }

        public void Analyse(string parentId, MethodDeclarationSyntax node, SemanticModel model)
        {
            var owner = this._Repository.FindNode<Nodes.Type>(parentId);

            var method = new Method();

            method.Assembly = owner.Assembly;
            method.NameSpace = owner.NameSpace;
            method.OwnerName = owner.Name;
            method.Name = node.Identifier.Text;
            method.Modifier = String.Join(", ", node.Modifiers.Select(a => a.Text));
            method.HasBody = node.Body != null;

            method.Id = this._Repository.FindIdOrCreate(method, "Method", new
            {
                Assembly = method.Assembly,
                NameSpace = method.NameSpace,
                OwnerName = method.OwnerName,
                Name = method.Name
            });
            this._Repository.CreateRelationship(parentId, method.Id, "HAS");

            #region CodeRepository
            // Store code on document database (Elastic search)
            var codeBlock = new CodeBlock();
            codeBlock.Id = method.Id;
            codeBlock.Language = "csharp";
            codeBlock.Name = method.Name;
            codeBlock.Type = "Method";
            codeBlock.Code = node.ToFullString();

            this._CodeRepository.CreateCodeBlock(codeBlock);
            #endregion

            var returnSymbol = model.GetSymbolInfo(node.ReturnType);
            var returnTypeId = CodeResolver.FindOrCreateType(returnSymbol.Symbol)?.Id;
            if(String.IsNullOrEmpty(returnTypeId) == false)
            {
                this._Repository.CreateRelationship(method.Id, returnTypeId, "TYPEOF");
            }

            foreach(var prmNode in node.ParameterList.ChildNodes())
            {
                CodeResolver.FindVisitorForNode(method.Id, model, prmNode);
            }

            foreach(var attr in node.AttributeLists)
            {
                CodeResolver.FindVisitorForNode(method.Id, model, attr);
            }

            if(node.Body != null)
            {
                CodeResolver.FindVisitorForNode(method.Id, model, node.Body);
            }
        }        
    }
}
