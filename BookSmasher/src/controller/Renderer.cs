using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSmasher.src.controller
{
    // Communicate with the razor files to display text and information
    public class Renderer
    {
        public void RenderAddBook(InsightFacade insightFacade)
        {
            Console.WriteLine("\nAdd book. Format: id,filepath");
            var bookInfo = Console.ReadLine().Split(",");

            // TODO think about other error cases
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

            // TODO magic number that needs to go
            Console.WriteLine("\nA sentence will be displayed, followed by 4 other sentences. Rank the 4 from best to worst as the next sentence left to right.\n");

            try
            {
                insightFacade.TrainModel(ids.ToList());
                Console.WriteLine("Model trained.");
            } catch (Exception e)
            {
                Console.WriteLine("Model not trained. " + e.Message);
                return;
            }

        }

        public void RenderGenerateBook(InsightFacade insightFacade)
        {

        }
    }
}
