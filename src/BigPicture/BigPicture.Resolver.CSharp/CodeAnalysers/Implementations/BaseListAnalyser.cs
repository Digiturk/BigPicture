using BigPicture.Core.Repository;
using BigPicture.Resolver.CSharp.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp.CodeAnalysers.Implementations
{
    public class BaseListAnalyser : IAnalyser<BaseListSyntax>
    {
        private IRepository _Repository { get; set; }

        public BaseListAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, BaseListSyntax node, SemanticModel model)
        {
            foreach (var baseNode in node.ChildNodes())
            {
                var baseTypeNode = baseNode as SimpleBaseTypeSyntax;
                var symbolInfo = model.GetSymbolInfo(baseTypeNode.Type);
                
                if(symbolInfo.Symbol != null)
                {
                    var symbol = symbolInfo.Symbol as INamedTypeSymbol;

                    if(symbol.TypeKind == TypeKind.Interface)
                    {
                        var interFace = new Interface();
                        interFace.Assembly = symbol.ContainingAssembly.Identity.Name;
                        // TODO Update this later interFace.Modifier = 
                        interFace.Name = symbol.Name;
                        interFace.NameSpace = symbol.ContainingNamespace.ToString();
                        interFace.Id = this._Repository.FindIdOrCreate(interFace, "Interface",
                            new { Name = interFace.Name, NameSpace = interFace.NameSpace, Assembly = interFace.Assembly });

                        this._Repository.CreateRelationship(parentId, interFace.Id, "IMPLEMENTS");
                    }
                    else if(symbol.TypeKind == TypeKind.Class)
                    {
                        var cls = new Class();
                        cls.Assembly = symbol.ContainingAssembly.Identity.Name;
                        // TODO Update this later interFace.Modifier = 
                        cls.Name = symbol.Name;
                        cls.NameSpace = symbol.ContainingNamespace.ToString();
                        cls.Id = this._Repository.FindIdOrCreate(cls, "Class",
                            new { Name = cls.Name, NameSpace = cls.NameSpace, Assembly = cls.Assembly });

                        this._Repository.CreateRelationship(parentId, cls.Id, "EXTENDS");
                    }
                }
            }
        }
    }
}
