﻿using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp.Nodes
{
    public class Type : Node
    {

        public String Assembly { get; set; }
        public String NameSpace { get; set; }
        public String Modifier { get; set; }
    }
}
