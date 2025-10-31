using HillCipher.Interfaces;
using System;
using System.Linq;

namespace HillCipher.Services
{
    public class HillKeyService : IHillKeyService
    {
        public bool IsKeyValid(string key, string alphabet)
        {
            int n = (int)Math.Sqrt(key.Length);
            if (n * n != key.Length) throw new ArgumentException("Длина ключа должна быть квадратом целого числа.");

            int[,] matrix = GenerateKeyMatrix(key, alphabet);
            int det = Determinant(matrix);
            int mod = alphabet.Length;
            int modDet = ((det % mod) + mod) % mod;

            if (modDet == 0)
                return false;

            return GCD(det, mod) == 1;
        }
        public void ValidateInputs(string text, string key, string alphabet)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Текст не должен быть пустым.");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Ключ не должен быть пустым.");

            if (string.IsNullOrEmpty(alphabet))
                throw new ArgumentException("Алфавит не должен быть пустым.");

            if (!IsKeyValid(key, alphabet))
                throw new ArgumentException("Недопустимый ключ для заданного алфавита.");

            if (text.Any(c => !alphabet.Contains(c)))
                throw new ArgumentException("Текст содержит символы, отсутствующие в алфавите.");

            int n = (int)Math.Sqrt(key.Length);
            if (n * n != key.Length)
                throw new ArgumentException("Длина ключа должна быть квадратом целого числа (например, 4, 9, 16).");
        }
        public int[,] GenerateKeyMatrix(string key, string alphabet)
        {
            int n = (int)Math.Sqrt(key.Length);
            int[,] matrix = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    char c = key[i * n + j];
                    matrix[i, j] = alphabet.IndexOf(c) + 1;
                }
            }
            return matrix;
        }
        private static int GCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            if (a == 0) return b;
            if (b == 0) return a;

            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private static int Determinant(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            if (n == 1)
                return matrix[0, 0];
            if (n == 2)
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            int det = 0;
            for (int p = 0; p < n; p++)
            {
                int[,] subMatrix = new int[n - 1, n - 1];
                for (int i = 1; i < n; i++)
                {
                    int colIndex = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (j == p) continue;
                        subMatrix[i - 1, colIndex] = matrix[i, j];
                        colIndex++;
                    }
                }
                det += matrix[0, p] * Determinant(subMatrix) * (p % 2 == 0 ? 1 : -1);
            }
            return det;
        }

        public int[,] InvertMatrixMod(int[,] matrix, int mod)
        {
            int n = matrix.GetLength(0);
            int det = Determinant(matrix);
            int detInv = ModInverse(((det % mod) + mod) % mod, mod);

            int[,] adj = AdjugateMatrix(matrix);
            int[,] inv = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    inv[i, j] = ((adj[i, j] * detInv) % mod + mod) % mod;
                }
            }
            return inv;
        }
        private int[,] AdjugateMatrix(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int[,] adj = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int[,] sub = SubMatrix(matrix, i, j);
                    int sign = ((i + j) % 2 == 0) ? 1 : -1;
                    adj[j, i] = sign * Determinant(sub);
                }
            }
            return adj;
        }
        private int[,] SubMatrix(int[,] matrix, int row, int col)
        {
            int n = matrix.GetLength(0);
            int[,] sub = new int[n - 1, n - 1];
            for (int i = 0, r = 0; i < n; i++)
            {
                if (i == row) continue;
                for (int j = 0, c = 0; j < n; j++)
                {
                    if (j == col) continue;
                    sub[r, c++] = matrix[i, j];
                }
                r++;
            }
            return sub;
        }
        private int ModInverse(int a, int mod)
        {
            a = ((a % mod) + mod) % mod;
            for (int x = 1; x < mod; x++)
                if ((a * x) % mod == 1) return x;
            throw new ArgumentException("Модульный обратный не существует.");
        }
    }

}