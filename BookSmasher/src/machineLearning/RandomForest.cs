using BookSmasher.src.machineLearning;
using System.Collections.Generic;
using System.Linq;

namespace Classifier.src.machineLearning
{
    // build random forest model from data once it is being labelled.
    public class RandomForest : IMLModel
    {
        private int _numTrees = 0;
        private int _maxDepth = 0;

        private List<RandomTree> _trees = null;

        public RandomForest(int maxDepth, int numTrees)
        {
            _numTrees = numTrees;
            _maxDepth = maxDepth;
        }

        public void Fit(List<List<int>> X, List<int> y)
        {
            var trees = new List<RandomTree>();

            for (int i = 0; i < _numTrees; i++)
            {
                var model = new RandomTree(_maxDepth);
                model.Fit(X, y);
                trees[i] = model;
            }

            _trees = trees;

        }

        public List<int> Predict(List<List<int>> X)
        {
            int numExamples = X.Count;

            var predictionsMode = new List<int>();
            var predictions = new List<List<int>>();

            for (int i = 0; i < _numTrees; i++)
            {
                predictions[i] = _trees[i].Predict(X);
            }

            for (int i = 0; i < numExamples; i++)
            {
                predictionsMode[i] = FindMode(predictions[i]);
            }

            return predictionsMode;
        }

        // TODO move, thi isn't the right place for it
        public int FindMode(List<int> x)
        {
            return x.GroupBy(v => v)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
        }
    }
}
