using System;
using System.Collections.Generic;
using System.Linq;
using BookSmasher.src.controller;
using BookSmasher.src.machineLearning;
using Classifier.src.machineLearning;
using Classifier.src.model;

namespace Classifier.src.controller
{
    public class InsightFacade : IInsightFacade
    {
        private List<string> _bookIds = new List<string>();
        private List<Book> _books = new List<Book>();

        // bag of words stored here, not great choice, but should be easy to fix
        private List<string> _bagOfWords = new List<string>();

        public List<string> AddBook(string id, string content)
        {
            // TODO do file check in different area
            if (content == null)
            {
                throw new InvalidOperationException("Id and content must be provided to add a book.");
            }
            // if else stuff for now here, don't want it -> need like polymoprhism or something
            if (!content.EndsWith(".txt"))
            {
                throw new NotSupportedException("This file format isn't accepted.");
            }

            if (IdHelper.IdAlreadyAdded(id) || !IdHelper.IsValid(id))
            {
                throw new InvalidOperationException("Id is invalid.");
            }

            var bookBuilder = new BookBuilder(_bagOfWords);
            var newBook = bookBuilder.ConstructBook(id, content);
            _bagOfWords = bookBuilder.bagOfWords;

            // TODO improve storage of books -> caching?
            //var bookKeeper = new BookKeeper();
            //bookKeeper.CacheBook(newBook);
            _books.Add(newBook);
            _bookIds.Add(id);

            return _bookIds;
        }

        // TODO generaize this
        public string GenerateBook(string id1, string id2, int maxDepth, int numTrees)
        {
            // train model with training data from both books -> random forest
            // TODO currently books store training ex without reference to which book they're combined with

            // TODO named tuples
            // TODO watch if duplicates are stored between the two books -> for now store in just book1
            var firstBook = _books[_bookIds.IndexOf(id1)];
            var secondBook = _books[_bookIds.IndexOf(id2)];

            var allSentences = new List<SentenceExample>();
            allSentences.AddRange(firstBook.clusteredExamples);
            allSentences.AddRange(secondBook.clusteredExamples);

            Random rand = new Random();
            foreach (var sentence in allSentences)
            {
                // ideally this would already be done when adding TODO
                sentence.classification = rand.Next(0, 4);
            }

            var XTrain = firstBook.ConstructTrainingX(_bagOfWords);
            var yTrain = firstBook.classifiedExamples.Select(x => x.Item2).ToList();
            //var model = new RandomForest(maxDepth, numTrees);
            var model = new DecisionTree(maxDepth, new DecisionStumpInfoGain());
            model.Fit(XTrain, yTrain);

            // take one sentence and remove from all sentences
            // predict it
            // take 5 (or k) more, assign values, predict them and pick the best -> wasteful I know
            // use the best (highest label) as the next sentence and remove it, return all others
            // keep doing this storing sentences in order until number reached or no more sentences
            // write this list to a text file or console

            var output = new List<SentenceExample>();
            var firstSentence = allSentences[rand.Next(0, allSentences.Count)];
            allSentences.Remove(firstSentence);
            output.Add(firstSentence);

            while (allSentences.Count != 0) {

                var adjacentClassifications = new List<int>();
                var nextSentences = new List<SentenceExample>();
                for (int i = 0; i < 5; i++)
                {
                    var nextSentence = allSentences[rand.Next(0, allSentences.Count)];
                    if (nextSentences.Contains(nextSentence) && allSentences.Count > 8)
                    {
                        i--;
                        continue;
                    }
                    nextSentence.prevSentenceClassification = firstSentence.classification;
                    adjacentClassifications.Add(nextSentence.classification);
                    nextSentences.Add(nextSentence);
                }

                foreach (var sentence in nextSentences)
                {
                    sentence.adjacentSentenceClassification = adjacentClassifications;
                }

                var testSentences = firstBook.ConstructTestX(_bagOfWords, nextSentences);
                var labels = model.Predict(testSentences);

                // gets the best sentence
                int maxValue = labels.Max();
                int maxIndex = labels.ToList().IndexOf(maxValue);

                var bestNextSentence = nextSentences[maxIndex];

                allSentences.Remove(bestNextSentence);
                output.Add(bestNextSentence);

                firstSentence = bestNextSentence;

            }

            string[] stringOutput = output.Select(x => x.sentence).ToArray();


            System.IO.File.WriteAllLines(@"..\..\output.txt", stringOutput);



            //var XPredict = firstBook.ConstructTestX(_bagOfWords, firstBook.clusteredExamples);
            //// TODO think about how we're reusing training data in test stuff and extremes
            //var predictedExamples = model.Predict(XPredict);

            // now print

            // feed first sentence of random book to this as first line
            // then grab random 5 sentences from both books and pick one with best score as next sentence
            // repeat this process until either set number of sentences or end of sentences

            // just print these to the console
            // or into txt file

            // TODO need check that y, X same length

            // TODO seems to always predict same thing, which is wack with test stuff
            return null;
        }

