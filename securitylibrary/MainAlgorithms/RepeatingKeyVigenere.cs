using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            string key = "";
            int[][] table = CreatingVigenereTable();
            Dictionary<char, int> char_to_number = get_char_place();
            Dictionary<int, char> number_to_char = get_number_char();
            Dictionary<string, bool> keyTable = new Dictionary<string, bool>();
            char x;
            string currKey = "";
            int count = 0;
            List<string> subStrings = new List<string>();
            for (int i = 0; i < plainText.Length; i++)
            {
                int plain_item = char_to_number[plainText[i]];
                for (int k = 0; k < 26; k++)
                {
                    x = (char)table[plain_item][k];

                    if (x.Equals(cipherText[i]))
                    {
                        if (keyTable.ContainsKey(number_to_char[k].ToString().ToUpper()) && keyTable.ContainsKey(currKey))
                        {
                            //key = (key.Length >= currKey.Length) ? key : currKey;
                            currKey = number_to_char[k].ToString().ToUpper();
                            subStrings.Add(currKey);
                            break;
                        }
                        currKey += (char)table[0][k];
                        subStrings.Add(currKey);
                        if (!keyTable.ContainsKey(currKey))
                            keyTable.Add(currKey, true);
                        break;
                    }
                }
            }
            keyTable = new Dictionary<string, bool>();
            for (int i = 0; i < subStrings.Count; i++)
            {
                if (keyTable.ContainsKey(subStrings[i]))
                {
                    if (!key.Contains(subStrings[i - 1]))
                        key += subStrings[i - 1];
                }
                else
                    keyTable.Add(subStrings[i], true);
            }

            return key;
        }

        public string Decrypt(string cipherText, string key)
        {
            string plainText = "";
            //Creating Vigenere Table
            int[][] alpha = CreatingVigenereTable();
            if (key.Length < cipherText.Length)
            {
                int diff = cipherText.Length - key.Length;
                for (int i = 0; i < diff; i++)
                    key += key[i];
            }
            for (int i = 0; i < key.Length; i++)
            {
                int x, y;
                x = y = 0;
                for (int j = 0; j < 26; j++)
                {
                    if (key[i] == char.ToLower((char)alpha[0][j]))
                    {
                        x = j;
                        break;
                    }
                }
                for (int j = 0; j < 26; j++)
                {
                    char ch1 = cipherText[i];
                    char ch2 = (char)alpha[j][x];
                    if (ch1 == ch2)
                    {
                        y = j;
                        break;
                    }
                }
                plainText += char.ToLower((char)alpha[0][y]);
            }
            return plainText;
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
                    key += key[i];
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

        private int[][] CreatingVigenereTable()
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

        private Dictionary<char, int> get_char_place()
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
        private Dictionary<int, char> get_number_char()
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