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

        // GET Product/<value>
        [HttpGet("{productName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductDTO> GetProduct(string productName)
        {
            ProductDTO result = this.dataAccess.GetProduct(productName);

            if (result == null)
            {
                return NotFound();
            }

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

        //TODO: legfrissebb vélemény ellenőrzése timestamppel
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<string> PostComment([FromBody] InsertCommentDTO insertCommentDTO)
        {
            this.dataAccess.AddComment(insertCommentDTO.ProductName, insertCommentDTO.CommentContent);

            return new CreatedResult($"/product/{insertCommentDTO.ProductName}", insertCommentDTO.ProductName);
        }

        private readonly DataAccess dataAccess;
        private readonly ILogger<ProductController> logger;
    }
}
