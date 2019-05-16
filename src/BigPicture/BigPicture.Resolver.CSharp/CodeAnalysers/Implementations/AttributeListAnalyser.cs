using BigPicture.Resolver.CSharp.Resolvers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BigPicture.Resolver.CSharp.CodeAnalysers.Implementations
{
    public class AttributeListAnalyser : IAnalyser<AttributeListSyntax>
    {
        public AttributeListAnalyser()
        {
        }

        public void Analyse(string parentId, AttributeListSyntax node, SemanticModel model)
        {
            foreach (var attr in node.Attributes)
            {
                CodeResolver.FindVisitorForNode(parentId, model, attr);
            }
        }
    }
}
