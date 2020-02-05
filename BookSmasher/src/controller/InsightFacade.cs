using System;
using System.Collections.Generic;
using Classifier.src.machineLearning;
using Classifier.src.model;

namespace Classifier.src.controller
{
    public class InsightFacade : IInsightFacade
    {
        private List<string> _bookIds = new List<string>();
        private List<Book> _books = new List<Book>();

        // bag of words stored here, not great choice, but should be easy to fix
        private List<string> _bagOfWords = new List<string>();

        public List<string> AddBook(string id, string content)
        {
            // TODO do file check in different area
            if (content == null)
            {
                throw new InvalidOperationException("Id and content must be provided to add a book.");
            }
            // if else stuff for now here, don't want it -> need like polymoprhism or something
            if (!content.EndsWith(".txt"))
            {
                throw new NotSupportedException("This file format isn't accepted.");
            }

            if (IdHelper.IdAlreadyAdded(id) || !IdHelper.IsValid(id))
            {
                throw new InvalidOperationException("Id is invalid.");
            }

            var bookBuilder = new BookBuilder(_bagOfWords);
            var newBook = bookBuilder.ConstructBook(id, content);
            _bagOfWords = bookBuilder.bagOfWords;

            // TODO improve storage of books -> caching?
            //var bookKeeper = new BookKeeper();
            //bookKeeper.CacheBook(newBook);
            _books.Add(newBook);
            _bookIds.Add(id);

            return _bookIds;
        }

        // TODO generaize this
        public string GenerateBook(int maxDepth, int numTrees)
        {
            // train model with training data from both books -> random forest
            // TODO currently books store training ex without reference to which book they're combined with

            // TODO named tuples
            // TODO watch if duplicates are stored between the two books -> for now store in just book1
            var X_train = _books[0].ConstructTrainingX(_bagOfWords);
            var y = _books[0].classifiedExamples.Item2;
            
            var model = new RandomForest(maxDepth, numTrees);
            model.Fit(X_train, y);

            var X = _books[0].ConstructTestX(_bagOfWords);

            var predictedExamples = model.Predict(X);

            // now print

            // feed first sentence of random book to this as first line
            // then grab random 5 sentences from both books and pick one with best score as next sentence
            // repeat this process until either set number of sentences or end of sentences

            // just print these to the console
            // or into txt file

            throw new NotImplementedException();
        }

        public List<string> ListBooks()
        {
            return _bookIds;
        }

        public List<string> RemoveBook(string id)
        {
            if (!IdHelper.IdAlreadyAdded(id) || !IdHelper.IsValid(id))
            {
                throw new InvalidOperationException("Id is invalid.");
            }

            // TODO remove from local storage with BookKeeper

            _bookIds.RemoveAt(_bookIds.FindIndex(x => x.Equals(id)));
            return _bookIds;
        }

        public IMLModel TrainModel(string id1, string id2)
        {
            // need both books so error check for that

            // take ids and grabsome number of classified examples from each book, and then display one,
            // and rank the best number of sentences to follow in order that type of sentence

            // add this in as a collection of trained exampels for the book
            // then when the book is generated use these to build a model for the book first
            // TODO renamed classified examples, they're clustered not classified

            // algo to build trained ex
            // grab 1 sentence example of each classification type randomly from between both books
            // grab 2 examples of each classification type from each book or something
            // display one example from fisrt set and 1 of each classification type from other set
            // rank the examples,
            // repeat this for all of first set

            // represent ordering by what book came first (as a feature) and thne ranking how this cluster was ordered
            // against that


            // console app to do this
            // display set 1 sentence
            // then number and display other sentences
            // scale with number of sentences displayed so it's not all clusters -> paginate
            // show how response should be formatted
            // store new trained exampels with this response info

            // then shoulkd have clusters^2 number of training examples to work with

            // save trained examples int books
            throw new NotImplementedException();
        }
    }
}
