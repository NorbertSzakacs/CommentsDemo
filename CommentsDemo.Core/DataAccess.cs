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
            this.demoData = new List<ProductDTO>();
        }

        public void FillWithDemoData(long productNumber, long commentsPerProductNumber)
        {
            for (int i = 0; i < productNumber; i++)
            {
                demoData.Add(new ProductDTO 
                {
                    ProductName = $"BestProductNamedAs{i}", 
                    Comments = GenerateComments(commentsPerProductNumber).ToList() }
                );                
            }
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            return demoData;
        }

        private IEnumerable<CommentDTO> GenerateComments(long requiredNumber = 2)
        {
            // TODO test only
           //string generatedComment = RandomString();           

            for (int commentLoop = 0; commentLoop < requiredNumber; commentLoop++)
            {
                yield return new CommentDTO { Content = masterCommentSample, CreatedAt = DateTime.Now.ToUniversalTime() };
            }
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
        
        private List<ProductDTO> demoData = new List<ProductDTO>();
    }
}
