using Microsoft.Azure.Cosmos.Table;

namespace CommentsDemo.Core.AzureModel
{
    public class CommentEntity : TableEntity
    {
        
        public CommentEntity()
        {

        }

        public CommentEntity(string productNameIn, string commentIn)
        {
            // Key (PartitionKey and RowKey) blacklist: / \ # ? \t \n \r and also control character from U+007F to U+009F
            // TODO: This demo neither handle the blacklisted characters nor a too long string (>1kB) input for the productNameIn.
            PartitionKey = productNameIn;
            RowKey = DateTimeUtil.GetInvertedDateTime();
            Comment = commentIn;
        }

        // Property names will be encoded into URL paths. Pay attention to HTTP/1.1       
        public string Comment { get; set; }

    }
}
