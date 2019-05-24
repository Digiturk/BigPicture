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
        private Dictionary<string, object> _LocalVariables = new Dictionary<string, object>();
        private Dictionary<String, object> _LocalVariablesCodes = new Dictionary<string, object>();

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

        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            var name = (node.Left as IdentifierNameSyntax)?.Identifier.Text;

            if(String.IsNullOrEmpty(name) == false)
            {
                var value = this._Model.GetConstantValue(node.Right);

                if (value.HasValue)
                {
                    if(this._LocalVariables.ContainsKey(name))
                    {
                        this._LocalVariables[name] = value.Value;
                    }
                    else
                    {
                        this._LocalVariables.Add(name, value.Value);
                    }
                }
                else if(this._LocalVariables.ContainsKey(name))
                {
                    this._LocalVariables.Remove(name);
                }
            }
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            foreach(var v in node.Declaration.Variables)
            {
                var name = v.Identifier.Text;
                if (v.Initializer != null)
                {
                    var val = this._Model.GetConstantValue(v.Initializer.Value);

                    if (val.HasValue)
                    {
                        if(this._LocalVariables.ContainsKey(name))
                        {
                            this._LocalVariables[name] = val.Value;
                        }
                        else
                        {
                            this._LocalVariables.Add(name, val.Value);
                        }
                    }
                    else
                    {
                        var code = v.Initializer.Value.ToFullString().Trim();
                        if (this._LocalVariablesCodes.ContainsKey(name))
                        {
                            this._LocalVariablesCodes[name] = code;
                        }
                        else
                        {
                            this._LocalVariablesCodes.Add(name, code);
                        }
                    }
                }
            }
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
            memberAccess.Code = node.Parent.ToFullString().Trim();

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

                        memberAccess.ParamNames.Add(prmName);
                        if(value.HasValue)
                        {
                            memberAccess.ParamValues.Add(value.Value?.ToString()??"<NULL>");
                        }
                        else if (_LocalVariables.ContainsKey(arg.ToFullString()))
                        {
                            memberAccess.ParamValues.Add(_LocalVariables[arg.ToFullString()]?.ToString()??"<NULL>");
                        }
                        else if(_LocalVariablesCodes.ContainsKey(arg.ToFullString()))
                        {
                            memberAccess.ParamValues.Add(_LocalVariablesCodes[arg.ToFullString()]?.ToString() ?? "<NULL>");
                        }
                        else
                        {
                            memberAccess.ParamValues.Add("<UNKNOWN>");
                        }
                        memberAccess.ParamCodes.Add(arg.ToFullString());
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

    public class MemberAccess
    {
        public String Assembly { get; set; }
        public String NameSpace { get; set; }
        public String TypeName { get; set; }
        public String Name { get; set; }
        public String Kind { get; set; }
        public String Code { get; set; }
        public List<String> ParamNames { get; } = new List<String>();
        public List<String> ParamValues { get; } = new List<String>();
        public List<String> ParamCodes { get; } = new List<String>();
    }
}
