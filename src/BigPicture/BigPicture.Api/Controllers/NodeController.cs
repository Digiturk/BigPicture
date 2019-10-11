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
    public class NodeController : ControllerBase
    {
        private readonly IGraphRepository _Repository;

        public NodeController(IGraphRepository repository)
        {
            this._Repository = repository;
        }

        [HttpGet("{id}")]
        public ActionResult<Node> Get(String id)
        {
            var result = _Repository.GetNodeById(id);
            return Ok(result);
        }

        [HttpGet("search/{term}/{limit?}/{skip?}")]
        public ActionResult<IEnumerable<Node>> Search(string term, int limit = 5, int skip = 0)
        {
            var result = _Repository.FindNodesByName(term, limit, skip);
            return Ok(result);
        }
    }
}