using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compare_Files_Aws_Azure.Service
{
    public class AmazonS3
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.SAEast1; // sa-east-1

        private static IAmazonS3 client;

        public static async Task<List<string>> ListObjectsAsync(string bucketName)
        {
            List<string> Files = new List<string>();
            client = new AmazonS3Client(bucketRegion);

            try
            {
                string[] folders = bucketName.Split("/".ToCharArray());
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = folders[0],
                    MaxKeys = 1000,
                    Prefix = folders.Length == 1 ? "" : folders[1]
                };
                ListObjectsV2Response response;
                do
                {
                    response = await client.ListObjectsV2Async(request);
                    Files.AddRange(
                        response.S3Objects
                        .Select(a =>
                        {
                            string replaceLeadingSlashAndFolder = a.Key.StartsWith("/") ? a.Key.Substring(1, a.Key.Length - 1) : a.Key;
                            if (replaceLeadingSlashAndFolder != request.Prefix + "/")
                                replaceLeadingSlashAndFolder = request.Prefix.Length == 0 ? replaceLeadingSlashAndFolder : replaceLeadingSlashAndFolder.Substring(request.Prefix.Length + 1, replaceLeadingSlashAndFolder.Length - (request.Prefix.Length + 1));
                            return replaceLeadingSlashAndFolder;
                        })
                        .Where(a => !a.EndsWith("/")));
                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine("S3 error occurred. Exception: " + amazonS3Exception.ToString());
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message.ToString());
                Console.ReadKey();
            }
            Console.WriteLine($"AWS completed: {Files.Count()}");
            return Files;
        }
    }
}
