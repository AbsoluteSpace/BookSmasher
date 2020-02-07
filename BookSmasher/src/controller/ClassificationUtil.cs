using BookSmasher.src.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookSmasher.src.controller
{
    public static class ClassificationUtil
    {
        public static List<List<int>> ConstructMatrixX(List<string> bagOfWords, List<SentenceExample> examples)
        {
            var retVal = new List<List<int>>();

            // TODO will need to be more than bag of words at some point
            for (int i = 0; i < examples.Count; i++)
            {
                retVal.Add(new List<int>());

                // initialize with 0 by default for the correct length
                for (int j = 0; j < bagOfWords.Count; j++)
                {
                    retVal[i].Add(0);
                }

                var sentence = examples[i];
                // TODO check not off by 1
                foreach (var wordIdx in sentence.wordIndexes)
                {
                    retVal[i][wordIdx] = 1;
                }

                // add prevSentence classification as feature
                retVal[i].Add(examples[i].prevSentenceClassification);
                // add adjacentSentence classifications as features
                retVal[i].AddRange(examples[i].adjacentSentenceClassification);
            }

            return retVal;
        }

        public static List<List<int>> GenerateFakeX()
        {
            var output = new List<List<int>>();
            var rand = new Random();

            for (int i = 0; i < 200; i++)
            {
                var toAdd = new List<int>();
                for (int j = 0; j < 40; j++)
                {
                    toAdd.Add(rand.Next(0, 4));
                }
                output.Add(toAdd);
            }

            return output;
        }

        public static List<int> GenerateFakeY(List<List<int>> X)
        {
            var output = new List<int>();

            for (int i = 0; i < X.Count; i++)
            {
                if (X[i][4] == 1 || X[i][2] == 0)
                {
                    if (X[i][6] == 1 || X[i][5] == 0)
                    {
                        output.Add(1);
                    }
                    else
                    {
                        output.Add(3);
                    }
                }
                else
                {
                    output.Add(2);
                }
            }

            return output;
        }

    }
}
