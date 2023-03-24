using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            string key = "";
            int[][] table = CreatingVigenereTable();
            Dictionary<char, int> char_to_number = get_char_place();
            Dictionary<int, char> number_to_char = get_number_char();
            char x;
            int count = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                int plain_item = char_to_number[plainText[i]];
                for (int k = 0; k < 26; k++)
                {
                    x = (char)table[plain_item][k];

                    if (x.Equals(cipherText[i]))
                    {
                        if (plainText[count] == number_to_char[k])
                        {
                            count++;
                            if (count == 3)
                            {
                                key = key.Substring(0, key.Length - 2);
                                return key;
                            }
                        }
                        else
                        {
                            count = 0;
                        }
                        key += number_to_char[k];

                        break;
                    }

                }
            }


            return "";
        }

        public string Decrypt(string cipherText, string key)
        {
            string plain_text = "";
            int[][] table = CreatingVigenereTable();
            Dictionary<char, int> char_to_number = get_char_place();
            Dictionary<int, char> number_to_char = get_number_char();
            char x;

            for (int i = 0; i < key.Length; i++)
            {
                int key_item = char_to_number[key[i]];
                for (int k = 0; k < 26; k++)
                {
                    x = (char)table[key_item][k];
                    if (x.Equals(cipherText[i]))
                    {
                        plain_text += number_to_char[k];
                        key += plain_text[i];
                        if (i == cipherText.Length - 1)
                        {
                            return plain_text;
                        }
                        break;
                    }
                }
            }
            return "";
        }

        public string Encrypt(string plainText, string key)
        {
            string cipherText = "";
            //Creating Vigenere Table
            int[][] alpha = CreatingVigenereTable();
            if (key.Length < plainText.Length)
            {
                int diff = plainText.Length - key.Length;
                for (int i = 0; i < diff; i++)
                    key += plainText[i];
            }
            for (int i = 0; i < plainText.Length; i++)
            {
                int x, y;
                x = y = 0;
                for (int j = 0; j < 26; j++)
                {
                    if (plainText[i] == char.ToLower((char)alpha[0][j]))
                    {
                        x = j;
                        break;
                    }
                }
                for (int j = 0; j < 26; j++)
                {
                    if (key[i] == char.ToLower((char)alpha[0][j]))
                    {
                        y = j;
                        break;
                    }
                }
                cipherText += (char)alpha[x][y];
            }
            return cipherText;
        }


        public int[][] CreatingVigenereTable()
        {
            int[][] alpha = new int[26][];
            for (int i = 0; i < 26; i++)
            {
                alpha[i] = new int[26];
                for (int j = 0; j < 26; j++)
                {
                    alpha[i][j] = (i + j) + 65;
                    if (alpha[i][j] >= 91)
                    {
                        alpha[i][j] -= 26;
                    }
                }
            }
            return alpha;
        }

        public Dictionary<char, int> get_char_place()
        {
            char[] c = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            int key = 0;
            Dictionary<char, int> dictionary = new Dictionary<char, int>();
            foreach (char value in c)
            {
                dictionary.Add(value, key);
                key++;
            }
            return dictionary;
        }
        public Dictionary<int, char> get_number_char()
        {
            char[] c = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            int key = 0;
            Dictionary<int, char> dictionary = new Dictionary<int, char>();
            foreach (char value in c)
            {
                dictionary.Add(key, value);
                key++;
            }
            return dictionary;
        }
    }
}
