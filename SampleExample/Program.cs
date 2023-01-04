using Discount.Grpc.Protos;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace SampleExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var input = new GetDiscountRequest { Productname = "IPhone X" };
            var channel = GrpcChannel.ForAddress("https://localhost:5003");
            var client = new DiscountProtoService.DiscountProtoServiceClient(channel);
            var reply = await client.GetDiscountAsync(input);
            Console.WriteLine("Hello World!");
        }
    }
}
