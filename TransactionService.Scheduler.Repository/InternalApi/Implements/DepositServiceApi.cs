using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using TransactionService.Scheduler.Repository.InternalApi.Interfaces;
using TransactionService.Scheduler.Services.Model;

namespace TransactionService.Scheduler.Repository.InternalApi.Implements;

public class DepositServiceApi : IDepositServiceApi
{
    private readonly string _depositServiceUrl;

    public DepositServiceApi(IConfiguration configuration)
    {
        _depositServiceUrl = configuration["ApiInternal:Deposit"] ?? "https://localhost:7201";
    }


    public async Task<bool> ScheduleProc(PostScheduleProcRequest request)
    {
        // public async Task<string> GetVersion() => await _depositServiceUrl
        //     .AppendPathSegment("version")
        //     .GetStringAsync();
        return await _depositServiceUrl.AppendPathSegment("/Transaction/scheduleproc")
            .PostJsonAsync(request)
            // .ReceiveString();
        .ReceiveJson<bool>();
    }
}