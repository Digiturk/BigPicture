using BigPicture.Core.Repository;
using BigPicture.Resolver.CSharp.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp.CodeAnalysers.Implementations
{
    public class UsingDirectiveAnalyser : IAnalyser<UsingDirectiveSyntax>
    {
        private IRepository _Repository { get; set; }

        public UsingDirectiveAnalyser(IRepository repository)
        {
            this._Repository = repository;
        }

        public void Analyse(string parentId, UsingDirectiveSyntax node, SemanticModel model)
        {
            var ns = new NameSpace();

            var child = node.ChildNodes().First();
            ns.Text = child.GetText().ToString();

            ns.Id = this._Repository.FindIdOrCreate(ns, "NameSpace", new { Text = ns.Text });
            this._Repository.CreateRelationship(parentId, ns.Id, "USING");
        }
    }
}
