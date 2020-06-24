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

        public static async Task<List<string>> ListingObjectsAsync(string bucketName)
        {
            List<string> Files = new List<string>();            
            client = new AmazonS3Client(bucketRegion);

            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                    MaxKeys = 1000
                };
                ListObjectsV2Response response;
                do
                {
                    response = await client.ListObjectsV2Async(request);
                    Files.AddRange(response.S3Objects.Select(a => a.Key));
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

            return Files;
        }
    }
}
