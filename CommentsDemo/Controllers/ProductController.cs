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

        [HttpGet]
        [Route("ProductNames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<string>> GetProductNames()
        {
            IEnumerable<string> result = this.dataAccess.GetProductNames();

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

        //GET Product/Comments?productname=<value>&limit=<value>&offset=<value>"
        [HttpGet]
        [Route("Comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProductDTO>> GetCommentFragments([FromQuery] CommentRequest request)
        {
            ProductDTO result = this.dataAccess.GetProduct(request.ProductName);

            return Ok(result.Comments.Skip(request.Offset).Take(request.Limit));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<string> PostComment([FromBody] InsertCommentDTO insertCommentDTO)
        {
            this.dataAccess.AddComment(insertCommentDTO.ProductName, insertCommentDTO.CommentContent);

            return new CreatedResult($"/product/{insertCommentDTO.ProductName}", insertCommentDTO.ProductName);
        }

        [HttpPost]
        [Route("NewComment")]
        [ProducesResponseType(StatusCodes.Status201Created)]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> PostNewestComment([FromBody] InsertCommentDTO insertCommentDTO)
        {
            InsertResultEnum resultEnum = this.dataAccess.AddCommentIfPermitted(insertCommentDTO);

            switch (resultEnum)
            {
                case InsertResultEnum.Success:
                    return new CreatedResult($"/product/{insertCommentDTO.ProductName}", insertCommentDTO.ProductName);                    
                case InsertResultEnum.NotPermitted:
                    return ValidationProblem("The latest comment id is not valid!");
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private readonly DataAccess dataAccess;
        private readonly ILogger<ProductController> logger;
    }
}
