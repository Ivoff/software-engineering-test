namespace ForumAggregator.Application.Services;

public interface IPasswordService
{
    public string HashPassword(string password, out byte[] salt);

    public bool CheckPassword(string password, string hash, byte[] salt);
}