using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core.Repository
{
    public interface IRepository
    {
        string TestConnection();

        string CreateNode(String nodeType, object data);
    }
}