using CommentsDemo.Common;
using CommentsDemo.Core;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;

namespace CommentsDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        public ProductController(DataAccess dataAccessIn, ILogger<ProductController> loggerIn)
        {
            this.logger = loggerIn;
            this.dataAccess = dataAccessIn;

            if (dataAccess.GetProducts().Any())
            {
                return;
            }

            this.dataAccess.FillWithDemoData(5, 5);
        }

        // GET /Product
        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProductDTO>> Get()
        {          
            ProductDTO[] result = this.dataAccess.GetProducts().ToArray();

            logger.LogInformation("Product list scan performed.");

            return Ok(result);
        }

        // GET Product/Fragments?limit=<value>&offset=<value>
        [HttpGet]
        [Route("Fragments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProductDTO>> GetProductFragments([FromQuery] ProductRequest request)
        {
            IEnumerable<ProductDTO> result = this.dataAccess.GetProducts();

            return Ok(result.OrderBy(p => p.ProductName).Skip(request.Offset).Take(request.Limit));
        }

        // GET Product/<value>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"value{id}";
        }

        private readonly DataAccess dataAccess;
        private readonly ILogger<ProductController> logger;
    }
}
