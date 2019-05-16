using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp.CodeAnalysers
{
    public interface IAnalyser<T> where T : SyntaxNode
    {
        void Analyse(String parentId, T node, SemanticModel model);
    }
}
