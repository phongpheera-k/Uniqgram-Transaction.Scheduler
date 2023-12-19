using TransactionService.Scheduler.Repository.InternalApi.Interfaces;
using TransactionService.Scheduler.Services.Model;
using TransactionService.Scheduler.Services.Services.Interfaces;

namespace TransactionService.Scheduler.Services.Services.Implements;

public class PreTransactionService : IPreTransactionService
{
    private readonly IDepositServiceApi _depositServiceApi;

    public PreTransactionService(IDepositServiceApi depositServiceApi)
    {
        _depositServiceApi = depositServiceApi;
    }
    
    public async Task<bool> CallApiDepositScheduleProc(PostScheduleProcRequest request)
    {
        // var result = await _depositServiceApi.ScheduleProc(request);
        return await _depositServiceApi.ScheduleProc(request);
    }
}