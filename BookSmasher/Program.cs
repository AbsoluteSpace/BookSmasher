using BookSmasher.src.controller;
using System;

namespace BookSmasher
{
    class Program
    {

         /*
         * cat,C:\Users\Q\Desktop\test.txt
         * dog,C:\Users\Q\Desktop\test2.txt
         */
        static void Main(string[] args)
        {
            var keepLooping = true;
            var insightFacade = new InsightFacade();
            var renderer = new Renderer();

            while(keepLooping)
            {
                Console.WriteLine("\n" + "Press [A] to add a book, [R] to remove a book, [L] to list books," +
                    " [T] to train a classifier, [G] to generate a new book, [Escape] to exit");

                var keyPress = Console.ReadKey().Key;

                switch (keyPress)
                {
                    case ConsoleKey.A:
                        renderer.RenderAddBook(insightFacade);
                        break;
                    case ConsoleKey.R:
                        renderer.RenderRemoveBook(insightFacade);
                        break;
                    case ConsoleKey.L:
                        renderer.RenderListBook(insightFacade);
                        break;
                    case ConsoleKey.T:
                        renderer.RenderTrainBook(insightFacade);
                        break;
                    case ConsoleKey.G:
                        renderer.RenderGenerateBook(insightFacade);
                        break;
                    case ConsoleKey.Escape:
                        keepLooping = false;
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
