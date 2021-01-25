using System.Collections.Generic;

namespace CommentsDemo.Common
{
    public class ProductDTO
    {
        public string ProductName { get; set; }

        // O(1) operations: insert, remove, count. Multithreaded scenario supported only for read operations.
        public LinkedList<CommentDTO> Comments { get; set; }
    }
}
