﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        // call after fit
        public List<int> Predict(List<List<int>> X)
        {
            int numExamples = X.Count;
            int numFeatures = numExamples != 0 ? X[0].Count : 0; // all elements should be the same size TODO THIS WILL PROBABLY BE PROB

            var yhat = new int[numExamples];
            foreach(var i in yhat)
            {
                yhat[i] = 1;
            }

            if (splitVariable == -1)
            {
                foreach (var i in yhat)
                {
                    yhat[i] = splitSat;
                }

                return yhat.ToList();
            }

            for(int i = 0; i < numExamples; i++)
            {
                if (X[i][splitVariable] > splitValue)
                {
                    yhat[i] = splitSat;
                } else
                {
                    yhat[i] = splitNot;
                }
            }

            return yhat.ToList();
        }

        // TODO this is wack and needs to go:
        public void Fit(List<List<int>> X, List<int> y)
        {
            Fit(X, y, null);
        }

        // TODO want to change this to not be list of list
        // splitFeatures is an array of indexes of columns of features to split on
        public void Fit(List<List<int>> X, List<int> y, int[] splitFeatures = null)
        {
            int numExamples = X.Count;
            int numFeatures = numExamples != 0 ? X[0].Count : 0; // all elements should be the same size TODO THIS WILL PROBABLY BE PROB

            if (numExamples == 0 || numFeatures == 0)
            {
                return;
            }

            // TODO improve
            int[] count = new int[y.Max() + 1]; // all elements to 0 by default
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

            // TODO maybe move this up before entropy check
            if (y.Distinct().Count() <= 1)
            {
                return;
            }

            // TODO question if everything is right choice
            if (splitFeatures == null)
            {
                splitFeatures = new int[numFeatures];
            }

            foreach(var d in splitFeatures)
            {
                // need all of the examples at only the current feature d
                // in an array of the unique sorted values

                // check that is np.unique(X[:,d])
                var distinctThresholds = X.Select(x => x[d]).Distinct();
                var thresholds = distinctThresholds.OrderBy(o => o).ToList(); // low to high?

                // should I remove the alst element of thresholds TODO
                foreach(var val in thresholds)
                {
                    // indices where feature > threshold is their label otherwise 0
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

                    var prob1 = X.Where(x => x[d] > val).Select(x => x[d]).Sum() / numExamples;
                    var prob0 = 1 - prob1;

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

            // TODO how does this handle not going int o loops and assigning anything?
            // should veen have to worry?

            // stuff here, which I'm not convinced does anything
            //_splitVariable = d;
            //_splitValue = val;
            //_splitSat = GetArgMax(count1);
            //_splitNot = GetArgMax(count0);

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
            // might be able to do this with a sum
            double entropy = 0;
            foreach (var prob in vector)
            {
                entropy -= prob != 0 ? prob * Math.Log(prob) : 0;
            }

            return entropy; // TODO could be problem here, double check
        }

        public int GetArgMax(int[] count)
        {
            // TODO not great
            return count.ToList().IndexOf(count.Max());
        }

    }
}
