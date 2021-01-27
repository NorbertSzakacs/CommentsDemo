using System;

namespace CommentsDemo.Common
{
    public class CommentsDemoException : Exception
    {
        public CommentsDemoException(string message) : base(message) { }

        public CommentsDemoException(string message, Exception e) : base(message, e) { }
    }
}