        public List<string> ListBooks()
        {
            return _bookIds;
        }

        public List<string> RemoveBook(string id)
        {
            if (!IdHelper.IdAlreadyAdded(id) || !IdHelper.IsValid(id))
            {
                throw new InvalidOperationException("Id is invalid.");
            }

            // TODO remove from local storage with BookKeeper

            _bookIds.RemoveAt(_bookIds.FindIndex(x => x.Equals(id)));
            return _bookIds;
        }

        public void TrainModel(string id1, string id2)
        {
            // need both books so error check for that
            var firstBook = _books[_bookIds.IndexOf(id1)];
            var secondBook = _books[_bookIds.IndexOf(id2)];

            var output = new List<Tuple<SentenceExample, int>>();

            var allSentences = new List<SentenceExample>();
            allSentences.AddRange(firstBook.clusteredExamples);
            allSentences.AddRange(secondBook.clusteredExamples);

            Random rand = new Random();
            foreach(var sentence in allSentences)
            {
                sentence.classification = rand.Next(0, 4);
            }

            RandomUtil.Shuffle(allSentences, rand);
            var sentencesToClassify = new List<SentenceExample>();

            for (int i = 0; i < allSentences.Count && i < 8; i++)
            {
                // probably better way with random
                sentencesToClassify.Add(allSentences[i]);
            }

            foreach (var sent in sentencesToClassify)
            {
                Console.WriteLine("Current Sentence: " + sent.sentence);

                // TODO can make all these numbers hyperparams
                // pick 5 random sentences

                var random = new List<SentenceExample>();
                var classificationsInRandom = new List<int>();
                for (int j = 0; j < 3; j++)
                {
                    // update classification of before
                    var toAdd = sentencesToClassify[rand.Next(0, sentencesToClassify.Count)];
                    toAdd.prevSentenceClassification = sent.classification;
                    random.Add(toAdd);
                    classificationsInRandom.Add(toAdd.classification);
                }

                // display the 5
                for (int j = 0; j < random.Count; j++)
                {
                    Console.WriteLine((j + 1) + ": " + random[j].sentence);
                }

                // user ranks them
                Console.WriteLine("Order the sentences like 1,4,5,2,3");

                var ranking = Console.ReadLine().Split(',');
                var intRanking = new List<int>();

                foreach (var entry in ranking)
                {
                    var intToAdd = int.Parse(entry);
                    intRanking.Add(intToAdd);
                }

                // use classified sentneces by updating prevsentence, adjacent, and label score
                foreach(var cs in random)
                {
                    var idxInRand = random.IndexOf(cs);
                    // TODO knowingly putting in bug where it classifies itself as an adjacent
                    cs.adjacentSentenceClassification = classificationsInRandom;
                    output.Add(new Tuple<SentenceExample, int>(cs, intRanking[intRanking.IndexOf(idxInRand + 1)]));
                }

            }

            firstBook.classifiedExamples = output;

            // TODO figure out return value
            return;
        }

        private List<List<int>> GenerateFakeX()
        {
            var output = new List<List<int>>();
            var rand = new Random();

            for (int i = 0; i < 200; i++)
            {
                var toAdd = new List<int>();
                for (int j = 0; j < 40; j++)
                {
                    toAdd.Add(rand.Next(0,4));
                }
                output.Add(toAdd);
            }

            return output;
        }

        private List<int> GenerateFakeY(List<List<int>> X)
        {
            var output = new List<int>();

            for (int i = 0; i < X.Count; i++)
            {
                if (X[i][4] == 1 || X[i][2] == 0)
                {
                    if (X[i][6] == 1 || X[i][5] == 0)
                    {
                        output.Add(1);
                    } else
                    {
                        output.Add(3);
                    }
                } else
                {
                    output.Add(2);
                }
            }

            return output;
        }
    }
}
