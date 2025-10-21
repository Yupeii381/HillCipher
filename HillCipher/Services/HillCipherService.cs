using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace HillCipher.Services
{
    public class HillCipherService
    {
        private int[,] GenerateKeyMatrix(string key)
        {
            if ((int)Math.Sqrt(key.Length) != Math.Sqrt(key.Length))
            {
                throw new ArgumentException("String length must be square of number.");
            }

        }
        private Dictionary<char, int> MakeAlphDict(string alphabet) => alphabet
            .Select((character, index) => new {character, index})
            .ToDictionary(x => x.character, x => x.index + 1);
    }
}
