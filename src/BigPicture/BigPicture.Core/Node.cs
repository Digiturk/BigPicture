using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core
{
    public class Node : Entity
    {
        public String Name { get; set; }
        public List<String> Labels { get; set; }
    }
}
