using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp.CodeAnalysers
{
    public class MySyntaxWalker : CSharpSyntaxWalker
    {
        private SemanticModel _Model { get; set; }
        private List<MemberAccess> _MemberAccessList = new List<MemberAccess>();

        public MySyntaxWalker(SemanticModel model)
        {
            this._Model = model;
        }

        public List<MemberAccess> GetMemberAccesses()
        {
            return this._MemberAccessList;
        }

        public override void VisitConstructorInitializer(ConstructorInitializerSyntax node)
        {

        }

        //public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        //{
        //    var symbol = this._Model.GetSymbolInfo(node.Expression);
        //    if(symbol.)

        //    if (symbol.Symbol.OriginalDefinition.GetType() == typeof(INamedTypeSymbol))
        //    {
        //        var assembly = symbol.Symbol.OriginalDefinition.ContainingAssembly.Identity.Name;
        //        var ns = symbol.Symbol.OriginalDefinition.ContainingNamespace;
        //        var cs = symbol.Symbol.OriginalDefinition.Name;
        //    }
        //    else
        //    {
        //        var assembly = symbol.Symbol.OriginalDefinition.ContainingAssembly.Identity.Name;
        //        var ns = symbol.Symbol.OriginalDefinition.ContainingNamespace;
        //        var cs = symbol.Symbol.OriginalDefinition.ContainingType.Name;
        //        var methodName = symbol.Symbol.OriginalDefinition.Name;
        //    }
        //}

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {

        }

        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            var symbolInfo = this._Model.GetSymbolInfo(node);

            if (symbolInfo.Symbol == null)
            {
                return;
            }

            var memberAccess = new MemberAccess();
            memberAccess.Assembly = symbolInfo.Symbol.OriginalDefinition.ContainingAssembly.Identity.Name;
            memberAccess.NameSpace = symbolInfo.Symbol.OriginalDefinition.ContainingNamespace.ToString();
            memberAccess.TypeName = symbolInfo.Symbol.OriginalDefinition.ContainingType.Name;
            memberAccess.Name = symbolInfo.Symbol.OriginalDefinition.Name;
            memberAccess.Kind = symbolInfo.Symbol.OriginalDefinition.Kind.ToString();

            if(node.Parent.IsKind(SyntaxKind.InvocationExpression))
            {
                var parent = node.Parent as InvocationExpressionSyntax;
                var i = 0;
                foreach(var arg in parent.ArgumentList.Arguments)
                {
                    if (typeof(IMethodSymbol).IsInstanceOfType(symbolInfo.Symbol.OriginalDefinition)) {
                        var prms = (symbolInfo.Symbol.OriginalDefinition as IMethodSymbol).Parameters;
                        IParameterSymbol prmSymbol;
                        if (i >= prms.Length && prms.Length > 0)
                        {
                            prmSymbol = prms.Last();
                        }
                        else
                        {
                            prmSymbol = prms[i];
                        }

                        var prmName = prmSymbol?.Name?.ToString();

                        var value = this._Model.GetConstantValue(arg.Expression);

                        if (value.HasValue)
                        {

                        }
                    }

                    i++;
                }
            }

            if(! _MemberAccessList.Exists(a => 
                a.Assembly == memberAccess.Assembly && 
                a.Name == memberAccess.Name && 
                a.NameSpace == memberAccess.NameSpace &&
                a.TypeName == memberAccess.TypeName))
            {
                this._MemberAccessList.Add(memberAccess);
            }            
        }
    }

    public struct MemberAccess
    {
        public String Assembly { get; set; }
        public String NameSpace { get; set; }
        public String TypeName { get; set; }
        public String Name { get; set; }
        public String Kind { get; set; }
    }
}
