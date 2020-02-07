using System;
using System.Collections.Generic;
using System.Linq;

namespace BookSmasher.src.machineLearning
{
    public class RandomStumpInfoGain : IStump
    {
        public int splitVariable { get; set; }
        public int splitValue { get; set; }
        public int splitSat { get; set; }
        public int splitNot { get; set; }

        public DecisionStumpInfoGain stump = null;

        public RandomStumpInfoGain()
        {
            splitVariable = -1;
            splitValue = -1;
            splitSat = -1;
            splitNot = -1;
        }

        public void Fit(List<List<int>> X, List<int> y, int[] splitFeatures = null)
        {
            int numFeatures = X.Count != 0 ? X[0].Count : 0;
            int k = (int) Math.Floor(Math.Sqrt(numFeatures));

            // Better way: https://stackoverflow.com/a/17530353/10576762 .
            Random r = new Random();
            int[] kRandom = Enumerable.Range(0, numFeatures).OrderBy(x => r.Next()).Take(k).ToArray();

            stump = new DecisionStumpInfoGain();
            stump.Fit(X, y, kRandom);

            splitVariable = stump.splitVariable;
            splitValue = stump.splitValue;
            splitSat = stump.splitSat;
            splitNot = stump.splitNot;

        }

        public List<int> Predict(List<List<int>> X)
        {
            return stump.Predict(X);
        }
    }
}
