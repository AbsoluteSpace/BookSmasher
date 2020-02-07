using BookSmasher.src.model;
using System.Collections.Generic;

namespace BookSmasher.src.controller
{
    // Class to store books not globally like now. Unfinished.
    public class BookKeeper
    {
        public List<Book> books { get; set; }

        public BookKeeper()
        {
            books = new List<Book>();
        }

    }
}
