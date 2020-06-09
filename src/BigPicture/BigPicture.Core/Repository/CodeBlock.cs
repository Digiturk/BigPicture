using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Core.Repository
{
    public class CodeBlock
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Type { get; set; }
        public String Code { get; set; }
        public String Path { get; set; }
        public String Language { get; set; }
    }
}
