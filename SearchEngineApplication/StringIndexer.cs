using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TikaOnDotNet.TextExtraction;

namespace SearchEngineApplication
{
    public class StringIndexer
    {
        static private TextExtractor _cut = new TextExtractor();
        
        public static InvertedIndex CreateIndex(string[] files)
        {
            var result = new InvertedIndex();

            for (var i = 0; i < files.Length; i++)
            {
                var extracted = _cut.Extract(files[i]);

                var tokens = DefaultAnalyzer.Analyzer(extracted.Text.Split());

                foreach (var token in tokens)
                {
                    result.append(token, files[i]);

                }

            }

            foreach (KeyValuePair<string, Dictionary<string, int>> kvp in result.data)
            {

                Console.Write("Term = {0}", kvp.Key);

                foreach (KeyValuePair<string, int> res in kvp.Value)
                {
                    Console.Write("  Posting-List = {0},  Frequency = {1}", res.Key, res.Value);

                }

                Console.WriteLine();

            }


            return result;
        }
    }
}
