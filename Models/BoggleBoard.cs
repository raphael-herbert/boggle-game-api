public class BoggleBoard
{
    public List<List<char>> Board { get; set; }
    public List<string> ValidWords { get; set; }

    public BoggleBoard(List<List<char>> board, List<string> validWords) {
        this.Board = board;
        this.ValidWords = validWords;
    }
}
