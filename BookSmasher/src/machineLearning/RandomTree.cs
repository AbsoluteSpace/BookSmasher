using System;
using System.Collections.Generic;

namespace BookSmasher.src.machineLearning
{
    public class RandomTree
    {
        public DecisionTree DecisionTreeRoot = null;

        public RandomTree(int maxDepth)
        {
            DecisionTreeRoot = new DecisionTree(maxDepth, new RandomStumpInfoGain());
        }

        public void Fit(List<List<int>> X, List<int> y)
        {
            // TODO this is duplicated
            int numExamples = X.Count;

            var bootstrapX = new List<List<int>>();
            var bootstrapY = new List<int>();

            // fill up array with random examples with replacement
            var rand = new Random();
            for (int i = 0; i < numExamples; i++)
            {
                var idx = rand.Next(0, numExamples);
                bootstrapX.Add(X[idx]);
                bootstrapY.Add(y[idx]);
            }

            DecisionTreeRoot.Fit(bootstrapX, bootstrapY);
            var cat = 0;
        }

        public List<int> Predict(List<List<int>> X)
        {
            return DecisionTreeRoot.Predict(X);
        }
    }
}
