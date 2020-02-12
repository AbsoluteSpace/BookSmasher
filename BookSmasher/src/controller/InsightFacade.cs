using System;
using System.Collections.Generic;
using System.Linq;
using BookSmasher.src.machineLearning;
using BookSmasher.src.model;

namespace BookSmasher.src.controller
{
    public class InsightFacade : IInsightFacade
    {
        private Dictionary<string, Book> _books = new Dictionary<string, Book>();
        private Dictionary<string, BookCollection> _bookCollections = new Dictionary<string, BookCollection>();

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
            if (IdUtil.IdAlreadyAdded(id, _books.Keys.ToList()) || !IdUtil.IsValid(id))
            {
                throw new InvalidOperationException("Id is invalid.");
            }

            var bookBuilder = new BookBuilder(_bagOfWords);
            var newBook = bookBuilder.ConstructBook(id, content);
            _bagOfWords = bookBuilder.bagOfWords;

            // bad storage
            // paginate book's line 

            _books.Add(id, newBook);

            return _books.Keys.ToList();
        }


        public List<string> ListBooks()
        {
            return _books.Keys.ToList();
        }

        public List<string> ListBookCollectionNames()
        {
            return _bookCollections.Keys.ToList();
        }

        public List<string> RemoveBook(string id)
        {
            if (!IdUtil.IdAlreadyAdded(id, _books.Keys.ToList()) || !IdUtil.IsValid(id))
            {
                throw new InvalidOperationException("Id is invalid.");
            }

            _books.Remove(id);
            return _books.Keys.ToList();
        }

        public List<string> RemoveBookCollection(string id)
        {
            if (!IdUtil.IdAlreadyAdded(id, _bookCollections.Keys.ToList()) || !IdUtil.IsValid(id))
            {
                throw new InvalidOperationException("Id is invalid.");
            }

            _bookCollections.Remove(id);
            return _bookCollections.Keys.ToList();
        }

        // TODO refactor
        public void TrainModel(List<string> ids, int numExamplesToClassify, int numAdjacentExamples)
        {
            // At least 2 ids to train on.
            if (ids.Count < 2)
            {
                throw new Exception("At least 2 ids must be specified.");
            }

            var trainingBooks = _books.Where(book => ids.Contains(book.Key)).ToList();

            // Classify all sentences.
            var allSentences = new List<SentenceExample>();
            foreach (var book in trainingBooks)
            {
                allSentences.AddRange(book.Value.sentences);
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

            _bookCollections.Add(bookCol.id, bookCol);

        }

        public string GenerateBook(string id, int maxDepth, int numTrees, int numAdjacentExamples)
        {
            var bookCol = _bookCollections[id];

            // Train model.
            //var model = new RandomForest(maxDepth, numTrees);
            var model = new DecisionTree(maxDepth, new DecisionStumpInfoGain());
            var XTrain = ClassificationUtil.ConstructMatrixX(_bagOfWords, bookCol.trainingExamples);
            model.Fit(XTrain, bookCol.trainingLabels);

            var allSentences = bookCol.sentences;

            var bookGenerator = new BookGenerator(_bagOfWords);

            string[] stringOutput = bookGenerator.GeneratePredictedBook(allSentences, model, numAdjacentExamples)
                .Select(x => x.sentence).ToArray();
            var outputLocation = @"..\..\" + bookCol.id + ".txt";

            System.IO.File.WriteAllLines(outputLocation, stringOutput);
            return outputLocation;
        }

        // Display sentences on screen and takes user input to create training data.
        // TODO needs to scoot to another file and be broken up.
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

    }
}
