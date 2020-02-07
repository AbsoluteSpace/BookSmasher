using System;
using System.Collections.Generic;
using System.Linq;

namespace BookSmasher.src.machineLearning
{
    public class DecisionStumpInfoGain : IStump
    {
        public int splitVariable { get; set; }
        public int splitValue { get; set; }
        public int splitSat { get; set; }
        public int splitNot { get; set; }

        public DecisionStumpInfoGain()
        {
            splitVariable = -1;
            splitValue = -1;
            splitSat = -1;
            splitNot = -1;
        }

        public List<int> Predict(List<List<int>> X)
        {
            int numExamples = X.Count;
            var yhat = new int[numExamples];

            if (splitVariable == -1)
            {
                return yhat.Select(x => splitSat).ToList();
            }

            for (int i = 0; i < numExamples; i++)
            {
                yhat[i] = X[i][splitVariable] > splitValue ? splitSat : splitNot;
            }

            return yhat.ToList();
        }

        public void Fit(List<List<int>> X, List<int> y, int[] splitFeatures = null)
        {
            int numExamples = X.Count;
            int numFeatures = numExamples != 0 ? X[0].Count : 0;

            if (numExamples == 0 || numFeatures == 0)
            {
                return;
            }

            int[] count = new int[y.Max() + 1];
            foreach (var i in y)
            {
                count[i]++;
            }

            var totalEntropy = CalculateEntropy(count);

            double maxGain = 0;

            splitVariable = -1;
            splitValue = -1;
            splitSat = GetArgMax(count);
            splitNot = -1;

            if (y.Distinct().Count() <= 1)
            {
                return;
            }

            if (splitFeatures == null)
            {
                splitFeatures = new int[numFeatures];
                for (int i = 0; i < numFeatures; i++)
                {
                    splitFeatures[i] = i;
                }
            }

            foreach(var d in splitFeatures)
            {
                var distinctThresholds = X.Select(x => x[d]).Distinct();
                var thresholds = distinctThresholds.OrderBy(o => o).ToList();

                foreach(var val in thresholds)
                {
                    var y1_vals = new List<int>();
                    var y0_vals = new List<int>();
                    for (int i = 0; i < X.Count; i++)
                    {
                        if (X[i][d] > val)
                        {
                            y1_vals.Add(y[i]);
                        } else
                        {
                            y0_vals.Add(y[i]);
                        }
                    }

                    var count1 = new int[count.Length];
                    var count0 = new int[count.Length];

                    foreach (var i in y1_vals)
                    {
                        count1[i]++;
                    }

                    foreach (var i in y0_vals)
                    {
                        count0[i]++;
                    }

                    var entropy1 = CalculateEntropy(count1);
                    var entropy0 = CalculateEntropy(count0);

                    double prob1 = ((double) X.Where(x => x[d] > val).Count()) / numExamples;
                    double prob0 = 1 - prob1;

                    var infoGain = totalEntropy - prob1 * entropy1 - prob0 * entropy0;

                    if (infoGain > maxGain)
                    {
                        maxGain = infoGain;

                        splitVariable = d;
                        splitValue = val;
                        splitSat = GetArgMax(count1);
                        splitNot = GetArgMax(count0);
                    }
                }

            }

        }

        public double CalculateEntropy(int[] count)
        {
            var totalCount = count.Sum();

            if (totalCount == 0)
            {
                return 0;
            }

            var weightedCounts = count.Select(x => (double) x / totalCount).ToList();
            return Entropy(weightedCounts);
        }

        public double Entropy(List<double> vector)
        {
            double entropy = 0;
            foreach (var prob in vector)
            {
                entropy -= prob != 0 ? prob * Math.Log(prob) : 0;
            }

            return entropy;
        }

        public int GetArgMax(int[] count)
        {
            return count.ToList().IndexOf(count.Max());
        }

    }
}
