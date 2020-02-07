using System.Collections.Generic;
using System.Linq;

namespace BookSmasher.src.machineLearning
{
    public class DecisionTree
    {
        private int _maxDepth = 0;
        private IStump _stump = null;

        public DecisionTree subModel1 = null;
        public DecisionTree subModel0 = null;


        public DecisionTree(int depth, IStump stump)
        {
            _maxDepth = depth;
            _stump = stump;
        }

        public void Fit(List<List<int>> X, List<int> y)
        {
            int numExamples = X.Count;
            int numFeatures = numExamples != 0 ? X[0].Count : 0;

            _stump.Fit(X,y);

            if (_maxDepth <= 1 || _stump.splitVariable == -1)
            {
                subModel1 = null;
                subModel0 = null;
                return;

            }

            var y1 = new List<int>();
            var y0 = new List<int>();

            for (int i = 0; i < numExamples; i++)
            {
                if (X[i][_stump.splitVariable] > _stump.splitValue)
                {
                    y1.Add(y[i]);
                } else
                {
                    y0.Add(y[i]);
                }
            }

            // TODO fix this to add back in randomness.
            if (y1.Count != 0) {
                subModel1 = new DecisionTree(_maxDepth - 1, new DecisionStumpInfoGain());
                subModel1.Fit(X.Where(x => x[_stump.splitVariable] > _stump.splitValue).ToList(), y1);
            } else
            {
                subModel1 = null;
            }

            if (y0.Count != 0)
            {
                subModel0 = new DecisionTree(_maxDepth - 1, new DecisionStumpInfoGain());
                subModel0.Fit(X.Where(x => x[_stump.splitVariable] <= _stump.splitValue).ToList(), y0);
            }
            else
            {
                subModel0 = null;
            }

        }

        public List<int> Predict(List<List<int>> X)
        {
            int numExamples = X.Count;
            int numFeatures = numExamples != 0 ? X[0].Count : 0;

            var yhat = new List<int>();

            if (_stump == null || _stump.splitVariable == -1)
            {
                for (int i = 0; i < numExamples; i++)
                {
                    yhat.Add(_stump.splitSat);
                }
            }
            else if (subModel1 == null)
            {
                yhat = _stump.Predict(X);
            }
            else
            {
                var pred1 = subModel1.Predict(X.Where(x => x[_stump.splitVariable] > _stump.splitValue).ToList());
                var pred0 = subModel0.Predict(X.Where(x => x[_stump.splitVariable] <= _stump.splitValue).ToList());

                var pred1Index = 0;
                var pred0Index = 0;

                for (int i = 0; i < numExamples; i++)
                {
                    if (X[i][_stump.splitVariable] > _stump.splitValue)
                    {
                        yhat.Add(pred1[pred1Index]);
                        pred1Index++;
                    }
                    else
                    {
                        yhat.Add(pred0[pred0Index]);
                        pred0Index++;
                    }
                }
            }

            return yhat.ToList();

        }
    }
}
