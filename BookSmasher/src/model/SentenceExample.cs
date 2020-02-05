using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classifier.src.model
{
    // TODO watch out for too many interfaces
    public class SentenceExample : IExample
    {
        // same order of words, punctuation
        public string sentence { get; set; }

        // when word or punctuation is added use lookup to get its index
        public List<int> wordIndexes { get; set; }

        // indicates position in paragraph, 1 is start, 2 is middle, 3 is end
        //public int paragraphPosition { get; set; }

        // classify as some type of sentence to see what you get
        public int classification { get; set; }

        // index for previous sentence classifcation
        public int prevSentenceClassification { get; set; }

        // scoring according to prevSentence
        // higher int is better scoring
        public int prevSentenceScore { get; set; }

        public SentenceExample()
        {
            wordIndexes = new List<int>();
        }
    }
}
