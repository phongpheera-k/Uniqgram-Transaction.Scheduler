using TransactionService.Scheduler.Services.Services.Interfaces;

namespace TransactionService.Scheduler.Services.Services.Implements;

public class MathService : IMathService
{
    public async Task<int> Count(int startWith, int increment) 
        => await Task.FromResult(startWith + increment);
}