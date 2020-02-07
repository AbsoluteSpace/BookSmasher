using System.Collections.Generic;

namespace BookSmasher.src.machineLearning
{
    // Interface for stumps.
    public interface IStump
    {
        int splitVariable { get; set; }
        int splitValue { get; set; }
        int splitSat { get; set; }
        int splitNot { get; set; }

        void Fit(List<List<int>> X, List<int> y, int[] splitFeatures = null);
        List<int> Predict(List<List<int>> X);
    }
}
