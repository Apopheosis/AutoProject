using Auto.gRpc;
using Grpc.Net.Client;
var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

using var channel = GrpcChannel.ForAddress("https://localhost:7231", new GrpcChannelOptions{HttpHandler = httpHandler});
var grpcClient = new Pricer.PricerClient(channel);
Console.WriteLine("Ready! Press any key to send a gRPC request (or Ctrl-c to quit)");
while (true)
{
    Console.ReadKey(true);
    var request = new PriceRequest
    {
        Model = "volkswagen-beetle",
        Color = "Green"
    };
    var reply = grpcClient.GetPrice(request);
    Console.WriteLine($"Price: {reply.Price}");
}