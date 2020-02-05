using Classifier.src.model;
using System.Collections.Generic;
using System.Linq;

namespace Classifier.src.controller
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
            var lines = ParseLines(content);

            var newBook = new Book();
            newBook.id = id;
            newBook.clusteredExamples = lines.Cast<SentenceExample>().ToList(); // TODO make more general

            // at very end classify each of these according to some classifier algorithm

            return newBook;
        }

        // TODO should have restrictions on what sentences cna contain otherwise security issue
        private List<SentenceExample> ParseLines(string content)
        {
            var output = new List<SentenceExample>();

            // read all the lines from the file including whitespace
            string[] lines = System.IO.File.ReadAllLines(content);

            string sentenceToCont = null;

            for (int i = 0; i < lines.Length; i++)
            {
                var sentences = lines[i].Split(_delimiterChars).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                var endVal = sentences.Length;

                if (sentences.Length == 0)
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(sentenceToCont))
                {
                    sentences[0] = sentenceToCont.Trim() + " " + sentences[0];
                    sentenceToCont = null;
                }

                // TODO this is bad sructure
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
                    // TODO too many loops
                    for (int k = 0; k < words.Length; k++)
                    {
                        if (!bagOfWords.Contains(words[k]))
                        {
                            bagOfWords.Add(words[k]);
                        }

                        s.wordIndexes.Add(bagOfWords.IndexOf(words[k]));
                    }

                    output.Add(s);
                }

            }

            return output;
        }
    }
}
