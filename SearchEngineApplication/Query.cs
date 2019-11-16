using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngineApplication
{
    class Query
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inder"></param>
        /// <param name="queryRep"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, int>> matchingFunction(InvertedIndex inder, string queryRep)
        {

            var res = from result in inder.data
                      where result.Key.Equals(queryRep)
                      select result;

            return res.ToDictionary(p => p.Key, p => p.Value);
        }


        public static Dictionary<string, double> cosineSimilarity(Dictionary<string, double> query, SortedDictionary<string, Dictionary<string, int>> result)
        {
            double queryMod = 0;
            Dictionary<string, double> dotProducts = new Dictionary<string, double>();
            Dictionary<string, double> docMod = new Dictionary<string, double>();
            Dictionary<string, double> finaldocMod = new Dictionary<string, double>();

            foreach (var term in query)
            {
                queryMod += Math.Pow(term.Value, 2);
                if (result.ContainsKey(term.Key))
                {
                    foreach (KeyValuePair<string, int> doc in result[term.Key])
                    {
                        if (docMod.ContainsKey(doc.Key))
                        {
                            docMod[doc.Key] += Math.Pow(doc.Value, 2);
                            dotProducts[doc.Key] += term.Value * doc.Value;
                        }
                        else
                        {
                            docMod.Add(doc.Key, Math.Pow(doc.Value, 2));
                            dotProducts.Add(doc.Key, term.Value * doc.Value);
                        }
                    }
                }
            }
            foreach (KeyValuePair<string, double> item in docMod)
            {
                double value = dotProducts[item.Key] / (Math.Sqrt(docMod[item.Key]) * Math.Sqrt(queryMod));
                finaldocMod.Add(item.Key, value);
            }
            return finaldocMod;
        }

        public static Dictionary<string, double> convertQuery(string term)
        {
            Dictionary<string, double> weightedQuery = new Dictionary<string, double>();
            string[] x = DefaultAnalyzer.Analyzer(term.Split());

            foreach (string word in x)
            {
                if (weightedQuery.ContainsKey(word))
                {
                    weightedQuery[word] += 1;

                }
                else
                {

                    weightedQuery.Add(word, 1);
                }

            }

            return weightedQuery;

        }

    }
}
