using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
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
        _depositServiceUrl = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ? configuration["ApiInternal:LinuxDeposit"]!
            : configuration["ApiInternal:Deposit"]!;
    }
    public async Task<bool> ScheduleProc(PostScheduleProcRequest request)
    {
        HttpClient client = CreateHttpClient();
        HttpContent content = CreateHttpContent(request);
        
        string requestUrl = _depositServiceUrl
            .AppendPathSegment("Transaction")
            .AppendPathSegment("scheduleproc");

        var response = await client.PostAsync(requestUrl, content);
        return await HandleResponse(response);
    }

    private HttpClient CreateHttpClient()
    {
        var clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
        return new HttpClient(clientHandler);
    }
    
    private HttpContent CreateHttpContent(PostScheduleProcRequest request)
    {
        var jsonString = JsonSerializer.Serialize(request);
        return new StringContent(jsonString, Encoding.UTF8, "application/json");
    }

    private async Task<bool> HandleResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Error: {response.StatusCode}");
        }

        var result = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<bool>(result);
    }
    
    public Task<string> DepVersion()
    {
        return _depositServiceUrl
            .AppendPathSegment("Version")
            .GetStringAsync();
    }
    
}