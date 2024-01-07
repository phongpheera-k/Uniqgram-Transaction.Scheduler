using System.Globalization;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TransactionService.Scheduler.Helper.Expressions;
using TransactionService.Scheduler.Services.Model;
using TransactionService.Scheduler.Services.Services.Interfaces;

namespace TransactionService.Scheduler.Schedulers;

public class TransactionScheduler : IHostedService, IDisposable
{
    private readonly ILogger<TransactionScheduler> _logger;
    private readonly IPreTransactionService _preTransactionService;
    private readonly TimeSpan _scheduleFrequency;
    private Timer? _timer;

    public TransactionScheduler(ILogger<TransactionScheduler> logger, 
        IPreTransactionService preTransactionService, IConfiguration configuration)
    {
        _logger = logger;
        _preTransactionService = preTransactionService;
        _scheduleFrequency = TimeSpanExpressions.ParseToTimeSpan(
            configuration.GetSection("ScheduleFrequency").Value!);
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Transaction scheduler is starting at {DateTime.Now:h:mm:ss tt zz}");
        _timer = new Timer(AsyncWrapper, null, TimeSpan.Zero, _scheduleFrequency);

        return Task.CompletedTask;
    }

    private void AsyncWrapper(object? state) => Task.Run(Execution);

    private async Task Execution()
    {
        // _logger.LogInformation($"Transaction scheduler executing at {DateTime.Now:h:mm:ss tt zz}");
        // _countState = await _mathService.Count(_countState, 2);
        // _logger.LogInformation($"count at {_countState}");
        // var postScheduleProcRequest = new PostScheduleProcRequest() { OperDate = DateTime.Now};
        // var result = await _preTransactionService.CallApiDepositScheduleProc(postScheduleProcRequest);
        
        var testDate = DateTime.ParseExact("06/10/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
        var postScheduleProcRequest = new PostScheduleProcRequest { OperDate = testDate};
        try
        {
            var result = await _preTransactionService.CallApiDepositScheduleProc(postScheduleProcRequest);
            _logger.LogInformation($"[logs]result is {result}");
        }
        catch (Exception ex)
        {
           _logger.LogError($"Flurl.Http exception: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Transaction scheduler is stopping at {DateTime.Now:h:mm:ss tt zz}");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}