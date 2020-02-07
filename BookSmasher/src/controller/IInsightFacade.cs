using System.Collections.Generic;

namespace BookSmasher.src.controller
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
        void TrainModel(List<string> ids);

        // generate new book from two provided books
        // TODO undecided params
        string GenerateBook(string id1, string id2, int maxDepth, int numTrees);
    }
}
