using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Zupagood.Helpers;
using Zupagood.Models;

namespace Zupagood
{
    class Program
    {
        private static Stopwatch _stopwatch = new Stopwatch();
        private static ZupaTrie _zupaTrie = new ZupaTrie();
        private static SerializationHelper _serializationHelper = new SerializationHelper();

        static void Main()
        {
            List<string> words = ReadWordsFromFile();
            //GenerateSentencesFile(words);
            List<string> sentences = ReadSentencesFromFile();

            //_zupaTrie = _serializationHelper.Deserialize<ZupaTrie>("data.bin");

            InsertWords(words);
            InsertSentences(sentences);
            QuerySentences(sentences);

            //serializerHelper.Serialize<ZupaTrie>(_zupaTrie, "data.bin");

            Console.Read();
        }



        static void InsertWords(List<string> words)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            foreach (string word in words)
                _zupaTrie.Insert(word);

            _stopwatch.Stop();

            Console.WriteLine("Words: {0} ticks", (double)_stopwatch.ElapsedTicks / 10000000);
        }

        static void InsertSentences(List<string> sentences)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            foreach (string sentence in sentences)
                _zupaTrie.Submit(sentence);

            _stopwatch.Stop();

            Console.WriteLine("Sentences: {0} ticks", (double)_stopwatch.ElapsedTicks / 10000000);
        }

        static void QuerySentences(List<string> sentences)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            ZupaNode currentResult = null;

            foreach (string sentence in sentences)
                currentResult = _zupaTrie.Query(sentence);

            _stopwatch.Stop();

            Console.WriteLine("Query: {0} ticks", (double)_stopwatch.ElapsedTicks / 10000000);
        }



        static List<string> ReadWordsFromFile()
        {
            StreamReader wordStream = new StreamReader(@"words.txt");

            List<string> words = new List<string>();

            while (!wordStream.EndOfStream)
                words.Add(wordStream.ReadLine());

            return words;
        }

        static List<string> ReadSentencesFromFile()
        {
            StreamReader sentenceStream = new StreamReader(@"sentences.txt");

            List<string> sentences = new List<string>();

            while (!sentenceStream.EndOfStream)
                sentences.Add(sentenceStream.ReadLine());

            return sentences;
        }

        static void GenerateSentencesFile(List<string> words)
        {
            const int numberOfSentences = 10000;
            const int numberOfWordsPerSentence = 100;

            Random random = new Random();

            StringBuilder sentenceBuilder = new StringBuilder();

            for (int i = 0; i < numberOfSentences; i++)
            {
                for (int j = 0; j < numberOfWordsPerSentence; j++)
                {
                    sentenceBuilder.Append(words[random.Next(words.Count)]);
                    sentenceBuilder.Append(" ");
                }

                sentenceBuilder.Length--;

                sentenceBuilder.Append(Environment.NewLine);
            }
            
            File.AppendAllText("sentences.txt", sentenceBuilder.ToString().Trim());
        }
    }
}
