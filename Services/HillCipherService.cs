using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace HillCipher.Services
{
    public class HillCipherService : IHillCipherService
    {
        private readonly IHillKeyService _keyService;
        public HillCipherService(IHillKeyService keyService) => _keyService = keyService;

        public string Encrypt(string plaintext, string key, string alphabet)
        {
            _keyService.ValidateInputs(plaintext, key, alphabet);

            var keyMatrix = _keyService.GenerateKeyMatrix(key, alphabet);

            int n = keyMatrix.GetLength(0);
            int mod = alphabet.Length;

            int remainder = plaintext.Length % n;
            if (remainder != 0)
            {
                int paddingNeeded = n - remainder;
                char paddingChar = plaintext[^1];
                plaintext = plaintext.PadRight(plaintext.Length + paddingNeeded, paddingChar);
            }

            StringBuilder EncryptedText = new StringBuilder();

        

            for (int i = 0; i < plaintext.Length; i += n)
            {
                int[] vector = new int[n];
                for (int j = 0; j < n; j++)
                    vector[j] = alphabet.IndexOf(plaintext[i + j]) + 1;

                int[] encryptedBlock = MultiplyMatrixVector(keyMatrix, vector, mod);

                for (int j = 0; j < n; j++)
                    EncryptedText.Append(alphabet[(encryptedBlock[j] - 1 + mod) % mod]);
            }

            return EncryptedText.ToString();
        }
        public string Decrypt(string ciphertext, string key, string alphabet)
        {
            _keyService.ValidateInputs(ciphertext, key, alphabet);
            if (ciphertext.Length % Math.Sqrt(key.Length) != 0)
                throw new ArgumentException("Длина зашифрованного текста должна быть кратна размеру ключа.");

            int[,] keyMatrix = _keyService.GenerateKeyMatrix(key, alphabet);
            int[,] inverseMatrix = _keyService.InvertMatrixMod(keyMatrix, alphabet.Length);
            int n = keyMatrix.GetLength(0);
            int mod = alphabet.Length;

            StringBuilder decrypted = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i += n)
            {
                int[] block = new int[n];
                for (int j = 0; j < n; j++)
                    block[j] = alphabet.IndexOf(ciphertext[i + j]) + 1;

                int[] decryptedBlock = MultiplyMatrixVector(inverseMatrix, block, mod);

                for (int j = 0; j < n; j++)
                    decrypted.Append(alphabet[(decryptedBlock[j] - 1 + mod) % mod]);
            }

            return decrypted.ToString();
        }


        private static int[] MultiplyMatrixVector(int[,] matrix, int[] vector, int mod)
        {
            int n = matrix.GetLength(0);
            int[] result = new int[n];

            for (int i = 0; i < n; i++)
            {
                int sum = 0;
                for (int j = 0; j < n; j++)
                    sum += matrix[i, j] * vector[j];
                result[i] = ((sum % mod) + mod) % mod;
                if (result[i] == 0)
                    result[i] = mod; 
            }
            return result;
        }
    }
}