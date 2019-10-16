using BigPicture.Core;
using BigPicture.Core.IOC;
using BigPicture.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace BigPicture.ApiOld.Controllers
{
    [RoutePrefix("api/node")]
    public class NodeController : ApiController
    {
        private readonly IGraphRepository _Repository;

        public NodeController()
        {
            this._Repository = Container.Resolve<IGraphRepository>();
        }

        [Route("{id}")]
        [HttpGet]
        public JsonResult<dynamic> Get(String id)
        {
            var result = _Repository.GetNodeById(id);
            return Json(result);
        }

        [Route("search/{term}/{limit?}/{skip?}")]
        [HttpGet]
        public JsonResult<IEnumerable<Node>> Search(string term, int limit = 5, int skip = 0)
        {
            var result = _Repository.FindNodesByName(term, limit, skip);
            return Json(result);
        }
    }
}
