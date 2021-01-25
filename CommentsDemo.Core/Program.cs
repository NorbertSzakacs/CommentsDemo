using System;
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

            dataAccess.FillWithDemoData(5,5);
            ProductDTO[] result = dataAccess.GetProducts().ToArray();

            foreach (ProductDTO item in result)
            {
                string jsonString = JsonSerializer.Serialize(item);
                Console.WriteLine($"Product information:");
                Console.Write($"{jsonString}");
            }
        }
    }
}
