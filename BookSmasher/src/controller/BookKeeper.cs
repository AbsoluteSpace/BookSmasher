using BookSmasher.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookSmasher.src.controller
{
    public class BookKeeper
    {
        public Book book1 { get; set; }
        public Book book2 { get; set; }

        public BookKeeper()
        {

        }

        // return filepath to the book -> for now just store book as variable
        public string CacheBook(Book book)
        {
            // just store book as above
            return null;
        }
    }
}
