using System;
using System.Linq;
using System.Text;

namespace BookSmasher.src.controller
{
    // Displays information on the console
    public class Renderer
    {
        // TODO things that are hyperparameters. Undecided on how to set (CV).
        private int _maxDepth = 8;
        private int _numTrees = 20;
        private int _numAdjacentExamples = 4;
        private int _numExamplesToClassify = 8;

        // Display console information for adding a new book.
        public void RenderAddBook(InsightFacade insightFacade)
        {
            Console.WriteLine("\nAdd book. Format: id,filepath");
            var bookInfo = Console.ReadLine().Split(",");

            if (bookInfo.Length != 2)
            {
                Console.WriteLine($"Book not added. id and filepath must be seperated by a comma, and neither id nor filepath may contain a comma");
                return;
            }

            var id = bookInfo[0].Trim();
            var content = bookInfo[1].Trim();

            try
            {
                var addedBookIds = insightFacade.AddBook(id, content);
                Console.WriteLine($"Book {id} added.");
            } catch(Exception e)
            {
                Console.WriteLine($"Book {id} not added. " + e.Message);
            }
        }

        // Display console information for removing a book.
        public void RenderRemoveBook(InsightFacade insightFacade)
        {
            Console.WriteLine("\nRemove book. Format: id");
            var id = Console.ReadLine().Trim();

            try
            {
                var addedBookIds = insightFacade.RemoveBook(id);
                Console.WriteLine($"Book {id} removed.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Book {id} not removed. " + e.Message);
                return;
            }
        }

        // Display console information for listing added books.
        public void RenderListBook(InsightFacade insightFacade)
        {
            var addedBooks = insightFacade.ListBooks();
            var idString = new StringBuilder();

            for(int i = 0; i < addedBooks.Count - 1; i++)
            {
                idString.Append(addedBooks[i] + ", ");
            }

            if (addedBooks.Count == 0)
            {
                Console.WriteLine("\nNo books added.");
                return;
            }

            idString.Append(addedBooks[addedBooks.Count - 1]);

            Console.WriteLine("\nAdded book ids: " + idString.ToString());
        }

        // Display console information for training.
        public void RenderTrainBook(InsightFacade insightFacade)
        {
            Console.WriteLine("\nInput the ids of books to be combined, books must already be added, min 2. Format: id1,id2,...,idN");
            // TODO add link to see currently added ids

            var ids = Console.ReadLine().Split(",");
            var addedIds = insightFacade.ListBooks();

            foreach (var id in ids) {
                if (!IdHelper.IdAlreadyAdded(id, addedIds) || !IdHelper.IsValid(id))
                {
                    Console.WriteLine($"Aborting, id: {id} is invalid.");
                    return;
                }
            }

            Console.WriteLine($"\nA sentence will be displayed, followed by {_numAdjacentExamples} other sentences." +
                $" Rank the {_numAdjacentExamples} from best to worst as the next sentence. Right is best, left is worst.\n");

            try
            {
                insightFacade.TrainModel(ids.ToList(), _numExamplesToClassify, _numAdjacentExamples);
                Console.WriteLine("Model trained.");
            } catch (Exception e)
            {
                Console.WriteLine("Model not trained. " + e.Message);
                return;
            }

        }

        // Display console information for generating a new book.
        public void RenderGenerateBook(InsightFacade insightFacade)
        {
            Console.WriteLine("\nGenerate book after two or more books are trained together.");

            var trainedCollections = RenderListBookCollectionids(insightFacade);
            if (trainedCollections == null)
            {
                Console.WriteLine("\nNo bookCollections added.");
                return;
            }

            Console.WriteLine("\nThe following bookCollections are trained:" + trainedCollections);
            Console.WriteLine("\nWrite the id of the collection you wish to generate a book for. Format: id");

            var id = Console.ReadLine().Trim();
            var addedColIds = insightFacade.ListBookCollectionNames();

            if (!IdHelper.IdAlreadyAdded(id, addedColIds) || !IdHelper.IsValid(id))
            {
                Console.WriteLine($"Aborting, id: {id} is invalid.");
                return;
            }

            try
            {
                var outputLocation = insightFacade.GenerateBook(id, _maxDepth, _numTrees, _numAdjacentExamples);
                Console.WriteLine($"New book generated at {outputLocation}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"New book not generated. " + e.Message);
                return;
            }
        }

        // Display console information for listing book collections. Helper for now.
        private string RenderListBookCollectionids(InsightFacade insightFacade)
        {
            var addedBookCollections = insightFacade.ListBookCollectionNames();
            var idString = new StringBuilder();

            for (int i = 0; i < addedBookCollections.Count - 1; i++)
            {
                idString.Append(addedBookCollections[i] + ", ");
            }

            if (addedBookCollections.Count == 0)
            {
                return null;
            }

            return idString.Append(addedBookCollections[addedBookCollections.Count - 1]).ToString();
        }
    }
}
