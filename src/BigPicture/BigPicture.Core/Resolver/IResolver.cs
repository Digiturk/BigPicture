﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core.Resolver
{
    public interface IResolver<T>
    {
        void Resolve();
    }
}
