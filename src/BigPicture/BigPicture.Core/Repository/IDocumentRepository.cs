using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Core.Repository
{
    public interface IDocumentRepository
    {
        CodeBlock GetCodeBlock(String id);
        string CreateCodeBlock(CodeBlock codeBlock);
        void RemoveAllCodeBlocks();
    }
}
