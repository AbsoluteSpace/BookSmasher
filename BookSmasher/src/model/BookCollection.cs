using System.Collections.Generic;
using System.Text;

namespace BookSmasher.src.model
{
    // Collection of Books
    class BookCollection
    {
        // Name of collection is id1_id2_..._idN.
        public string id { get; set; }

        // Store ids of combined books.
        public List<string> ids { get; set; }

        // Combined bag of words associated with each book.
        public List<string> bagOfWords { get; set; }

        // All sentences.
        public List<SentenceExample> sentences { get; set; }

        // Examples and labels for training the model.
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

            // Names with whitespaces are hard.
            id = builder.ToString().TrimEnd().Replace(' ', '_');
        }
    }
}
