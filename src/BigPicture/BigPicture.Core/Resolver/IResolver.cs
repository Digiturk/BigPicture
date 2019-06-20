using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core.Resolver
{
    public interface IResolver<T> where T : IEntity
    {
        void Resolve(T obj);
    }
}
