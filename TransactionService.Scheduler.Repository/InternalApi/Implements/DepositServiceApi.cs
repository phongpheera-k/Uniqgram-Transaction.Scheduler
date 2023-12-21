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
        return await _depositServiceUrl
            .AppendPathSegment("Transaction")
            .AppendPathSegment("scheduleproc")
            .PostJsonAsync(request)
            // .ReceiveString();
        .ReceiveJson<bool>();
    }

    public Task<string> DepVersion()
    {
        return _depositServiceUrl
            .AppendPathSegment("Version")
            .GetStringAsync();
    }
    
//     try
//     {
//         var response = await "https://api.example.com"
//             .AppendPathSegment("data")
//             .WithOAuthBearerToken("your_access_token")
//             .GetAsync();
//
//         if (response.IsSuccessStatusCode)
//         {
//             string responseBody = await response.Content.ReadAsStringAsync();
//             Console.WriteLine(responseBody);
//         }
//         else
//         {
//             Console.WriteLine($"Request failed: {response.StatusCode}");
//         }
//     }
//     catch (FlurlHttpException ex)
// {
//     Console.WriteLine($"Flurl.Http exception: {ex.Message}");
// }
    
}