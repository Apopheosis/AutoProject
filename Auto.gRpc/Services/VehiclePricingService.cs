using Grpc.Core;
using Microsoft.JSInterop;

namespace Auto.gRpc.Services;

public class PricerService : Pricer.PricerBase
{
    private readonly ILogger<PricerService> logger;

    public PricerService(ILogger<PricerService> ilogger)
    {
        this.logger = logger;
    }

    public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context)
    {
        return Task.FromResult(new PriceReply() { CurrencyCode = "RUB", Price = 400 });
    }
}
