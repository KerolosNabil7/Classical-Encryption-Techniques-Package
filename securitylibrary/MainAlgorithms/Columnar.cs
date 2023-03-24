using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {

            SortedDictionary<int, int> table = new SortedDictionary<int, int>();
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            for (int i = 1; i < int.MaxValue; i++)
            {
                int plainTextPtr = 0, row = i, column = (int)Math.Ceiling((double)plainText.Length / i); ;
                string[,] plainTextMatrix = new string[column, row];
                for (int j = 0; j < column; j++)
                {
                    for (int k = 0; k < i; k++)
                    {
                        if (plainTextPtr >= plainText.Length)
                        {
                            plainTextMatrix[j, k] = "";
                        }
                        else
                        {
                            plainTextMatrix[j, k] = plainText[plainTextPtr].ToString();
                            plainTextPtr++;
                        }
                    }
                }

                List<string> substringsList = new List<string>();
                for (int j = 0; j < i; j++)
                {
                    string word = "";
                    for (int k = 0; k < column; k++)
                    {
                        word += plainTextMatrix[k, j];
                    }
                    substringsList.Add(word);
                }



                bool correctkey = false;
                string cipherCopy = cipherText;
                int indexOfSubstring = 0;

                for (int j = 0; j < substringsList.Count; j++)
                {
                    indexOfSubstring = cipherCopy.IndexOf(substringsList[j]);
                    if (indexOfSubstring != -1)
                    {
                        table.Add(indexOfSubstring, j + 1);
                        cipherCopy.Replace(substringsList[j], "_");
                    }
                    else
                        correctkey = true;
                }
                if (!correctkey)
                    break;

            }
            List<int> answer = new List<int>();
            Dictionary<int, int> answerTable = new Dictionary<int, int>();

            for (int i = 0; i < table.Count; i++)
                answerTable.Add(table.ElementAt(i).Value, i + 1);

            for (int i = 1; i <= answerTable.Count; i++)
                answer.Add(answerTable[i]);

            return answer;

        }

        public string Decrypt(string cipherText, List<int> key)
        {

            List<KeyValuePair<int, int>> orderOfCol = new List<KeyValuePair<int, int>>();
            string plainText = "";
            var textMatrix = ConstructDectryptionMatrix(cipherText, key, ref orderOfCol);
            for (int i = 0; i < textMatrix.Count; i++)
            {
                for (int j = 0; j < textMatrix[0].Count; j++)
                {
                    if (textMatrix[i][j] != '_')
                        plainText += textMatrix[i][j];
                }
            }
            plainText = plainText.ToLower();
            return plainText;
        }

        public string Encrypt(string plainText, List<int> key)
        {
            List<KeyValuePair<int, int>> orderOfCol = new List<KeyValuePair<int, int>>();
            var textMatrix = ConstructEnctryptionMatrix(plainText, key, ref orderOfCol);
            int currCol = 0;
            string cipherText = "";
            var someList = orderOfCol.OrderBy(x => x.Key).ToList();
            for (int i = 0; i < key.Count(); i++)
            {
                currCol = someList.ElementAt(i).Value;
                for (int j = 0; j < textMatrix.Count; j++)
                {
                    if (textMatrix.ElementAt(j).ElementAt(currCol) != '_')
                        cipherText += textMatrix.ElementAt(j).ElementAt(currCol);
                }
            }
            cipherText = cipherText.ToUpper();
            return cipherText;


        }
        private List<List<char>> ConstructEnctryptionMatrix(string text, List<int> key, ref List<KeyValuePair<int, int>> orderOfCol)
        {
            int col = key.Count(), row = (int)Math.Ceiling((double)text.Length / col), textPtr = 0;
            var textMatrix = new List<List<char>>();

            for (int i = 0; i < key.Count; i++)
                orderOfCol.Add(new KeyValuePair<int, int>(key[i] - 1, i));

            for (int i = 0; i < row; i++)
            {
                textMatrix.Add(new List<char>());
                for (int j = 0; j < col; j++)
                {
                    if (textPtr < text.Length)
                        textMatrix.ElementAt(i).Add(text.ElementAt(textPtr++));
                    else
                        textMatrix.ElementAt(i).Add('_');

                }
            }
            return textMatrix;
        }

        private List<List<char>> ConstructDectryptionMatrix(string text, List<int> key, ref List<KeyValuePair<int, int>> orderOfCol)
        {
            int col = key.Count(), row = (int)Math.Ceiling((double)text.Length / col), textPtr = 0;
            var textMatrix = new List<List<char>>();


            for (int i = 0; i < key.Count; i++)
                orderOfCol.Add(new KeyValuePair<int, int>(i, key[i] - 1));

            var someList = orderOfCol.OrderBy(x => x.Value).ToList();
            for (int i = 0; i < row; i++)
            {
                textMatrix.Add(new List<char>());
                for (int j = 0; j < col; j++)
                {
                    textMatrix.ElementAt(i).Add('_');
                }
            }

            int currCol = 0;
            for (int i = 0; i < someList.Count; i++)
            {
                currCol = someList.ElementAt(i).Key;
                for (int j = 0; j < row; j++)
                {
                    if (textPtr < text.Length)
                        textMatrix[j][currCol] = text.ElementAt(textPtr++);
                    else
                        textMatrix[j][currCol] = '_';
                }
            }
            return textMatrix;


        }


    }
}
