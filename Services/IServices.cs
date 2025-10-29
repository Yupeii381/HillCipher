namespace HillCipher.Services
{
    public interface IHillCipherService
    {
        string Encrypt(string plaintext, string key, string alphabet);
        string Decrypt(string ciphertext, string key, string alphabet);
    }
    public interface IHillKeyService
    {
        bool IsKeyValid(string key, string alphabet);
        int[,] GenerateKeyMatrix(string key, string alphabet);
        void ValidateInputs(string text, string key, string alphabet);
        int[,] InvertMatrixMod(int[,] matrix, int mod);
    }
}