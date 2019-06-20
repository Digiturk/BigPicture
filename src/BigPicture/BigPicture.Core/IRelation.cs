using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core
{
    public interface IRelation : IEntity
    {
        String Name { get; }
    }
}
