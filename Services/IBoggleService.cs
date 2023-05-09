public interface IBoggleService
{
    BoggleBoard GetBoard(int size = 4);
    BoggleBoard NewBoard(int size = 4);
}