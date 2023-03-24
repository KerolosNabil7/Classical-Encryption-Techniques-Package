using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            char[] characters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string chText = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                int index = 0;
                for (int j = 0; j < 26; j++)
                {
                    if (char.ToUpper(plainText[i]) == characters[j])
                    {
                        index = j;
                        break;
                    }
                }
                index = (index + key) % 26;
                chText += characters[index];
            }
            return chText;
        }

        public string Decrypt(string cipherText, int key)
        {
            char[] characters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string plain_text = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                int index = 0;
                for (int j = 0; j < 26; j++)
                {
                    if (cipherText[i] == characters[j])
                    {
                        index = j;
                        break;
                    }
                }
                index = (index - key) % 26;
                if (index < 0)
                    index = 26 + index;
                plain_text += char.ToLower(characters[index]);
            }
            return plain_text;
        }

        public int Analyse(string plainText, string cipherText)
        {
            char[] characters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            int cipher_index, plain_index;
            cipher_index = plain_index = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                if (char.ToUpper(plainText[0]) == characters[i])
                    plain_index = i;
                if (cipherText[0] == characters[i])
                    cipher_index = i;
            }
            int key = ((cipher_index - plain_index) % 26);
            if (key < 0)
                key = 26 + key;
            return key;
        }
    }
}
