using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core
{
    public interface IRelation : IObject
    {
        String Name { get; set; }
    }
}
