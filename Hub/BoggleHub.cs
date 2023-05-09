using Microsoft.AspNetCore.SignalR;

public class BoggleHub : Hub
{
    private readonly IBoggleService _boggleService;
    private readonly IBoggleTimerService _boggleTimerService;
    private static Dictionary<string, int> _scores = new Dictionary<string, int>();

    public BoggleHub(IBoggleService boggleService, IBoggleTimerService boggleTimerService)
    {
        _boggleTimerService = boggleTimerService;
        _boggleService = boggleService;
    }

    public override async Task OnConnectedAsync()
    {
        BoggleBoard newBoard = _boggleService.GetBoard();
        await Clients.Caller.SendAsync("ReceiveBoard", newBoard);
        await Clients.Caller.SendAsync("ReceiveRemainingTime", _boggleTimerService.GetRemainingTime());
        await base.OnConnectedAsync();
        await Clients.Caller.SendAsync("ReceiveConnectedClients", _scores);
        // await Clients.All.SendAsync("ClientConnected", Context.ConnectionId);
    }

    public async Task UpdateScore(string nickname, int score)
    {
        await Clients.All.SendAsync("ReceiveScoreUpdate", nickname, score);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _scores.Remove(Context.ConnectionId);
        // await Clients.Others.SendAsync("ClientDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
