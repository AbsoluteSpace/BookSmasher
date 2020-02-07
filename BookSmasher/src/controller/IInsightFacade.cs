using System.Collections.Generic;

namespace BookSmasher.src.controller
{
    // Contains high level methods.
    interface IInsightFacade
    {
        // Add new book.
        List<string> AddBook(string id, string content);

        // Remove book.
        List<string> RemoveBook(string id);

        // List added books.
        List<string> ListBooks();

        // List added book collections.
        List<string> ListBookCollectionNames();

        // Remove book collection.
        List<string> RemoveBookCollection(string id);

        // Train model based on some combination of added books.
        // numExamplesToClassify and numAdjacentExamples are hyperparameters for number training examples.
        void TrainModel(List<string> ids, int numExamplesToClassify, int numAdjacentExamples);

        // Generate new book from provided book collection.
        string GenerateBook(string id, int maxDepth, int numTrees, int numAdjacentExamples);
    }
}
