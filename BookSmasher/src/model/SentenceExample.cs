using System.Collections.Generic;

namespace BookSmasher.src.model
{
    // Stores sentences from books represented as feature vectors.
    public class SentenceExample
    {
        // Sentence with original format.
        public string sentence { get; set; }

        // Indices of words in sentence according to bagOfWords.
        public List<int> wordIndexes { get; set; }

        // TODO indicates position in paragraph.
        //public int paragraphPosition { get; set; }

        // Sentence type.
        public int classification { get; set; }

        // Previous sentence type.
        public int prevSentenceClassification { get; set; }

        // Types of sentences being compared against.
        public List<int> adjacentSentenceClassification { get; set; }

        // Scoring according to prevSentence, higher value is better score.
        public int prevSentenceScore { get; set; }

        public SentenceExample()
        {
            wordIndexes = new List<int>();
        }

        // TODO need way to get all features lined up right and stored
    }
}
