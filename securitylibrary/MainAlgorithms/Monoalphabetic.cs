using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            string empty = "";
            StringBuilder key = new StringBuilder(empty);
            List<char> test = new List<char>();
            string l = "abcdefghijklmnopqrstuvwxyz";
            string l_upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            bool flag = false;

            for (int i = 0; i < l.Length; i++)
            {
                for (int j = 0; j < plainText.Length; j++)
                {
                    if (l[i] == plainText[j] && !test.Contains(cipherText[j]))
                    {
                        key.Append(cipherText[j]);
                        test.Add(cipherText[j]);
                        flag = true;
                        break;
                    }
                }
                if (flag == false && i > 0)
                {
                    key.Append(',');
                }
                flag = false;
            }
            for (int i = 0; i < key.Length; i++)
            {
                if (key[i] == ',')
                {
                    for (int j = 0; j < l_upper.Length; j++)
                    {
                        if (!test.Contains(l_upper[j]))
                        {
                            test.Add(l_upper[j]);
                            key[i] = l_upper[j];
                            break;
                        }
                    }
                }
            }
            return key.ToString().ToLower();
        }


        public string Decrypt(string cipherText, string key)
        {
            string letters = "abcdefghijklmnopqrstuvwxyz";
            string empty = "";
            StringBuilder plain = new StringBuilder(empty);
            for (int i = 0; i < cipherText.Length; i++)
            {
                plain.Append(letters[index_of_letter(cipherText[i], key)]);
            }
            return plain.ToString();
        }

        public string Encrypt(string plainText, string key)
        {
            string empty = "";
            StringBuilder cipher = new StringBuilder(empty);
            for (int i = 0; i < plainText.Length; i++)
            {
                cipher.Append(key[index_of_alpha(plainText[i])]);
            }
            return cipher.ToString();
        }

        private int index_of_alpha(char letter)
        {
            char[] alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] alpha_capital = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            for (int i = 0; i < alpha.Length; i++)
            {
                if (alpha[i] == letter || alpha_capital[i] == letter)
                    return i;
            }
            return -1;
        }

        private int index_of_letter(char letter, string key)
        {
            key = key.ToUpper();
            for (int i = 0; i < key.Length; i++)
            {
                if (key[i].Equals(letter))
                    return i;
            }
            return -1;
        }
        private char is_letter_exists(char letter, string key)
        {
            key = key.ToUpper();
            int index = index_of_alpha(letter);
            for (int i = 0; i < key.Length; i++)
            {
                if (key[i].Equals(letter))
                    return 'f';
            }
            return 'f';
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        /// 
        struct structt
        {
            public char c;
            public int count;
        }
        public string AnalyseUsingCharFrequency(string cipher)
        {
            string l = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            structt s;
            structt[] unordered_list = new structt[26];
            char[] cipher_freq = new char[26];

            int counter = 0;
            for (int i = 0; i < l.Length; i++)
            {
                for (int j = 0; j < cipher.Length; j++)
                {
                    if (l[i] == cipher[j])
                        counter++;
                }
                s.c = l[i];
                s.count = counter;
                unordered_list[i] = s;
                counter = 0;
            }
            int max = -10;
            int index = 0;

            for (int j = 0; j < unordered_list.Length; j++)
            {
                for (int i = 0; i < unordered_list.Length; i++)
                {
                    if (unordered_list[i].count > max)
                    {
                        max = unordered_list[i].count;
                        index = i;
                    }
                }
                cipher_freq[j] = unordered_list[index].c;
                unordered_list[index].count = -1000000;
                max = -10;
            }

            string char_by_frequency = "etaoinsrhldcumfpgwybvkxjqz";

            StringBuilder sb = new StringBuilder(cipher);
            for (int i = 0; i < cipher.Length; i++)
            {
                for (int j = 0; j < cipher_freq.Length; j++)
                {
                    if (cipher[i] == cipher_freq[j])
                    {
                        sb[i] = char_by_frequency[j];
                    }
                }
            }
            return sb.ToString();
        }
    }
}