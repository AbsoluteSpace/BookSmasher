using System;
using System.Collections.Generic;
using System.Text;

namespace BookSmasher.src.machineLearning
{
    public interface IStump
    {
        void Fit(List<List<int>> X, List<int> y);
        List<int> Predict(List<List<int>> X);
    }
}
