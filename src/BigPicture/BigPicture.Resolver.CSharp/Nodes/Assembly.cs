﻿using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Resolver.CSharp.Nodes
{
    public class Assembly : INode
    {
        public String Id { get; set; }

        public String Name { get; set; }        
    }
}