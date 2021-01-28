using Microsoft.AspNetCore.Mvc;

namespace CommentsDemo
{
    public class CommentRequest
    {
        [FromQuery(Name = "productname")]
        public string ProductName { get; set; }

        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = 5;

        [FromQuery(Name = "offset")]
        public int Offset { get; set; } = 0;
    }
}
