using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            int key = 1;

            while (key < plainText.Length)
            {
                string encryptedText = Encrypt(plainText, key);
                if (cipherText.Equals(encryptedText))
                {
                    return key;
                }
                else
                {
                    key++;
                }
            }
            return -1;
        }

        public string Decrypt(string cipherText, int key)
        {
            double length = Math.Ceiling((double)cipherText.Length / key);
            string plain_text = Encrypt(cipherText, (int)length);
            return plain_text;
        }

        public string Encrypt(string plainText, int key)
        {

            double x = (double)plainText.Length / key;
            double size = Math.Ceiling(x);
            string cipher_text = "";
            int count = 0;
            char[,] text_matrix = new char[key, (int)size];
            for (int i = 0; i < (int)size; i++)
            {
                for (int k = 0; k < key; k++)
                {
                    if (count == plainText.Length)
                    {
                        break;
                    }
                    text_matrix[k, i] = plainText[count];
                    count++;
                }
            }

            for (int i = 0; i < key; i++)
            {
                for (int k = 0; k < (int)size; k++)
                {
                    if (text_matrix[i, k] == '\0')
                    {
                        break;
                    }
                    cipher_text += text_matrix[i, k];
                }
            }
            return cipher_text.ToUpper();
        }

    }
}
