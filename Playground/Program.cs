namespace Playground
{
    public class HillCipherService
    {
        public int[,] GenerateKeyMatrix(string key, string alphabet)
        {
            if (Math.Sqrt(key.Length) - (int)Math.Sqrt(key.Length) != 0)
            {
                throw new ArgumentException("String length must be square of number.");
            }
            int matrSize = (int)Math.Sqrt(key.Length);
            int[,] result = new int[matrSize, matrSize];

            Dictionary<char, int> someDict = MakeAlphDict(alphabet);

            int k = 0;
            for (int i  = 0; i < matrSize; i++)
            {
                for (int j = 0; j < matrSize; j++)
                {
                    result[i, j] = someDict[key[k]];
                    k++;
                }
            }
            return result;
        }

        public double CalculateDeterminant(int[,] matrix)
        {
            int size = matrix.GetLength(0);
            double[,] temp = new double[size, size];

            for (int i =0; i<size; i++)
            {
                for (int j = 0;j < size; j++)
                {
                    temp[i,j] = matrix[i,j];
                }
            }

            double det = 1;

            for (int i = 0; i < size; i++)
            {
                int pivot = i;
                for (int j = pivot + 1; j < size; j++)
                {
                    if (Math.Abs(temp[j,i]) > Math.Abs(temp[pivot, i]))
                    {
                        pivot = j;
                    }
                }

                if (pivot != i)
                {
                    for (int k = 0; k < size; k++)
                    {
                        (temp[i, k], temp[pivot, k]) = (temp[pivot, k], temp[i, k]);
                    }
                    det = -det;
                }

                if (temp[i,i] == 0)
                {
                    return 0;
                }
                det *= temp[i, i];

                for (int k = i + 1; k < size; k++)
                {
                    double factor = (temp[k, i] / temp[i, i]);
                    for (int j = i; j < size; j++)
                    {
                        temp[k, j] -= (factor * temp[i, j]);
                    }
                }
            }
            return det;
        }

        public void PrintKeyMatrix(int[,] keyMatrix)
        {
            int size = keyMatrix.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(keyMatrix[i, j].ToString() + "\t");
                }
                Console.WriteLine();
            }
        }

        public Dictionary<char, int> MakeAlphDict(string alphabet) => alphabet
                    .Select((character, index) => new { character, index })
                    .ToDictionary(x => x.character, x => x.index + 1);

        private int GCD(int a, int b) => b == 0 ? a : GCD(a, a % b)
    }
    public class Program()
    {
        public static void Main(string[] args)
        {
            var service = new HillCipherService();

            string alphabet = @"абвгдеёжзийклмнопрстуфхцчшщъыьэюя., ?";
            string keyWord = @"кодовое слово же";

            int[,] keyMatrix = service.GenerateKeyMatrix(keyWord, alphabet);
            service.PrintKeyMatrix(keyMatrix);
            Console.WriteLine(service.CalculateDeterminant(keyMatrix));
        }
    }
}
