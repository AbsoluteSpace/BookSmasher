using System;
using System.Collections.Generic;
using System.Text;

namespace BookSmasher.src.model
{
    // Collection of Books
    class BookCollection
    {
        // store ids of combined books
        public List<string> ids { get; set; }

        // combined bag of words associated with each book
        public List<string> bagOfWords { get; set; }

        // all sentences
        public List<SentenceExample> sentences { get; set; }

        // examples to training model. Item1 is example, item 2 is its label
        // TODO best to keep them like this or seperate?
        // TODO name tuple
        public List<Tuple<SentenceExample, int>> trainingExamples { get; set; }

        public BookCollection(List<string> ids)
        {
            this.ids = ids;
            sentences = new List<SentenceExample>();
            trainingExamples = new List<Tuple<SentenceExample, int>>();
        }
    }
}
