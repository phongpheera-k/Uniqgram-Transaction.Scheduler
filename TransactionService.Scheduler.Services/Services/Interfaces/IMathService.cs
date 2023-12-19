namespace TransactionService.Scheduler.Services.Services.Interfaces;

public interface IMathService
{
    Task<int> Count(int startWith, int increment);
}