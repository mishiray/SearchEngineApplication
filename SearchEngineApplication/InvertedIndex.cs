using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngineApplication
{
    public class InvertedIndex : IEnumerable
    {

        public readonly SortedDictionary<string, Dictionary<string, int>> data =
        new SortedDictionary<string, Dictionary<string, int>>();

        internal void append(string term, string documentId)
        {


            if (data.ContainsKey(term))
            {
                if (data[term].ContainsKey(documentId))
                {
                    data[term][documentId] += 1;
                }
                else
                {

                    data[term].Add(documentId, 1);
                }
            }

            else
            {
                var postings = new Dictionary<string, int>();
                postings.Add(documentId,1);

                data.Add(term, postings);

            }

        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)data).GetEnumerator();
        }

    }
}
