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
    [RoutePrefix("api/code")]
    public class CodeController : ApiController
    {
        private readonly IDocumentRepository _DocumentRepository;

        public CodeController()
        {            
            this._DocumentRepository = Container.Resolve<IDocumentRepository>();
        }

        [Route("{id}")]
        [HttpGet]
        public JsonResult<CodeBlock> Get(String id)
        {
            var codeBlock = this._DocumentRepository.GetCodeBlock(id);
            return Json(codeBlock);
        }
    }
}
