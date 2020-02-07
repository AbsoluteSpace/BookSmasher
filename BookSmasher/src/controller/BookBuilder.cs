using BookSmasher.src.model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookSmasher.src.controller
{
    // Retrieve information from input files
    public class BookBuilder
    {
        // TODO advanced sentence handling
        private char[] _delimiterChars = { '.', '!', '?' };

        public List<string> bagOfWords;

        public BookBuilder(List<string> bagOfWords)
        {
            this.bagOfWords = bagOfWords;
        }

        public Book ConstructBook(string id, string content)
        {
            var newBook = new Book();
            newBook.id = id;
            // TODO did this throw an error
            newBook.clusteredExamples = ParseLines(content); // TODO make more general -> interface rule of 3

            // TODO classify each of these according to some classifier algorithm

            return newBook;
        }

        // TODO need to go through all names
        // TODO should have restrictions on what sentences can contain otherwise security issue
        private List<SentenceExample> ParseLines(string content)
        {
            var output = new List<SentenceExample>();

            // TODO make exception handled to not show filepath
            string[] lines = System.IO.File.ReadAllLines(content);

            string sentenceToCont = null;

            for (int i = 0; i < lines.Length; i++)
            {
                var sentences = lines[i].Split(_delimiterChars).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                var endVal = sentences.Length;

                // next iteration if no sentences
                if (sentences.Length == 0)
                {
                    continue;
                }

                // leftover sentence from previous line, add onto the first sentence
                if (!string.IsNullOrWhiteSpace(sentenceToCont))
                {
                    sentences[0] = sentenceToCont.Trim() + " " + sentences[0];
                    sentenceToCont = null;
                }

                // TODO this is bad sructure -> refactor to method?
                // if line doesn't end with one of these punctuation, last sentence continues to next line
                if (!(lines[i].EndsWith('.') || lines[i].EndsWith('!') || lines[i].EndsWith('?')))
                {
                    sentenceToCont = sentences[sentences.Length-1];
                    endVal--;
                }

                for (int j = 0; j < endVal; j++)
                {
                    var s = new SentenceExample();
                    s.sentence = sentences[j].TrimEnd() + ".";

                    // TODO doesn't look at punctuation
                    var words = sentences[j].Split(" ");
                    s.wordIndexes.AddRange(UpdateBagOfWords(words));

                    output.Add(s);
                }

            }

            return output;
        }

        private List<int> UpdateBagOfWords(string[] words)
        {
            var wordIndexes = new List<int>();

            for (int k = 0; k < words.Length; k++)
            {
                if (!bagOfWords.Contains(words[k]))
                {
                    bagOfWords.Add(words[k]);
                }

                wordIndexes.Add(bagOfWords.IndexOf(words[k]));
            }

            return wordIndexes;
        }
    }
}
