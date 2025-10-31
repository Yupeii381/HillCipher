namespace HillCipher.Interfaces;

public interface IHillKeyService
{
    bool IsKeyValid(string key, string alphabet);
    int[,] GenerateKeyMatrix(string key, string alphabet);
    void ValidateInputs(string text, string key, string alphabet);
    int[,] InvertMatrixMod(int[,] matrix, int mod);
}