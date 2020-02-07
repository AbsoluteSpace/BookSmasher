using BookSmasher.src.machineLearning;
using BookSmasher.src.model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookSmasher.src.controller
{
    // Class to help with the generation of output books.
    public class BookGenerator
    {
        public List<string> bagOfWords { get; set; }

        public BookGenerator(List<string> bagOfWords)
        {
            this.bagOfWords = bagOfWords;
        }

        // Generates what should be printed into the .txt file as the predicted book.
        public List<SentenceExample> GeneratePredictedBook(List<SentenceExample> allSentences, DecisionTree model,
            int numAdjacentExamples)
        {
            var predictedBook = new List<SentenceExample>();
            Random rand = new Random();

            // Add sentences to predictedBook.
            var firstSentence = allSentences[rand.Next(0, allSentences.Count)];
            allSentences.Remove(firstSentence);
            predictedBook.Add(firstSentence);

            while (allSentences.Count != 0)
            {
                var nextSentences = GenerateNextSentences(allSentences, firstSentence, numAdjacentExamples, rand);
                var labels = model.Predict(ClassificationUtil.ConstructMatrixX(bagOfWords, nextSentences));

                // Gets the best sentence according to label.
                int maxValue = labels.Max();
                int maxIndex = labels.ToList().IndexOf(maxValue);

                var bestNextSentence = nextSentences[maxIndex];

                // Add bestNextSentence as next sentence in new book.
                allSentences.Remove(bestNextSentence);
                predictedBook.Add(bestNextSentence);
                firstSentence = bestNextSentence;

            }

            return predictedBook;
        }

        // Generate sentences to compare against the first sentence.
        public List<SentenceExample> GenerateNextSentences(List<SentenceExample> allSentences,
            SentenceExample firstSentence, int numAdjacentExamples, Random rand)
        {
            var adjacentClassifications = new List<int>();
            var nextSentences = new List<SentenceExample>();

            for (int i = 0; i < numAdjacentExamples; i++)
            {
                var nextSentence = allSentences[rand.Next(0, allSentences.Count)];
                // No duplicates until the end.
                if (nextSentences.Contains(nextSentence) && allSentences.Count > numAdjacentExamples)
                {
                    i--;
                    continue;
                }
                adjacentClassifications.Add(nextSentence.classification);
                nextSentences.Add(nextSentence);
            }

            // Fill in features based on what the sentence is compared against.
            foreach (var sentence in nextSentences)
            {
                sentence.prevSentenceClassification = firstSentence.classification;
                sentence.adjacentSentenceClassification = adjacentClassifications;
            }

            return nextSentences;
        }
    }
}
