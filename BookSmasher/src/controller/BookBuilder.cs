using BookSmasher.src.model;
using System.Collections.Generic;
using System.Linq;

namespace BookSmasher.src.controller
{
    // Store information from input file as a book.
    public class BookBuilder
    {
        private char[] _delimiterChars = { '.', '!', '?' };
        public List<string> bagOfWords;

        public BookBuilder(List<string> bagOfWords)
        {
            this.bagOfWords = bagOfWords;
        }

        // Construct book from given txt file. Content is the filepath.
        public Book ConstructBook(string id, string content)
        {
            var newBook = new Book();
            newBook.id = id;
            newBook.sentences = ParseLines(content);

            return newBook;
        }

        // Create examples using sentences from txt file. Content is the filepath to the txt file.
        private List<SentenceExample> ParseLines(string content)
        {
            var output = new List<SentenceExample>();
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

                // If leftover sentence from previous line, add onto the first sentence.
                if (!string.IsNullOrWhiteSpace(sentenceToCont))
                {
                    sentences[0] = sentenceToCont.Trim() + " " + sentences[0];
                    sentenceToCont = null;
                }

                // If line doesn't end with one of these punctuations, last sentence continues to next line.
                if (!EndsWithPunctuation(lines[i]))
                {
                    sentenceToCont = sentences[sentences.Length-1];
                    endVal--;
                }

                for (int j = 0; j < endVal; j++)
                {
                    var words = sentences[j].Split(" ");

                    var s = new SentenceExample();
                    s.sentence = sentences[j].TrimEnd() + ".";
                    s.wordIndexes.AddRange(UpdateBagOfWords(words));

                    output.Add(s);
                }

            }

            return output;
        }

        // Return indices of words in bagOfWords, add the words if not already present.
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

        // Return true if sentence ends with .,?,! otherwise false.
        private bool EndsWithPunctuation(string line)
        {
            return line.EndsWith('.') || line.EndsWith('!') || line.EndsWith('?');
        }
    }
}
