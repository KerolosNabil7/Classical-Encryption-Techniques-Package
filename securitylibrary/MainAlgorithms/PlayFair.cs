using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        string matrix = "";

        List<string> alphabetic = new List<string>{
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "k",
            "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
            "v", "w", "x", "y", "z"};

        public void Fill_Matrix(string key)
        {
            int key_Count = 0, alphabetic_Count = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    // Escape repeated chars
                    while (key_Count < key.Length && matrix.Contains(key[key_Count]))
                        key_Count++;

                    // There exist more chars in key
                    if (key_Count < key.Length)
                    {
                        matrix += key[key_Count];

                        if (key[key_Count] == 'i' || key[key_Count] == 'j')
                            alphabetic.Remove("i");
                        else
                            alphabetic.Remove(key[key_Count].ToString());
                        key_Count++;
                    }
                    // Continue filling the Matrix with the remaining alphabitcs
                    else
                    {
                        matrix += alphabetic[alphabetic_Count];
                        alphabetic_Count++;
                    }
                }
            }
        }

        public string Decrypt(string cipherText, string key)
        {
            // Split the Cipher Text into two chars groups
            List<List<string>> two_Chars = new List<List<string>>();
            for (int i = 0; i < cipherText.Length; i += 2)
                two_Chars.Add(new List<string> { cipherText[i].ToString(), cipherText[i + 1].ToString() });

            // Fill the 5 * 5 Matrix
            Fill_Matrix(key);

            // Decryption
            string plain_Text = "";
            foreach (List<string> chars in two_Chars)
            {
                int firstChar_Num = matrix.IndexOf(chars[0].ToLower()), firstChar_Row = firstChar_Num / 5, firstChar_Col = firstChar_Num % 5;
                int secondChar_Num = matrix.IndexOf(chars[1].ToLower()), secondChar_Row = secondChar_Num / 5, secondChar_Col = secondChar_Num % 5;

                // Same Row
                if (firstChar_Row == secondChar_Row)
                {
                    // First char
                    if (firstChar_Num % 5 == 0)
                        plain_Text += matrix[firstChar_Num + 4];
                    else
                        plain_Text += matrix[firstChar_Num - 1];

                    // Second char
                    if (secondChar_Num % 5 == 0)
                        plain_Text += matrix[secondChar_Num + 4];
                    else
                        plain_Text += matrix[secondChar_Num - 1];
                }
                // Same Column
                else if (firstChar_Col == secondChar_Col)
                {
                    if (firstChar_Num - 5 < 0)
                        plain_Text += matrix[firstChar_Num + 20];
                    else
                        plain_Text += matrix[firstChar_Num - 5];

                    if (secondChar_Num - 5 < 0)
                        plain_Text += matrix[secondChar_Num + 20];
                    else
                        plain_Text += matrix[secondChar_Num - 5];
                }
                // Different row and column
                else
                {
                    plain_Text += matrix[firstChar_Row * 5 + secondChar_Col];
                    plain_Text += matrix[secondChar_Row * 5 + firstChar_Col];
                }
                if(plain_Text.Length> 3)
                {
                    if ((plain_Text[plain_Text.Length - 2] == plain_Text[plain_Text.Length - 4]) && plain_Text[plain_Text.Length - 3] == 'x')
                    {
                        plain_Text = plain_Text.Remove(plain_Text.Length - 3, 1);
                    }
                }
            }

            // Extra added char will be removed
            if (plain_Text[plain_Text.Length - 1] == 'x')
                plain_Text = plain_Text.Remove(plain_Text.Length - 1);

            return plain_Text;
        }


        public string Encrypt(string plainText, string key)
        {
            // Split the Plain Text into two chars groups
            List<List<string>> two_Chars = new List<List<string>>();
            for (int i = 0; i < plainText.Length;)
            {
                if (plainText.Length - i > 1 && plainText[i] != plainText[i + 1])
                {
                    two_Chars.Add(new List<string> { plainText[i].ToString(), plainText[i + 1].ToString() });
                    i += 2;
                }
                else
                {
                    two_Chars.Add(new List<string> { plainText[i].ToString(), "x" });
                    i += 1;
                }
            }

            // Fill the 5 * 5 Matrix
            Fill_Matrix(key);

            // Encryption
            string cipher_Text = "";
            foreach (List<string> chars in two_Chars)
            {
                int firstChar_Num = matrix.IndexOf(chars[0]), firstChar_Row = firstChar_Num / 5, firstChar_Col = firstChar_Num % 5;
                int secondChar_Num = matrix.IndexOf(chars[1]), secondChar_Row = secondChar_Num / 5, secondChar_Col = secondChar_Num % 5;

                // Same Row
                if (firstChar_Row == secondChar_Row)
                {
                    // First char
                    if ((firstChar_Num + 1) % 5 == 0)
                        cipher_Text += matrix[firstChar_Num - 4];
                    else
                        cipher_Text += matrix[firstChar_Num + 1];

                    // Second char
                    if ((secondChar_Num + 1) % 5 == 0)
                        cipher_Text += matrix[secondChar_Num - 4];
                    else
                        cipher_Text += matrix[secondChar_Num + 1];
                }
                // Same Column
                else if (firstChar_Col == secondChar_Col)
                {
                    if ((firstChar_Num + 5) / 25 > 0)
                        cipher_Text += matrix[firstChar_Num - 20];
                    else
                        cipher_Text += matrix[firstChar_Num + 5];

                    if ((secondChar_Num + 5) / 25 > 0)
                        cipher_Text += matrix[secondChar_Num - 20];
                    else
                        cipher_Text += matrix[secondChar_Num + 5];
                }
                // Different row and column
                else
                {
                    cipher_Text += matrix[firstChar_Row * 5 + secondChar_Col];
                    cipher_Text += matrix[secondChar_Row * 5 + firstChar_Col];
                }

            }
            cipher_Text = cipher_Text.ToUpper();
            return cipher_Text;
        }
    }
}