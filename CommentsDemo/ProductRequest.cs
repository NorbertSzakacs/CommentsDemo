using Microsoft.AspNetCore.Mvc;

namespace CommentsDemo
{
    public class ProductRequest
    {
        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = 5;

        [FromQuery(Name = "offset")]
        public int Offset { get; set; }
    }
}
