using System;
using System.Collections.Generic;

namespace BookSmasher.src.model
{
    public class Book : IBook
    {
        // TODO look at accessability levels
        // TODO think about how it's stored, this is wasteful
        public string id { get; set; }

        // order is guranteed, so can always recunstruct original book as txt
        public List<SentenceExample> clusteredExamples {get; set;}

        // where examples that can be used as training go TODO I don't like this
        public List<Tuple<SentenceExample,int>> classifiedExamples { get; set; }


        // bag of words stored here and maybe retrieved when a new book is added
        // maybe TODO

        public Book()
        {
            clusteredExamples = new List<SentenceExample>();
            classifiedExamples = new List<Tuple<SentenceExample, int>>();

        }

        public List<List<int>> ConstructTrainingX(List<string> bagOfWords)
        {
            var retVal = new List<List<int>>();

            // TODO will need to be more than bag of words at some point
            for (int i = 0; i < classifiedExamples.Count; i++)
            {
                retVal.Add(new List<int>());

                // initialize with 0 by default for the correct length
                for (int j = 0; j < bagOfWords.Count; j++)
                {
                    retVal[i].Add(0);
                }

                var sentence = classifiedExamples[i].Item1;
                // TODO check not off by 1
                foreach(var wordIdx in sentence.wordIndexes)
                {
                    retVal[i][wordIdx] = 1;
                }

                // add prevSentence classification as feature
                retVal[i].Add(classifiedExamples[i].Item1.prevSentenceClassification);
                // add adjacentSentence classifications as features
                retVal[i].AddRange(classifiedExamples[i].Item1.adjacentSentenceClassification);
            }

            return retVal;
        }

        public List<List<int>> ConstructTestX(List<string> bagOfWords, List<SentenceExample> sentences)
        {
            var retVal = new List<List<int>>();

            // TODO will need to be more than bag of words at some point
            for (int i = 0; i < sentences.Count; i++)
            {
                retVal.Add(new List<int>());
                // initialize with 0 by default for the correct length
                for (int j = 0; j < bagOfWords.Count; j++)
                {
                    retVal[i].Add(0);
                }

                var sentence = sentences[i];
                // TODO check not off by 1
                foreach (var wordIdx in sentence.wordIndexes)
                {
                    retVal[i][wordIdx] = 1;
                }

                // add prevSentence classification as feature
                retVal[i].Add(sentences[i].prevSentenceClassification);
                // add adjacentSentence classifications as features
                retVal[i].AddRange(sentences[i].adjacentSentenceClassification);
            }

            return retVal;
        }
    }
}
