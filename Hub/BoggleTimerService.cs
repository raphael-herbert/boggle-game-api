using System.Timers;
using Microsoft.AspNetCore.SignalR;

using Timer = System.Timers.Timer;

public class BoggleTimerService : IBoggleTimerService
{
    private readonly IBoggleService _boggleService;
    private readonly IHubContext<BoggleHub> _hubContext;
    private readonly Timer _timer;
    private const int GameDurationInSeconds = 180;
    private DateTime _lastResetTime;
 
    public BoggleTimerService(IBoggleService boggleService, IHubContext<BoggleHub> hubContext)
    {
        _boggleService = boggleService;
        _hubContext = hubContext;
        _timer = new Timer(GameDurationInSeconds * 1000);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        _timer.Enabled = true;
        _lastResetTime = DateTime.UtcNow;
    }

    public int GetRemainingTime()
    {
        var elapsedTime = DateTime.UtcNow - _lastResetTime;
        var remainingTime = GameDurationInSeconds - elapsedTime.TotalSeconds;
        return (int)Math.Max(0, remainingTime);
    }

    private async void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        var newBoard = _boggleService.NewBoard();
        await _hubContext.Clients.All.SendAsync("ReceiveBoard", newBoard);
        await _hubContext.Clients.All.SendAsync("ReceiveRemainingTime", GameDurationInSeconds);
        _lastResetTime = DateTime.UtcNow; // Update the last reset time
    }
}
