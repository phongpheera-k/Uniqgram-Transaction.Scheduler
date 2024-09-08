using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
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
    private readonly bool _useFrequencyNotTime;
    private readonly TimeSpan _scheduleFrequency;
    private readonly DateTime _scheduleTime;
    private Timer? _timer;

    public TransactionScheduler(ILogger<TransactionScheduler> logger, 
        IPreTransactionService preTransactionService, IConfiguration configuration)
    {
        _logger = logger;
        _preTransactionService = preTransactionService;
        _useFrequencyNotTime = bool.Parse(configuration.GetSection("Schedule:UseFrequencyNotTime").Value!);
        _scheduleFrequency = TimeSpanExpressions.ParseToTimeSpan(
            configuration.GetSection("Schedule:Frequency").Value!);
        _scheduleTime = DateTime.ParseExact(configuration.GetSection("Schedule:Time").Value!, "HH:mm", 
            CultureInfo.CurrentCulture);
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Transaction scheduler service is starting at {DateTime.Now:h:mm:ss tt zz}");
        _timer = SetTimer(AsyncWrapper, null);

        return Task.CompletedTask;
    }
    
    private Timer SetTimer(Action action, object? state)
    {
        if (_useFrequencyNotTime)
            return new Timer(_ => action(), state, TimeSpan.Zero, _scheduleFrequency);

        var scheduleTime = _scheduleTime.TimeOfDay;
        var currentTime = DateTime.Now.TimeOfDay;
        var timeToGo = scheduleTime > currentTime ? 
            scheduleTime - currentTime : 
            scheduleTime - currentTime + TimeSpan.FromDays(1);
        return new Timer(_ => action(), state, timeToGo, TimeSpan.FromDays(1));
    }

    private void AsyncWrapper() => Task.Run(Execution);

    private async Task Execution()
    {
        // _logger.LogInformation($"Transaction scheduler executing at {DateTime.Now:h:mm:ss tt zz}");
        // _countState = await _mathService.Count(_countState, 2);
        // _logger.LogInformation($"count at {_countState}");
        // var postScheduleProcRequest = new PostScheduleProcRequest() { OperDate = DateTime.Now};
        // var result = await _preTransactionService.CallApiDepositScheduleProc(postScheduleProcRequest);
        
        // var postScheduleProcRequest = new PostScheduleProcRequest() { OperDate = DateTime.Now};
        var testDate = DateTime.ParseExact("08/09/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture);
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