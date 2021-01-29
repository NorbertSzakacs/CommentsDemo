using System.Collections.Generic;
using System.Threading.Tasks;

using CommentsDemo.Core.AzureModel;

using Microsoft.Azure.Cosmos.Table;

namespace CommentsDemo.Core
{
    interface IAzureTableAccess
    {
        Task<CloudTable> CreateTableAsync(string tableName);

        Task<CommentEntity> InsertCommentAsync(string tableName, string productName, string comment);

        // Partition scan
        Task<IEnumerable<CommentEntity>> RetrieveCommentsAsync(string tableName, string productName);

        Task<string> RetrieveLatestCommentAsync(string tableName, string productName);

        Task<long> RetrieveCommentCountAsync(string tableName, string productName);

        // Table scan
        Task<IEnumerable<string>> RetrieveProductListAsync(string tableName); 
    }
}
