namespace TransactionService.Scheduler.Services.Services.Implements;

public interface IMathService
{
    Task<int> Count(int startWith, int increment);
}