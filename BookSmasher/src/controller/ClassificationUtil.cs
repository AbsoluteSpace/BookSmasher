﻿using BookSmasher.src.model;
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
                if (sentence.prevSentenceClassification != -1) {
                    retVal[i].Add(sentence.prevSentenceClassification);
                }
                // Add adjacentSentence classifications as features.
                if (sentence.adjacentSentenceClassification != null) {
                    retVal[i].AddRange(sentence.adjacentSentenceClassification);
                }
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

        // X is things that want euclidian distance to.
        // Output is list where each entry is list of euclidian distances of x point to xtest points. 
        public static List<List<double>> EuclidianDistance(List<List<double>> X, List<List<double>> XTest)
        {
            var retVal = new List<List<double>>();

            for (int i = 0; i < X.Count; i++)
            {
                var innerList = new List<double>();

                for (int j = 0; j < XTest.Count; j++)
                {
                    innerList.Add(Distance(X[i], XTest[j]));
                }

                retVal.Add(innerList);
            }

            return retVal;
        }


        public static double Distance(List<double> Xi, List<double> Xj)
        {
            double retVal = 0;

            for (int i = 0; i < Xi.Count; i++)
            {
                retVal += Math.Pow((Xi[i] - Xj[i]), 2);
            }

            return Math.Sqrt(retVal);
        }

        // Take in list of examples, and return list where each entry is an average feature from input.
        public static List<double> Average(List<List<double>> X)
        {
            var retVal = new List<double>();

            if (X.Count == 0)
            {
                return null;
            }

            for(int i = 0; i < X[0].Count; i++)
            {
                double sumOfVals = 0;

                foreach(var example in X)
                {
                    sumOfVals += example[i];
                }

                retVal.Add(sumOfVals / X.Count);
            }

            return retVal;
        }

    }
}
