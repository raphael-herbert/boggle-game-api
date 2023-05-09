public class BoardService : IBoardService
{
    private HashSet<string> validWords = new HashSet<string>();
    private char[][]? board;

    private IDictionaryService DictionaryService;

    public BoardService(IDictionaryService dictionaryService) { 
        DictionaryService = dictionaryService;
    }

    public BoggleBoard Generate(int size = 4) {
        string letters = "AAAAAAAAABBCCDDDDEEEEEEEEEEEEEEEEFFFFGGGHHIIIIIIIIIIIIIIIIJJKKLLLLLLLMMMNNNNNNNNNOOOOOOOOPPQRRRRRRRRRSSSSSSSSSTTTTTTTUUUUUUUUUUVVWWXYYZ";
        Random random = new Random();
        do {
            validWords = new HashSet<string>();
            board = new char[size][];
            for (int i = 0; i < size; i++)
            {
                board[i] = new char[size];
                for (int j = 0; j < size; j++)
                {
                    board[i][j] = letters[random.Next(letters.Length)];
                }
            }
            validWords = FindAllValidWords(board);
        } while (validWords.Count < 120);

        return new BoggleBoard(this.ConvertToNestedList(board), validWords.ToList());
    }

    private HashSet<string> FindAllValidWords(char[][] board)
    {
        HashSet<string> validWords = new HashSet<string>();
        int rows = board.Length;
        int cols = board[0].Length;
        bool[][] visited = new bool[rows][];
        for (int i = 0; i < rows; i++)
        {
            visited[i] = new bool[cols];
        }

        void FindWordsDFS(int row, int col, string prefix)
        {
            if (DictionaryService.HasWord(prefix))
            {
                validWords.Add(prefix);
            }

            if (!DictionaryService.HasPrefix(prefix))
            {
                return;
            }

            if (row < 0 || row >= rows || col < 0 || col >= cols || visited[row][col])
            {
                return;
            }

            visited[row][col] = true;
            int[][] directions = new[]
            {
                new[] { -1, -1 },
                new[] { -1, 0 },
                new[] { -1, 1 },
                new[] { 0, -1 },
                new[] { 0, 1 },
                new[] { 1, -1 },
                new[] { 1, 0 },
                new[] { 1, 1 },
            };

            foreach (int[] direction in directions)
            {
                int newRow = row + direction[0];
                int newCol = col + direction[1];
                if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || visited[newRow][newCol])
                {
                    continue;
                }
                FindWordsDFS(newRow, newCol, prefix + board[newRow][newCol]);
            }

            visited[row][col] = false;
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                FindWordsDFS(i, j, board[i][j].ToString());
            }
        }

        return validWords;
    }

    private List<List<char>> ConvertToNestedList(char[][] array)
    {
        var nestedList = new List<List<char>>();
        for (int i = 0; i < array.Length; i++)
        {
            nestedList.Add(new List<char>(array[i]));
        }
        return nestedList;
    }
}