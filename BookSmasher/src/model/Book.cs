using System.Collections.Generic;

namespace BookSmasher.src.model
{
    // Data structure to store sentence examples.
    public class Book
    {
        public string id { get; set; }

        // Order is guranteed, so can always recunstruct original book as .txt.
        public List<SentenceExample> sentences {get; set;}

        public Book()
        {
            sentences = new List<SentenceExample>();
        }

    }
}
