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
    public class ClassDeclarationAnalyser : IAnalyser<ClassDeclarationSyntax>
    {
        private IRepository _Repository { get; set; }
        private ICodeRepository _CodeRepository { get; set; }

        public ClassDeclarationAnalyser(IRepository repository, ICodeRepository codeRepository)
        {
            this._Repository = repository;
            this._CodeRepository = codeRepository;
        }

        public void Analyse(string parentId, ClassDeclarationSyntax node, SemanticModel model)
        {
            var cls = new Class();
            var symbol = model.GetDeclaredSymbol(node);

            cls.Name = node.Identifier.Text;
            cls.Assembly = symbol.ContainingAssembly.Identity.Name;
            cls.NameSpace = symbol.ContainingNamespace.ToString();
            cls.Modifier = String.Join(", ", node.Modifiers.Select(a => a.Text));
            cls.IsAbstract = symbol.IsAbstract;
            cls.IsSealed = symbol.IsSealed;
            cls.IsStatic = symbol.IsStatic;                       

            cls.Id = this._Repository.FindIdOrCreate(cls, "Class", new { Name = cls.Name, NameSpace = cls.NameSpace, Assembly = cls.Assembly });
            this._Repository.UpdateNode<Class>(cls, "Type");
            this._Repository.CreateRelationship(parentId, cls.Id, "CONTAINS");

            #region CodeRepository
            // Store code on document database (Elastic search)
            var codeBlock = new CodeBlock();
            codeBlock.Id = cls.Id;
            codeBlock.Language = "csharp";
            codeBlock.Name = cls.Name;
            codeBlock.Type = "Class";
            codeBlock.Code = node.ToFullString();

            this._CodeRepository.CreateCodeBlock(codeBlock);
            #endregion

            if (node.BaseList != null)
            {
                CodeResolver.FindVisitorForNode(cls.Id, model, node.BaseList);                
            }

            // TODO parse attibutes
            foreach (var attrList in node.AttributeLists)
            {
                CodeResolver.FindVisitorForNode(cls.Id, model, attrList);
            }            

            foreach (var member in node.Members)
            {
                CodeResolver.FindVisitorForNode(cls.Id, model, member);
            }

            // TODO parse members
        }        
    }
}
