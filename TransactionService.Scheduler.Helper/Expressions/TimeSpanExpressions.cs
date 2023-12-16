using System.Text.RegularExpressions;

namespace TransactionService.Scheduler.Helper.Expressions;

public static class TimeSpanExpressions
{
    public static TimeSpan ParseToTimeSpan(string input)
    {
        var pattern = new Regex(@"(?:(\d+)Y)?(?:(\d+)M)?(?:(\d+)D)?(?:(\d+)h)?(?:(\d+)m)?(?:(\d+)s)?");
        var match = pattern.Match(input);

        var years = string.IsNullOrEmpty(match.Groups[1].Value) ? 0 : int.Parse(match.Groups[1].Value);
        var months = string.IsNullOrEmpty(match.Groups[2].Value) ? 0 : int.Parse(match.Groups[2].Value);
        var days = string.IsNullOrEmpty(match.Groups[3].Value) ? 0 : int.Parse(match.Groups[3].Value);
        var hours = string.IsNullOrEmpty(match.Groups[4].Value) ? 0 : int.Parse(match.Groups[4].Value);
        var minutes = string.IsNullOrEmpty(match.Groups[5].Value) ? 0 : int.Parse(match.Groups[5].Value);
        var seconds = string.IsNullOrEmpty(match.Groups[6].Value) ? 0 : int.Parse(match.Groups[6].Value);

        var totalDays = days + months * 30 + years * 365;
        return new TimeSpan(totalDays, hours, minutes, seconds);
    }
}