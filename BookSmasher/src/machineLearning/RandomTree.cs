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
            int numFeatures = X[0].Count;

            var bootstrapX = new List<List<int>>();
            var bootstrapY = new List<int>();

            // fill up array with random examples with replacement
            var rand = new Random();
            for (int i = 0; i < numFeatures; i++)
            {
                var idx = rand.Next(0, numFeatures);
                bootstrapX[idx] = X[idx];
                bootstrapY[idx] = y[idx];
            }

            DecisionTreeRoot.Fit(bootstrapX, bootstrapY);
        }

        public List<int> Predict(List<List<int>> X)
        {
            return DecisionTreeRoot.Predict(X);
        }
    }
}
