using System.Collections.Generic;

namespace CommentsDemo.Common
{
    public class ProductDTO
    {
        public string ProductName { get; set; }

        public List<CommentDTO> Comments { get; set; }
    }
}
