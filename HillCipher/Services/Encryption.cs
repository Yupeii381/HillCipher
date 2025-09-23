using HillCipher.DataAccess.Postgres.Models;
namespace HillCipher.Services;

public class Encryption
{

}

public static class Validation
{
    public static (bool, string) IsValidKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return (false, "Key cannot be null or empty.");

        int len = key.Length;
        int sqrtLen = (int)Math.Sqrt(len);
        if (Math.Pow(len, sqrtLen) != len)
            return (false, "Key length must be a perfect square.");

        int[,] matrix = new int[sqrtLen, sqrtLen];
        

        return (true, string.Empty);
    }
}

