﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core
{
    public interface INode : IObject
    {
        String Id { get; set; }
    }
}
