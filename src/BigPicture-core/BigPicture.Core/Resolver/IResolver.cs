using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core.Resolver
{
    public interface IResolver<T> where T : INode
    {
        void Resolve(T obj);
    }
}
