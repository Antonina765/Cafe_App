namespace Cafe.Data.Interface.Repositories;

public interface IChatMessageRepository
{
    void AddMessage(int? userId, string message);
    List<string> GetLastMessages(int count = 5);
}