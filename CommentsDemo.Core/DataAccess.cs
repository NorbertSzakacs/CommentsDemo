using System;
using System.Collections.Generic;
using System.Linq;

using CommentsDemo.Common;

namespace CommentsDemo.Core
{
    public class DataAccess
    {
        public DataAccess()
        {
            this.demoData = new Dictionary<string, ProductDTO>();
        }

        public void FillWithDemoData(long productNumber, long commentsPerProductNumber)
        {
            for (int i = 0; i < productNumber; i++)
            {
                LinkedList<CommentDTO> comments = new LinkedList<CommentDTO>();
                for (int commentLoop = 0; commentLoop < commentsPerProductNumber; commentLoop++)
                {
                    // Lists that contain reference types perform better when a node and its value are created at the same time
                    comments.AddFirst(
                        new CommentDTO
                        {
                            Content = masterCommentSample,
                            CreatedAt = DateTime.Now.ToUniversalTime()
                        });
                }

                demoData.Add($"BestProductNamedAs{i}",
                    new ProductDTO
                    {
                        ProductName = $"BestProductNamedAs{i}",
                        Comments = comments
                    });
            }
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            return demoData.Select(p => p.Value);
        }

        /// <summary>
        /// Adds a new comment to the given product.
        /// New product will be created in case of a new product identifier.
        /// </summary>
        /// <param name="productNameIn">Identifier of the product</param>
        /// <param name="comment">New comment</param>
        /// <returns>True if it is a new product otherwise false.</returns>
        public bool AddComment(string productNameIn, string comment)
        {
            if (demoData.TryGetValue(productNameIn, out ProductDTO existingProduct))
            {
                existingProduct.Comments.AddFirst(CreateCommentDTO(comment));

                return false;
            }

            LinkedList<CommentDTO> comments = new LinkedList<CommentDTO>();
            comments.AddFirst(CreateCommentDTO(comment));

            demoData.Add(productNameIn,
                new ProductDTO()
                {
                    ProductName = productNameIn,
                    Comments = comments
                });

            return true;
        }

        private static CommentDTO CreateCommentDTO(string comment)
        {
            return new CommentDTO { Content = comment, CreatedAt = DateTime.Now.ToUniversalTime() };
        }

        //private const int commentMaximumLength = 500;
        //private static readonly Random random = new Random();
        //private static string RandomString()
        //{
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_!?:)( @&=";
        //    return new string(Enumerable.Repeat(chars, commentMaximumLength)
        //      .Select(s => s[random.Next(s.Length)]).ToArray());
        //}

        private static string masterCommentSample =
            $"=86UH2 KBKTI@WE0 2M3UIUFNN13N)UBQI_CKXGO6!PUR7BHUA5@@5Z_N1H:BM G9S!86CQSV&@3:" +
            $"A8ZLTJNIXD42=93PMNT)I4&(NUR4MV=8 @M:111D2@MOHF7N6DJ4X84Q93&=?2 BE(5BW_845QLLR" +
            $"48H7E@JMOLIN E@32KABLJLT5! OY=HWS&E(_L7468_4X8Z6_BCFLJGUW7BNB)7OE6)&=C?QXPPWT" +
            $"1  LKYX BKJ?4SDJU3!AV0_:168I62 )3B(!FC!QKSCV5GP39GC5XW@BFC3X_OKTOPI2XLES?V9DX" +
            $"PIVRETZ0KQHE:5CSNZ H3HQ@6FXC3:C6:X_)HF5)5:D&BY30C=Y(:()H?Y5=(X RNVKQ)ILI)BW_E" +
            $"R(IV@=3PH?&33B3U6&I 650NN5)8PB!SECU2WY&XMBG2M7LG28ICO:S6P)HLF)=GBZY01A@I=434Z" +
            $"&B0U8G1:8DL48M1Q7RRCA16I4Z1_=C3J7HR:1!";

        private Dictionary<string, ProductDTO> demoData = new Dictionary<string, ProductDTO>();
    }
}
