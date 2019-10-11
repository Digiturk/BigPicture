using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core.Resolver
{
    public interface IResolver<T> where T : Entity
    {
        void Resolve(T obj);
    }
}
