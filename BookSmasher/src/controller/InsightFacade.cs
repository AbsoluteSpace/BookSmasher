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

        // todo add parameters and description up here
        public List<string> AddBook(string id, string content)
        {
            // check content
            if (content == null)
            {
                throw new InvalidOperationException("Id and content must be provided to add a book.");
            }
            if (!content.EndsWith(".txt"))
            {
                throw new NotSupportedException("This file format isn't accepted.");
            }

            // check id not added and validity
            if (IdHelper.IdAlreadyAdded(id, _bookIds) || !IdHelper.IsValid(id))
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
            if (!IdHelper.IdAlreadyAdded(id, _bookIds) || !IdHelper.IsValid(id))
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
            if (!IdHelper.IdAlreadyAdded(id, _bookCollectionIds) || !IdHelper.IsValid(id))
            {
                throw new InvalidOperationException("Id is invalid.");
            }

            var idx = _bookCollectionIds.FindIndex(x => x.Equals(id));

            _bookCollections.RemoveAt(idx);
            _bookCollectionIds.RemoveAt(idx);
            return _bookCollectionIds;
        }

        // TODO add escape by pressing escape for all of these
        // TODO refactor
        public void TrainModel(List<string> ids, int numExamplesToClassify, int numAdjacentExamples)
        {
            // TODO check that at least 2 books added
            var trainingBooks = _books.Where(book => ids.Contains(book.id)).ToList();

            var outputExamples = new List<SentenceExample>();
            var outputLabels = new List<int>();

            // get and classify all sentences
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

            // get random assortment of sentences to train on
            RandomUtil.Shuffle(allSentences, rand);
            var sentencesToClassify = new List<SentenceExample>();
            for (int i = 0; i < allSentences.Count && i < numExamplesToClassify; i++)
            {
                sentencesToClassify.Add(allSentences[i]);
            }

            foreach (var sent in sentencesToClassify)
            {
                Console.WriteLine("\nCurrent Sentence: " + sent.sentence);

                // pick numAdjacentExamples random sentences
                var randomSentences = new List<SentenceExample>();
                var classificationsInRandomSentences = new List<int>();
                for (int j = 0; j < numAdjacentExamples; j++)
                {
                    // store random sentence and its classification
                    var sentenceToAdd = sentencesToClassify[rand.Next(0, sentencesToClassify.Count)];
                    randomSentences.Add(sentenceToAdd);
                    classificationsInRandomSentences.Add(sentenceToAdd.classification);
                }

                // display the sentences to choose from
                for (int j = 0; j < randomSentences.Count; j++)
                {
                    Console.WriteLine((j + 1) + ": " + randomSentences[j].sentence);
                }

                // user ranks them
                Console.WriteLine("Order the sentences like 1,2,...,N. Left is better, right is worse.");

                // TODO validate the input somehow
                var ranking = Console.ReadLine().Split(',');
                var intRanking = new List<int>();

                foreach (var entry in ranking)
                {
                    var intToAdd = int.Parse(entry);
                    intRanking.Add(intToAdd);
                }

                // update features of each random sentence, add them as training data
                foreach (var cs in randomSentences)
                {
                    var idxInRand = randomSentences.IndexOf(cs);
                    // TODO knowingly putting in bug where it classifies itself as an adjacent
                    cs.adjacentSentenceClassification = classificationsInRandomSentences;
                    cs.prevSentenceClassification = sent.classification;
                    outputExamples.Add(cs);
                    outputLabels.Add(intRanking[intRanking.IndexOf(idxInRand + 1)]);
                }

            }

            var bookCol = new BookCollection(ids);
            bookCol.sentences = allSentences;
            bookCol.trainingExamples = outputExamples;
            bookCol.trainingLabels = outputLabels;
            bookCol.bagOfWords = _bagOfWords;

            _bookCollections.Add(bookCol);
            _bookCollectionIds.Add(bookCol.id);

        }

        // TODO generaize this
        public string GenerateBook(string id, int maxDepth, int numTrees, int numAdjacentExamples)
        {
            // TODO watch if duplicates are stored between the two books -> for now store in just book1
            var bookCol = _bookCollections[_bookCollectionIds.IndexOf(id)];

            // train model
            //var model = new RandomForest(maxDepth, numTrees);
            var model = new DecisionTree(maxDepth, new DecisionStumpInfoGain());
            var XTrain = ClassificationUtil.ConstructMatrixX(_bagOfWords, bookCol.trainingExamples);
            model.Fit(XTrain, bookCol.trainingLabels);

            var allSentences = bookCol.sentences;
            Random rand = new Random();

            var predictedBook = new List<SentenceExample>();

            // add sentences to predictedBook
            var firstSentence = allSentences[rand.Next(0, allSentences.Count)];
            allSentences.Remove(firstSentence);
            predictedBook.Add(firstSentence);

            while (allSentences.Count != 0)
            {

                var adjacentClassifications = new List<int>();
                var nextSentences = new List<SentenceExample>();

                for (int i = 0; i < numAdjacentExamples; i++)
                {
                    var nextSentence = allSentences[rand.Next(0, allSentences.Count)];
                    // no duplicates until the end
                    if (nextSentences.Contains(nextSentence) && allSentences.Count > numAdjacentExamples)
                    {
                        i--;
                        continue;
                    }
                    adjacentClassifications.Add(nextSentence.classification);
                    nextSentences.Add(nextSentence);
                }

                // fill in features based on what the sentence is compared against
                foreach (var sentence in nextSentences)
                {
                    sentence.prevSentenceClassification = firstSentence.classification;
                    sentence.adjacentSentenceClassification = adjacentClassifications;
                }

                // TODO some wasted time
                var labels = model.Predict(ClassificationUtil.ConstructMatrixX(_bagOfWords, nextSentences));

                // gets the best sentence according to label
                int maxValue = labels.Max();
                int maxIndex = labels.ToList().IndexOf(maxValue);

                var bestNextSentence = nextSentences[maxIndex];

                // add bestNextSentence as next sentence in new book
                allSentences.Remove(bestNextSentence);
                predictedBook.Add(bestNextSentence);
                firstSentence = bestNextSentence;

            }

            string[] stringOutput = predictedBook.Select(x => x.sentence).ToArray();
            var outputLocation = @"..\..\" + bookCol.id + ".txt";

            System.IO.File.WriteAllLines(outputLocation, stringOutput);
            return outputLocation;
        }

    }
}
