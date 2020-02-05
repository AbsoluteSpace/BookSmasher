using Classifier.src.machineLearning;
using Classifier.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classifier.src.controller
{
    // Contains high level methods for the project
    interface IInsightFacade
    {
        // add new book to the local storage of books
        // TODO add kind of book uploaded
        List<string> AddBook(string id, string content);

        // remove book from local storage of books
        List<string> RemoveBook(string id);

        // list books currently in local storage
        List<string> ListBooks();

        // train model for book combination in local storage
        IMLModel TrainModel(string id1, string id2);

        // generate new book from two provided books
        // TODO undecided params
        string GenerateBook(int maxDepth, int numTrees);
    }
}
