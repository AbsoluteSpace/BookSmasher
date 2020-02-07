using System;
using System.Collections.Generic;
using System.Text;

namespace BookSmasher.src.model
{
    // Collection of Books
    class BookCollection
    {
        // name of collection is ids
        // TODO customize collection name
        public string id { get; set; }

        // store ids of combined books
        public List<string> ids { get; set; }

        // combined bag of words associated with each book
        public List<string> bagOfWords { get; set; }

        // all sentences
        public List<SentenceExample> sentences { get; set; }

        // examples for training the model
        public List<SentenceExample> trainingExamples { get; set; }
        public List<int> trainingLabels { get; set; }

        public BookCollection(List<string> ids)
        {
            this.ids = ids;
            sentences = new List<SentenceExample>();
            trainingExamples = new List<SentenceExample>();
            trainingLabels = new List<int>();

            var builder = new StringBuilder();

            foreach(var id in ids)
            {
                builder.Append(id + " ");
            }

            // names with whitespaces are hard
            id = builder.ToString().TrimEnd().Replace(' ', '_');
        }
    }
}
