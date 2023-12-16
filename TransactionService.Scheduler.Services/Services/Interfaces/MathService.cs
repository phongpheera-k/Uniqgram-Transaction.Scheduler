using TransactionService.Scheduler.Services.Services.Implements;

namespace TransactionService.Scheduler.Services.Services.Interfaces;

public class MathService : IMathService
{
    public async Task<int> Count(int startWith, int increment) 
        => await Task.FromResult(startWith + increment);
}