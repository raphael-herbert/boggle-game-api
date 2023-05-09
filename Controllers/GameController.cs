using Microsoft.AspNetCore.Mvc;

namespace boggle_api.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IBoggleService _boggleService;

    public GameController(IBoggleService boggleService)
    {
        _boggleService = boggleService;
    }

    [HttpGet("join")]
    public IActionResult Join()
    {
        BoggleBoard board = _boggleService.GetBoard(); 
        return Ok(board);
    }
}
