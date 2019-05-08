using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core.Resolver
{
    public interface IResolveEngine
    {
        void LoadStartData();
        void StartResolvers();
    }
}
