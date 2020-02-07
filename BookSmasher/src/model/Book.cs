using System.Collections.Generic;

namespace BookSmasher.src.model
{
    public class Book : IBook
    {
        // TODO look at accessability levels
        // TODO think about how it's stored, this is wasteful
        public string id { get; set; }

        // order is guranteed, so can always recunstruct original book as txt
        public List<SentenceExample> sentences {get; set;}

        public Book()
        {
            sentences = new List<SentenceExample>();
        }

    }
}
