using System;
using System.Collections.Generic;
using System.Text;

namespace BigPicture.Core.Repository
{
    public interface IRepository<TModel> where TModel : class
    {
        TModel GetById(string id);
        IList<TModel> List(Func<TModel, bool> query = null);
        void Create(TModel model);
        void Update(TModel model);
        void Delete(TModel model);
        void Delete(string id);
    }
}
