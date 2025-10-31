namespace HillCipher.Interfaces;

public interface IHillCipherService
{
    string Encrypt(string plaintext, string key, string alphabet);
    string Decrypt(string ciphertext, string key, string alphabet);
}