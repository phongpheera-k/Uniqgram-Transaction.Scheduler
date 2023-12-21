using TransactionService.Scheduler.Services.Model;

namespace TransactionService.Scheduler.Services.Services.Interfaces;

public interface IPreTransactionService
{
    Task<bool> CallApiDepositScheduleProc(PostScheduleProcRequest request);
    Task<string> CallApiDepositVersion();
}