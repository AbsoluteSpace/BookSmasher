using System;
using System.Collections.Generic;
using System.Linq;

namespace BookSmasher.src.machineLearning
{
    public class RandomStumpInfoGain : IStump
    {
        public int splitVariable = -1;
        public int splitValue = -1;
        public int splitSat = -1;
        public int splitNot = -1;

        public DecisionStumpInfoGain stump = null;

        public RandomStumpInfoGain()
        {

        }

        public void Fit(List<List<int>> X, List<int> y)
        {
            int numFeatures = X[0].Count;
            int k = (int) Math.Floor(Math.Sqrt(numFeatures));

            // better way: https://stackoverflow.com/a/17530353/10576762
            Random r = new Random();
            // TODO this looks like trouble
            int[] kRandom = Enumerable.Range(0, numFeatures).OrderBy(x => r.Next()).Take(k).ToArray();

            // TODO feel like I need to assign values to this to make it useful at all

            stump = new DecisionStumpInfoGain();
            stump.Fit(X, y, kRandom);

            splitVariable = stump.splitVariable;
            splitValue = stump.splitValue;
            splitSat = stump.splitSat;
            splitNot = stump.splitNot;

        }

        // TODO check that this isn't sketch
        public List<int> Predict(List<List<int>> X)
        {
            return stump.Predict(X);
        }
    }
}
