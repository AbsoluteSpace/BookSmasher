﻿using BookSmasher.src.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookSmasher.src.machineLearning
{
    // Cluster the input data. TODO.
    public class KMeans
    {
        private int _k;
        private List<List<double>> _means;

        public KMeans(int k)
        {
            _k = k;
            _means = new List<List<double>>();
        }

        // Fit the KMeans clusterer. Returns the score of this clustering.
        private double FitHelper(List<List<int>> X)
        {
            // duplicated code, but only twice, don't refactor yet
            var doublesList = new List<List<double>>();
            var rand = new Random();

            foreach (var xi in X)
            {
                doublesList.Add(xi.Select(x => (double)x).ToList());
            }

            for (int i = 0; i < _k; i++)
            {
                var idx = rand.Next(0, X.Count - 1);
                _means.Add(doublesList[idx]);
            }

            // track whether means are still updating
            var changes = true;

            var labels = new Dictionary<int, List<List<double>>>();

            while (changes)
            {
                for (int i = 0; i < _k; i++)
                {
                    labels[i] = new List<List<double>>();
                }

                // go through each point and assign it to the closest mean by euclidian distance
                var distances = ClassificationUtil.EuclidianDistance(doublesList, _means);

                for (int i = 0; i < distances.Count; i++)
                {
                    labels[distances[i].IndexOf(distances[i].Max())].Add(doublesList[i]);
                }

                // update location of means by weight of points assigned to it

                var prevMean = _means;

                for (int i = 0; i < _k; i++)
                {
                    _means[i] = ClassificationUtil.Average(labels[i]);
                }

                // keep track of old mean assignments throughout and if under
                // a certain number of points changed means, stop (or after some number of iterations)
                var firstNotSecond = prevMean.Except(_means).ToList();
                var secondNotFirst = _means.Except(prevMean).ToList();

                changes = !firstNotSecond.Any() && !secondNotFirst.Any();
            }

            double score = 0;

            // TODO rewrite -> it's unclear.
            // calculates a score using the distance of all points from their mean.
            foreach(var key in labels.Keys)
            {
                var doubleList = new List<List<double>>();
                doubleList.Add(_means[key]);
                score += ClassificationUtil.EuclidianDistance(labels[key], doubleList)[0].Sum();
            }

            return score;

        }

        // Calls fit but a number of times and determines the best one.
        // Determine best by what has lowest sum of distances from points to their means.
        // TODO: magic number, should be a hyperparameter maybe.
        public void Fit(List<List<int>> X, int numRandomRestarts = 20)
        {
            var bestMeansSoFar = new List<List<double>>();
            var bestScore = double.PositiveInfinity;

            for (int i = 0; i < numRandomRestarts; i++)
            {
                var newScore = FitHelper(X);

                if (newScore < bestScore)
                {
                    bestScore = newScore;
                    bestMeansSoFar = _means;
                }

            }

            _means = bestMeansSoFar;

        }

        public List<int> Predict(List<List<int>> X)
        {
            var retVal = new List<int>();

            var doublesList = new List<List<double>>();

            foreach(var xi in X)
            {
                doublesList.Add(xi.Select(i => (double)i).ToList());
            }

            var distances = ClassificationUtil.EuclidianDistance(doublesList, _means);

            foreach(var distance in distances)
            {
                retVal.Add(distance.IndexOf(distance.Max()));
            }

            return retVal;
        }


    }
}
