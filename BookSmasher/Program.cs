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
            //Console.WriteLine("Add book 1. Format: id,filepath");
            //var book1Info = Console.ReadLine().Split(",");

            //Console.WriteLine("Add book 2 with id,filepath");
            //var book2Info = Console.ReadLine().Split(",");

            //if (book1Info.Length != 2 || book2Info.Length != 2)
            //{
            //    throw new Exception("id and filepath must be seperated by a comma and neither may contain a comma");
            //}

            // TODO formatting class for this
            var insightFacade = new InsightFacade();
            //insightFacade.AddBook(book1Info[0].Trim(), book1Info[1].Trim());
            //insightFacade.AddBook(book2Info[0].Trim(), book2Info[1].Trim());

            insightFacade.AddBook("cat", @"C:\Users\Q\Desktop\test.txt");
            insightFacade.AddBook("dog", @"C:\Users\Q\Desktop\test2.txt");

            //insightFacade.AddBook(book2Id, book2Content);

            //insightFacade.TrainModel(book1Info[0].Trim(), book2Info[0].Trim());
            insightFacade.TrainModel("cat", "dog");

            insightFacade.GenerateBook("cat", "dog", 8, 20);
            //// TODO fiddle with numbers


        }
    }
}
