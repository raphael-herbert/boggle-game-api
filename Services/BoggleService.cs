public class BoggleService : IBoggleService
{

    private IBoardService BoardService;

    private BoggleBoard? currentBoard;

    public BoggleService(IBoardService boardService) {
        BoardService = boardService;
    }

    public BoggleBoard NewBoard(int size = 4)
    {
        currentBoard = BoardService.Generate();
        return currentBoard;
    }

    public BoggleBoard GetBoard(int size = 4)
    {
        if (currentBoard is not null)
            return currentBoard;
        
        currentBoard = BoardService.Generate();
        return currentBoard;
    }
}