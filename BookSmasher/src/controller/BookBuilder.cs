using Classifier.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classifier.src.controller
{
    // Retrieve information from input files
    public class BookBuilder
    {
        // TODO should this be initialized in the constructor?
        //private List<string> _bagOfWords = new List<string>();

        public BookBuilder()
        {

        }

        public Book ConstructBook(string id, string content, List<string> bagOfWords)
        {
            // rip all of the sentences out of the book and shove in the examples for the book
            // 1. build example sentences properly classified
            // 2. each time an example is built, from top to bottom, add it into the book.

            // expanding on step 1, get words and punctuation, lookup words and stuff for index
            //, find posn in paragraph
            var lines = ParseLines(content, bagOfWords);

            // at very end classify each of these according to some classifier algorithm
            // argue which one
            //var clusterAlgo = new KMeans();
            // TODO big priority


            // set id of book
            var newBook = new Book();
            newBook.id = id;
            // TODO need to make general, working for now
            newBook.clusteredExamples = lines.Cast<SentenceExample>().ToList(); ;

            return newBook;
        }

        // TODO should have restrictions on what sentences cna contain otherwise security issue
        private List<SentenceExample> ParseLines(string content, List<string> bagOfWords)
        {
            var output = new List<SentenceExample>();

            // read all the lines from the file including whitespace
            string[] lines = System.IO.File.ReadAllLines(content);

            for (int i = 0; i < lines.Length; i++)
            {
                var se = new SentenceExample();

                // split this based on periods.
                // If more than one thing, then have to deal with it all
                // first would look at previous
                // second at first and so on
                // last would look at the next one

                // if only one thing then grab previous and next

                // current one that you're on
                // get indeices of words and punctuation used with hashset

                //se.paragraphPosition = ;

                // new plan, take line, split based on periods, see if something left over from prev line
                // if so add to the first split, and if sentence doesn't end with period then continue it for next
                // line


                //se.sentence = ;
                //se.wordIndexes = ;
            }

            return output;
        }

        public List<string> BuildBagOfWords(List<string> bagOfWords)
        {
            // can be passed in as empty
            // don't let there be duplicates


            // build bag of words representation for this book
            // go through each line and then add word into List if not already present
            // all have 0 as a value

            return null;

        }
    }
}
