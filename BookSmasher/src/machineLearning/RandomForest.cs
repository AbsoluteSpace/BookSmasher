using BookSmasher.src.machineLearning;
using System.Collections.Generic;
using System.Linq;

namespace Classifier.src.machineLearning
{
    // Build random forest model from data once it is labelled.
    public class RandomForest
    {
        private int _numTrees = 0;
        private int _maxDepth = 0;

        private List<RandomTree> _trees = null;

        public RandomForest(int maxDepth, int numTrees)
        {
            _numTrees = numTrees;
            _maxDepth = maxDepth;

            _trees = new List<RandomTree>();
        }

        public void Fit(List<List<int>> X, List<int> y)
        {
            for (int i = 0; i < _numTrees; i++)
            {
                var model = new RandomTree(_maxDepth);
                model.Fit(X, y);
                _trees.Add(model);
            }

        }

        public List<int> Predict(List<List<int>> X)
        {
            int numExamples = X.Count;

            var predictionsMode = new List<int>();
            var predictions = new List<List<int>>();

            for (int i = 0; i < _numTrees; i++)
            {
                predictions.Add(_trees[i].Predict(X));
            }

            for (int i = 0; i < numExamples; i++)
            {
                var toAdd = new List<int>();

                for (int j = 0; j < _numTrees; j++)
                {
                    toAdd.Add(predictions[j][i]);
                }

                predictionsMode.Add(ArrayUtil.FindMode(toAdd));
            }

            return predictionsMode;
        }

    }
}
