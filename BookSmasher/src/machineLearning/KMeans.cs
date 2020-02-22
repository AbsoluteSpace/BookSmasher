using BookSmasher.src.controller;
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

        public void Fit(List<List<int>> X)
        {
            // randomly select k points from X to be means.
            // store a collection of these means

            // go through each point and assign it to the closest mean by euclidian distance

            // update location of means by weight of points assigned to it

            // keep track of old mean assignments throughout and if under
            // a certain number of points changed means, stop (or after some number of iterations)


        }

        // calls fit but a number of times and determines the best one.
        public void FitWithRandomRestarts()
        {

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
