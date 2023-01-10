namespace ForumAggregator.Application.Services;

using System.Security.Cryptography;
using System.Text;

public class PasswordService: IPasswordService
{
    int _key_lenth = 64;
    HashAlgorithmName _hash_algorithm = HashAlgorithmName.SHA512;
    int iterations = 10000;

    public string HashPassword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(_key_lenth);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            _hash_algorithm,
            _key_lenth
        );

        return Convert.ToHexString(hash);
    }

    public bool CheckPassword(string password, string hash, byte[] salt)
    {
        var newHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, _hash_algorithm, _key_lenth);
        return newHash.SequenceEqual(Convert.FromHexString(hash));
    }
}