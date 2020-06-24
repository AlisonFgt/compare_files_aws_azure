using Compare_Files_Aws_Azure.Service;
using System;
using System.Collections.Generic;

namespace Compare_Files_Aws_Azure
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Compare files aws and azure!");
            List<string> result = AmazonS3.ListingObjectsAsync("trial.tradeforce.com.br").Result;
            Console.WriteLine(string.Join("\n", result));
        }
    }
}
