using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BookSmasher.src.machineLearning
{
    public class DecisionTree
    {
        private int _maxDepth = 0;
        private IStump _stump = null;

        public IStump splitModel = null;
        public DecisionTree subModel1 = null;
        public DecisionTree subModel0 = null;


        public DecisionTree(int depth, IStump stump)
        {
            _maxDepth = depth;
            _stump = stump;
        }

        // TODO all of these are going to need a return type of some sort
        public void Fit(List<List<int>> X, List<int> y)
        {// TODO watch out fr empty X in all of these cases
            int numExamples = X.Count;
            int numFeatures = numExamples != 0 ? X[0].Count : 0;

            // TODO this is wrong TODO TODO should be able to handle random too
            var model = (RandomStumpInfoGain) _stump;
            model.Fit(X,y);

            if (_maxDepth <= 1 || model.splitVariable == -1)
            {
                // either max depth reached or decision stump does nothing, so use the stump
                splitModel = model;
                subModel1 = null;
                subModel0 = null;

                // TODO fix 
                return;

            }

            var y1 = new List<int>();
            var y0 = new List<int>();

            for (int i = 0; i < numExamples; i++)
            {
                if (X[i][model.splitVariable] > model.splitValue)
                {
                    y1.Add(y[i]);
                } else
                {
                    y0.Add(y[i]);
                }
            }

            // Worried about all this casting
            splitModel = model;
            subModel1 = new DecisionTree(_maxDepth - 1, new RandomStumpInfoGain());
            subModel0 = new DecisionTree(_maxDepth - 1, new RandomStumpInfoGain());
            subModel1.Fit(X.Where(x => x[model.splitVariable] > model.splitValue).ToList(), y1);
            subModel0.Fit(X.Where(x => x[model.splitVariable] <= model.splitValue).ToList(), y0);

        }

        public List<int> Predict(List<List<int>> X)
        {
            int numExamples = X.Count;
            int numFeatures = numExamples != 0 ? X[0].Count : 0;

            var yhat = new int[numExamples];

            var model = (RandomStumpInfoGain)_stump;

            if (model.splitVariable == -1)
            {
                foreach (var i in yhat)
                {
                    yhat[i] = model.splitSat;
                }

            } else if (subModel1 == null)
            {
                return model.Predict(X);
            } else
            {
                var pred1 = subModel1.Predict(X.Where(x => x[model.splitVariable] > model.splitValue).ToList());
                var pred0 = subModel0.Predict(X.Where(x => x[model.splitVariable] <= model.splitValue).ToList());

                var pred1Index = 0;
                var pred0Index = 0;

                for (int i = 0; i < numExamples; i++)
                {
                    if (X[i][model.splitVariable] > model.splitValue)
                    {
                        yhat[i] = pred1[pred1Index];
                        pred1Index++;
                    }
                    else
                    {
                        yhat[i] = pred0[pred0Index];
                        pred0Index++;
                    }
                }
            }

            return yhat.ToList();

        }
    }
}
