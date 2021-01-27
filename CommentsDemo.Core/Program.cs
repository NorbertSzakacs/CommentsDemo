using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using CommentsDemo.Common;

namespace CommentsDemo.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello CommentsDemo console user!");
            DataAccess dataAccess = new DataAccess();

            int productCount = 2;
            List<string> productNames = dataAccess.FillWithDemoData(productCount, 3);

            ProductDTO[] result = new ProductDTO[productCount];

            int productLoop = 0;
            foreach (string productName in productNames)
            {
                // result.
                result[productLoop] = dataAccess.GetProduct(productName);
                productLoop++;
            }

            //ProductDTO[] result = dataAccess.GetProducts().ToArray();

            foreach (ProductDTO item in result)
            {
                string jsonString = JsonSerializer.Serialize(item);
                Console.WriteLine($"Product information:");
                Console.Write($"{jsonString}");
            }
        }    
    }
}
