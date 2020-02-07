using System;
using System.Collections.Generic;
using System.Linq;
using BookSmasher.src.machineLearning;
using BookSmasher.src.model;

namespace BookSmasher.src.controller
{
    public class InsightFacade : IInsightFacade
    {
        private List<string> _bookIds = new List<string>();
        private List<Book> _books = new List<Book>();

        private List<BookCollection> _bookCollections = new List<BookCollection>();
        private List<string> _bookCollectionIds = new List<string>();

        private List<string> _bagOfWords = new List<string>();

        public List<string> AddBook(string id, string content)
        {
            // Check content.
            if (content == null)
            {
                throw new InvalidOperationException("Id and content must be provided to add a book.");
            }
            if (!content.EndsWith(".txt"))
            {
                throw new NotSupportedException("This file format isn't accepted.");
            }

            // Check id validity.
            if (IdUtil.IdAlreadyAdded(id, _bookIds) || !IdUtil.IsValid(id))
            {
                throw new InvalidOperationException("Id is invalid.");
            }

            var bookBuilder = new BookBuilder(_bagOfWords);
            var newBook = bookBuilder.ConstructBook(id, content);
            _bagOfWords = bookBuilder.bagOfWords;

            _books.Add(newBook);
            _bookIds.Add(id);

            return _bookIds;
        }


        public List<string> ListBooks()
        {
            return _bookIds;
        }

        public List<string> ListBookCollectionNames()
        {
            return _bookCollectionIds;
        }

        public List<string> RemoveBook(string id)
        {
            if (!IdUtil.IdAlreadyAdded(id, _bookIds) || !IdUtil.IsValid(id))
            {
                throw new InvalidOperationException("Id is invalid.");
            }

            var idx = _bookIds.FindIndex(x => x.Equals(id));

            _books.RemoveAt(idx);
            _bookIds.RemoveAt(idx);
            return _bookIds;
        }

        public List<string> RemoveBookCollection(string id)
        {
            if (!IdUtil.IdAlreadyAdded(id, _bookCollectionIds) || !IdUtil.IsValid(id))
            {
                throw new InvalidOperationException("Id is invalid.");
            }

            var idx = _bookCollectionIds.FindIndex(x => x.Equals(id));

            _bookCollections.RemoveAt(idx);
            _bookCollectionIds.RemoveAt(idx);
            return _bookCollectionIds;
        }

        // TODO refactor
        public void TrainModel(List<string> ids, int numExamplesToClassify, int numAdjacentExamples)
        {
            // At least 2 ids to train on.
            if (ids.Count < 2)
            {
                throw new Exception("At least 2 ids must be specified.");
            }

            var trainingBooks = _books.Where(book => ids.Contains(book.id)).ToList();

            // Classify all sentences.
            var allSentences = new List<SentenceExample>();
            foreach (var book in trainingBooks)
            {
                allSentences.AddRange(book.sentences);
            }

            Random rand = new Random();
            foreach(var sentence in allSentences)
            {
                sentence.classification = rand.Next(0, 4);
            }

            // Get random assortment of sentences to train on.
            RandomUtil.Shuffle(allSentences, rand);
            var sentencesToClassify = new List<SentenceExample>();
            for (int i = 0; i < allSentences.Count && i < numExamplesToClassify; i++)
            {
                sentencesToClassify.Add(allSentences[i]);
            }

            // Get training data using sentences from user input.
            var trainingData = DisplayAdjacentSentences(sentencesToClassify, numAdjacentExamples, rand);

            var bookCol = new BookCollection(ids);
            bookCol.sentences = allSentences;
            bookCol.trainingExamples = trainingData.Item1;
            bookCol.trainingLabels = trainingData.Item2;
            bookCol.bagOfWords = _bagOfWords;

            _bookCollections.Add(bookCol);
            _bookCollectionIds.Add(bookCol.id);

        }

        public string GenerateBook(string id, int maxDepth, int numTrees, int numAdjacentExamples)
        {
            var bookCol = _bookCollections[_bookCollectionIds.IndexOf(id)];

            // Train model.
            //var model = new RandomForest(maxDepth, numTrees);
            var model = new DecisionTree(maxDepth, new DecisionStumpInfoGain());
            var XTrain = ClassificationUtil.ConstructMatrixX(_bagOfWords, bookCol.trainingExamples);
            model.Fit(XTrain, bookCol.trainingLabels);

            var allSentences = bookCol.sentences;

            string[] stringOutput = GeneratePredictedBook(allSentences, model, numAdjacentExamples)
                .Select(x => x.sentence).ToArray();
            var outputLocation = @"..\..\" + bookCol.id + ".txt";

            System.IO.File.WriteAllLines(outputLocation, stringOutput);
            return outputLocation;
        }

        // Display sentences on screen and takes user input to create training data.
        private Tuple<List<SentenceExample>, List<int>> DisplayAdjacentSentences(List<SentenceExample> sentencesToClassify,
            int numAdjacentExamples, Random rand)
        {
            var trainingExamples = new List<SentenceExample>();
            var trainingLabels = new List<int>();

            foreach (var sent in sentencesToClassify)
            {
                Console.WriteLine("\nCurrent Sentence: " + sent.sentence);

                // Pick numAdjacentExamples random sentences.
                var randomSentences = new List<SentenceExample>();
                var classificationsInRandomSentences = new List<int>();
                for (int j = 0; j < numAdjacentExamples; j++)
                {
                    // Store random sentence and its classification.
                    var sentenceToAdd = sentencesToClassify[rand.Next(0, sentencesToClassify.Count)];
                    if (randomSentences.Contains(sentenceToAdd) && sentencesToClassify.Count > numAdjacentExamples)
                    {
                        j--;
                        continue;
                    }
                    randomSentences.Add(sentenceToAdd);
                    classificationsInRandomSentences.Add(sentenceToAdd.classification);
                }

                // Display the sentences to choose from.
                for (int j = 0; j < randomSentences.Count; j++)
                {
                    Console.WriteLine((j + 1) + ": " + randomSentences[j].sentence);
                }

                // User ranks the sentences.
                Console.WriteLine("Order the sentences like 1,2,...,N. Left is worse, right is better.");

                var ranking = Console.ReadLine().Split(',');
                var intRanking = new List<int>();

                foreach (var entry in ranking)
                {
                    var intToAdd = int.Parse(entry);
                    intRanking.Add(intToAdd);
                }

                // Update features of each random sentence, add them as training data.
                foreach (var cs in randomSentences)
                {
                    var idxInRand = randomSentences.IndexOf(cs);
                    cs.adjacentSentenceClassification = classificationsInRandomSentences;
                    cs.prevSentenceClassification = sent.classification;
                    trainingExamples.Add(cs);
                    trainingLabels.Add(intRanking[intRanking.IndexOf(idxInRand + 1)]);
                }

            }

            return new Tuple<List<SentenceExample>, List<int>>(trainingExamples, trainingLabels);
        }

        // Generates what should be printed into the .txt file as the predicted book.
        private List<SentenceExample> GeneratePredictedBook(List<SentenceExample> allSentences, DecisionTree model,
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
                var labels = model.Predict(ClassificationUtil.ConstructMatrixX(_bagOfWords, nextSentences));

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
        private List<SentenceExample> GenerateNextSentences(List<SentenceExample> allSentences, 
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
