using Compare_Files_Aws_Azure.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Compare_Files_Aws_Azure
{
    class Program
    {
        static void Main(string[] args)
        {
            // args "bucket-name" "container-name" "storageaccount-name" "storageaccount-key"

            Console.WriteLine("Listing AWS");
            var awsListTask = AmazonS3.ListObjectsAsync(args[0]);
            Console.WriteLine("Listing Azure");
            var azureListTask = AzureContainer.ListAsync(args[1], args[2], args[3]);
            Console.WriteLine("Awaiting both");
            Task.WhenAll(awsListTask, azureListTask);
            Console.WriteLine("Comparing");
            Compare(awsListTask.Result, azureListTask.Result);
            File.AppendAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\aws.txt", awsListTask.Result);
            File.AppendAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\az.txt", azureListTask.Result);
            Console.WriteLine("Done");
        }

        public static void Compare(List<string> awsList, List<string> azList)
        {
            var diff = awsList.Except(azList).Distinct().ToArray();
            Console.WriteLine($"Missing on Azure: {diff.Count()}");
            foreach (var item in diff)
            {
                Console.WriteLine($"Diff: {item}");
            }

            diff = azList.Except(awsList).Distinct().ToArray();
            Console.WriteLine($"Missing on AWS: {diff.Count()}");
            foreach (var item in diff)
            {
                Console.WriteLine($"Diff: {item}");
            }
        }
    }
}
