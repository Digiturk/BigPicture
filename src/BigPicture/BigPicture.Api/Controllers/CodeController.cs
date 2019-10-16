using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BigPicture.Core;
using BigPicture.Core.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BigPicture.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeController : ControllerBase
    {
        private readonly IDocumentRepository _DocumentRepository;

        public CodeController(IDocumentRepository documentRepository)
        {
            this._DocumentRepository = documentRepository;            
        }

        [HttpGet("{id}")]
        public ActionResult<CodeBlock> GetCode(String id)
        {
            var codeBlock = this._DocumentRepository.GetCodeBlock(id);
            return Ok(codeBlock);
        }                
    }
}