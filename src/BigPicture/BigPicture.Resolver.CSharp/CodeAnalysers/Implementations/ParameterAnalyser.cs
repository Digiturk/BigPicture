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
    public class ParameterAnalyser : IAnalyser<ParameterSyntax>
    {
        private IRepository _Repository { get; set; }

        public ParameterAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, ParameterSyntax node, SemanticModel model)
        {
            var prm = new Parameter();

            prm.Name = node.Identifier.Text;
            prm.Id = this._Repository.CreateNode(prm, "Parameter");
            this._Repository.CreateRelationship(parentId, prm.Id, "EXPECTS");

            var symbolInfo = model.GetSymbolInfo(node.Type);
            var typeId = CodeResolver.FindOrCreateType(symbolInfo.Symbol);
            if(String.IsNullOrEmpty(typeId) == false)
            {
                this._Repository.CreateRelationship(prm.Id, typeId, "TYPEOF");
            }
            
        }        
    }
}
