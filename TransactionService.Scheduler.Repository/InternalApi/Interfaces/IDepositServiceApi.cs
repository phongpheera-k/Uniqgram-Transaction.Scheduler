using TransactionService.Scheduler.Services.Model;

namespace TransactionService.Scheduler.Repository.InternalApi.Interfaces;

public interface IDepositServiceApi
{
    Task<bool> ScheduleProc(PostScheduleProcRequest request);
}