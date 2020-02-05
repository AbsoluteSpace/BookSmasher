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
            Console.WriteLine("Add book 1. Format: id,filepath");

            var book1Info = Console.ReadLine().Split(",");

            if (book1Info.Length != 2)
            {
                throw new Exception("id and filepath must be seperated by a comma and neither may contain a comma");
            }

            //Console.WriteLine("Add book 2 with id,filepath");
            //var book2Info = Console.ReadLine();

            //var book2Id = book2Info.Split(",")[0];
            //var book2Content = book2Info.Split(",")[1];

            // TODO formatting class for this
            var insightFacade = new InsightFacade();
            insightFacade.AddBook(book1Info[0].Trim(), book1Info[1].Trim());

            //insightFacade.AddBook(book2Id, book2Content);

            //insightFacade.TrainModel(book1Id, book2Id);

            //// TODO fiddle with numbers
            //insightFacade.GenerateBook(8, 20);


        }
    }
}
