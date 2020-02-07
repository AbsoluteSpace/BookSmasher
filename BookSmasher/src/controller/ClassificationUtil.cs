using BookSmasher.src.model;
using System;
using System.Collections.Generic;

namespace BookSmasher.src.controller
{
    // Useful tools to help with classification.
    public static class ClassificationUtil
    {
        // Construct feature matrix using examples.
        public static List<List<int>> ConstructMatrixX(List<string> bagOfWords, List<SentenceExample> examples)
        {
            var retVal = new List<List<int>>();

            // TODO will need to be more than bag of words at some point
            for (int i = 0; i < examples.Count; i++)
            {
                retVal.Add(new List<int>());
                var sentence = examples[i];

                // Add words within the sentence as features.
                for (int j = 0; j < bagOfWords.Count; j++)
                {
                    var valToAdd = sentence.wordIndexes.Contains(j) ? 1 : 0;
                    retVal[i].Add(valToAdd);
                }

                // Add prevSentence classification as feature.
                retVal[i].Add(sentence.prevSentenceClassification);
                // Add adjacentSentence classifications as features.
                retVal[i].AddRange(sentence.adjacentSentenceClassification);
            }

            return retVal;
        }

        // Generate a fake feature matrix (filled with random vals) with input size.
        public static List<List<int>> GenerateFakeX(int numExamples, int numFeatures)
        {
            var output = new List<List<int>>();
            var rand = new Random();

            for (int i = 0; i < numExamples; i++)
            {
                var toAdd = new List<int>();
                for (int j = 0; j < numFeatures; j++)
                {
                    toAdd.Add(rand.Next(0, 4));
                }
                output.Add(toAdd);
            }

            return output;
        }

        // Generate random fake labels corresponding to a feature matrix.
        public static List<int> GenerateFakeY(List<List<int>> X)
        {
            var output = new List<int>();
            var numFeatures = X[0].Count;
            var rand = new Random();

            var firstSplit = rand.Next(0, numFeatures - 1);
            var secondSplit = rand.Next(0, numFeatures - 1);

            for (int i = 0; i < X.Count; i++)
            {
                if (X[i][firstSplit] == 1)
                {
                    var valToAdd = X[i][secondSplit] == 0 ? 1 : 3;
                    output.Add(valToAdd);
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
