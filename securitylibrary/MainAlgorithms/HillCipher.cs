using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {
        int DeterminantOfMatrix(int[,] mat, int n)
        {
            int num1, num2, det = 1, index, total = 1; // Initialize result

            // temporary array for storing row
            int[] temp = new int[n + 1];

            // loop for traversing the diagonal elements
            for (int i = 0; i < n; i++)
            {
                index = i; // initialize the index

                // finding the index which has non zero value
                while (index < n && mat[index, i] == 0)
                {
                    index++;
                }
                if (index == n) // if there is non zero element
                {
                    // the determinant of matrix is zero
                    continue;
                }
                if (index != i)
                {
                    // loop for swapping the diagonal element row and index row
                    for (int j = 0; j < n; j++)
                    {
                        Swap(mat, index, j, i, j);
                    }
                    // determinant sign changes when we shift
                    // rows go through determinant properties
                    det = (int)(det * Math.Pow(-1, index - i));
                }

                // storing the values of diagonal row elements
                for (int j = 0; j < n; j++)
                {
                    temp[j] = mat[i, j];
                }

                // traversing every row below the diagonal
                // element
                for (int j = i + 1; j < n; j++)
                {
                    num1 = temp[i]; // value of diagonal element
                    num2 = mat[j,
                               i]; // value of next row element

                    // traversing every column of row
                    // and multiplying to every row
                    for (int k = 0; k < n; k++)
                    {

                        // multiplying to make the diagonal
                        // element and next row element equal
                        mat[j, k] = (num1 * mat[j, k])
                                    - (num2 * temp[k]);
                    }
                    total *= num1; // Det(kA)=kDet(A);
                }
            }

            // multiplying the diagonal elements to get
            // determinant
            for (int i = 0; i < n; i++)
            {
                det *= mat[i, i];
            }
            return (det / total); // Det(kA)/k=Det(A);
        }

        int[,] Swap(int[,] arr, int i1, int j1, int i2, int j2)
        {
            (arr[i2, j2], arr[i1, j1]) = (arr[i1, j1], arr[i2, j2]);
            return arr;
        }

        int Calc_Smaller_Det(int[,] matrix, int row, int col)
        {
            int N = 3, count1 = 0, count2 = 0, count = 1;
            bool flag = false;
            int[] first_row = new int[2], second_row = new int[2];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i != row && j != col && count < 3)
                    {
                        count++;
                        first_row[count1++] = matrix[i, j];
                        if (count == 3)
                            flag = true;
                        continue;
                    }
                    if (i != row && j != col && flag)
                        second_row[count2++] = matrix[i, j];
                }
            }
            return first_row[0] * second_row[1] - first_row[1] * second_row[0];
        }

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            List<int> key = new List<int>() { i, j, k, l };
                            List<int> cipher = Encrypt(plainText, key);
                            if (cipher.SequenceEqual(cipherText))
                            {
                                return key;
                            }
                        }
                    }
                }
            }
            throw new InvalidAnlysisException();
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            // Dimension of input matrix
            int N = (int)Math.Sqrt(key.Count);

            // Convert the key to matrix
            int[,] key_Matrix = new int[N, N], temp_Matrix = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    key_Matrix[i, j] = key[i * N + j];
                    temp_Matrix[i, j] = key[i * N + j];
                }
            }

            // Calculate the det of key
            int det = (DeterminantOfMatrix(temp_Matrix, N) % 26 + 26) % 26;


            // Calculate new value of det
            if (N == 3)
            {
                int count = 1;
                while (count * det % 26 != 1)
                    count++;

                det = count;
            }

            // Calculate the inverse matrix of key
            int[,] key_Inverse = new int[N, N];
            if (N > 2)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        key_Inverse[i, j] = (det * (int)Math.Pow(-1, i + j) * Calc_Smaller_Det(key_Matrix, i, j) % 26 + 26) % 26;
                    }
                }
            }
            else
            {
                key_Inverse[0, 0] = ((det * key_Matrix[1, 1] % 26) + 26) % 26;
                key_Inverse[1, 1] = ((det * key_Matrix[0, 0] % 26) + 26) % 26;
                key_Inverse[0, 1] = ((det * -key_Matrix[0, 1] % 26) + 26) % 26;
                key_Inverse[1, 0] = ((det * -key_Matrix[1, 0] % 26) + 26) % 26;
            }

            // Calculate the transpose matrix of key
            int[,] key_Transpose = new int[N, N];
            if (N > 2)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        key_Transpose[i, j] = key_Inverse[j, i];
                    }
                }
            }

            // Convert cipher to matrix
            int columns_num = cipherText.Count / N;
            int[,] cipher_text = new int[N, columns_num];
            for (int i = 0; i < columns_num; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    cipher_text[j, i] = cipherText[i * N + j];
                }
            }

            // Calculate plain text
            List<int> plain_Text = new List<int>();
            for (int i = 0, result = 0; i < columns_num; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    for (int k = 0; k < N; k++)
                    {
                        if (N > 2)
                            result += key_Transpose[j, k] * cipher_text[k, i];
                        else
                            result += key_Inverse[j, k] * cipher_text[k, i];
                    }
                    plain_Text.Add(result % 26);
                    result = 0;
                }

            }

            // Check the correctness of key
            HillCipher algorithm = new HillCipher();
            List<int> cipher2 = algorithm.Encrypt(plain_Text, key);
            for (int i = 0; i < cipherText.Count; i++)
            {
                if(cipherText[i] != cipher2[i])
                {
                    throw new SystemException();
                }
            }
            return plain_Text;
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int N = (int)Math.Sqrt(key.Count);
            int[,] new_key = new int[N, N];

            // Create key matrix 
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    new_key[i, j] = key[i * N + j];
                }
            }

            // Create plain matrix 
            int columns_num = plainText.Count / N;
            int[,] new_plain = new int[N, columns_num];
            for (int i = 0; i < columns_num; i++)
            {
                for (int k = 0; k < N; k++)
                {
                    new_plain[k, i] = plainText[i * N + k];
                }
            }


            List<int> cipher = new List<int>();
            int result = 0;
            for (int i = 0; i < columns_num; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    for (int k = 0; k < N; k++)
                    {
                        result += new_key[j, k] * new_plain[k, i];
                    }
                    cipher.Add(result % 26);
                    result = 0;
                }

            }

            return cipher;
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            int N = 3;
            int columns_num = plainText.Count / N;

            // Convert the plain to matrix
            int[,] plain_Matrix = new int[N, columns_num], temp_Matrix = new int[N, columns_num];
            for (int i = 0; i < columns_num; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    plain_Matrix[i, j] = plainText[i * N + j];
                    temp_Matrix[i, j] = plainText[i * N + j];
                }
            }

            // Convert the cipher to matrix
            int[,] cipher_Matrix = new int[3, columns_num];
            for (int i = 0; i < columns_num; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    cipher_Matrix[i, j] = cipherText[i * N + j];
                }
            }


            // Calculate the det of key
            int det = (DeterminantOfMatrix(temp_Matrix, N) % 26 + 26) % 26;

            // Calculate new value of det
            int count = 1;
            while (count * det % 26 != 1)
                count++;

            det = count;

            // Calculate the inverse matrix of key
            int[,] plain_Inverse = new int[N, columns_num];
            for (int i = 0; i < columns_num; i++)
            {
                for (int j = 0; j < columns_num; j++)
                {
                    plain_Inverse[i, j] = (det * (int)Math.Pow(-1, i + j) * Calc_Smaller_Det(plain_Matrix, i, j) % 26 + 26) % 26;
                }
            }

            // Calculate the transpose matrix of key
            int[,] plain_Transpose = new int[columns_num, N];
            for (int i = 0; i < columns_num; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    plain_Transpose[i, j] = plain_Inverse[j, i];
                }
            }

            List<int> key = new List<int>();
            int result = 0;
            for (int i = 0; i < columns_num; i++)
            {
                for (int j = 0; j < columns_num; j++)
                {
                    for (int k = 0; k < N; k++)
                    {
                        result += plain_Transpose[j, k] * cipher_Matrix[k, i];
                    }
                    key.Add(result % 26);
                    result = 0;
                }

            }
            return key;
        }
    }
}