using Classifier.src.controller;
using System;

namespace BookSmasher
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO add hub for options other than just this set path when time
            // TODO generalize for more books
            // TODO write that id can't contain commas or something
            Console.WriteLine("Add book 1 with id,filepath");
            var book1Info = Console.ReadLine();
            Console.WriteLine("Add book 2 with id,filepath");
            var book2Info = Console.ReadLine();

            var book1Id = book1Info.Split(",")[0];
            var book1Content = book1Info.Split(",")[1];

            var book2Id = book2Info.Split(",")[0];
            var book2Content = book2Info.Split(",")[1];

            var insightFacade = new InsightFacade();
            insightFacade.AddBook(book1Id, book1Content);
            insightFacade.AddBook(book2Id, book2Content);

            insightFacade.TrainModel(book1Id, book2Id);

            // TODO fiddle with numbers
            insightFacade.GenerateBook(8, 20);


        }
    }
}
