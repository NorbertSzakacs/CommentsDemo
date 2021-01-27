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
        List<CommentEntity> RetrieveCommentsSimple(string tableName, string productName);
    }
}
