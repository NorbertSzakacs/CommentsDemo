using System;
using System.Collections.Generic;
using System.Linq;

using CommentsDemo.Common;
using CommentsDemo.Core.AzureModel;

namespace CommentsDemo.Core
{
    public class DataAccess
    {
        public DataAccess()
        {
            this.azureAccess = new AzureTableAccess();
        }

        public List<string> FillWithDemoData(long productNumber, long commentsPerProductNumber)
        {
            List<string> productNames = new List<string>();

            for (int productLoop = 0; productLoop < productNumber; productLoop++)
            {
                string productName = $"BestProductNamedAs{random.Next(10000)}";
                productNames.Add(productName);
                for (int commentLoop = 0; commentLoop < commentsPerProductNumber; commentLoop++)
                {
                    AddComment(productName, GenerateSampleComment());
                }
            }

            return productNames;
        }

        public IEnumerable<string> GetProductNames()
        {
            return this.azureAccess.RetrieveProductListAsync(tableName).Result;
        }

        /// <summary>
        /// Returns with every information about the requested product.
        /// </summary>
        /// <param name="productName">Identifier of the product</param>
        /// <returns><see cref="ProductDTO"/> if the requested product was found, otherwise <see cref="null"/>.</returns>
        public ProductDTO GetProduct(string productNameIn)
        {
            IEnumerable<CommentEntity> retrievedComments = this.azureAccess.RetrieveCommentsAsync(tableName, productNameIn).Result;
            if (!retrievedComments.Any())
            {
                return null;
            }

            List<CommentDTO> commentsInProduct = new List<CommentDTO>();

            foreach (CommentEntity comment in retrievedComments)
            {
                commentsInProduct.Add(CreateCommentDTO(comment.Comment, comment.RowKey));
            }

            return new ProductDTO() { ProductName = productNameIn, Comments = commentsInProduct };
        }

        /// <summary>
        /// Adds a new comment to the given product.
        /// New product will be created in case of a new product identifier.
        /// </summary>
        /// <param name="productNameIn">Identifier of the product</param>
        /// <param name="comment">New comment</param>
        public void AddComment(string productNameIn, string comment)
        {
            CommentEntity result = this.azureAccess.InsertCommentAsync(tableName, productNameIn, comment).Result;
        }

        private static CommentDTO CreateCommentDTO(string comment, string createdAt)
        {
            return new CommentDTO { Content = comment, CreatedAt = DateTimeUtil.GetOriginalDateTime(createdAt) };
        }

        private const int commentMaximumLength = 500;
        private static readonly Random random = new Random();

        private static string GenerateSampleComment()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_!?:)( @&=";
            return new string(Enumerable.Repeat(chars, random.Next(1, commentMaximumLength))
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string masterCommentSample =
            $"=86UH2 KBKTI@WE0 2M3UIUFNN13N)UBQI_CKXGO6!PUR7BHUA5@@5Z_N1H:BM G9S!86CQSV&@3:" +
            $"A8ZLTJNIXD42=93PMNT)I4&(NUR4MV=8 @M:111D2@MOHF7N6DJ4X84Q93&=?2 BE(5BW_845QLLR" +
            $"48H7E@JMOLIN E@32KABLJLT5! OY=HWS&E(_L7468_4X8Z6_BCFLJGUW7BNB)7OE6)&=C?QXPPWT" +
            $"1  LKYX BKJ?4SDJU3!AV0_:168I62 )3B(!FC!QKSCV5GP39GC5XW@BFC3X_OKTOPI2XLES?V9DX" +
            $"PIVRETZ0KQHE:5CSNZ H3HQ@6FXC3:C6:X_)HF5)5:D&BY30C=Y(:()H?Y5=(X RNVKQ)ILI)BW_E" +
            $"R(IV@=3PH?&33B3U6&I 650NN5)8PB!SECU2WY&XMBG2M7LG28ICO:S6P)HLF)=GBZY01A@I=434Z" +
            $"&B0U8G1:8DL48M1Q7RRCA16I4Z1_=C3J7HR:1!";

        private readonly IAzureTableAccess azureAccess;
        private const string tableName = "commentDemoTable0";
    }
}
