using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using TikaOnDotNet.TextExtraction;

namespace Interface
{

    /// <summary>
    /// Gets the files in the repository
    /// </summary>
    public class FileIndexer
    {
        /// <summary>
        /// a Hashset containing all the files in the repository
        /// </summary>
        public HashSet<FileInfo> _corpus = new HashSet<FileInfo> { };
        /// <summary>
        /// contains all keywords in the corpus
        /// </summary>
        public HashSet<string> _commonIndex = new HashSet<string> { };
        /// <summary>
        /// contains all the files in the corpus with all texts appended
        /// </summary>
        public Dictionary<FileInfo, List<string>> _Index = new Dictionary<FileInfo, List<string>> { };


        /// <summary>
        /// Gets all pdf, doc, docx, ppt, pptx, xls, xlsx, txt, html and xml files and adds them to a repository
        /// It creates a list of all the files in the repository
        /// </summary>
        /// <param name="directory">a string parameter</param>
        public void fileIndexer(string directory)
        {
        

            var fileExtensions = new List<string> { ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx", ".txt", ".html", ".xml" };
            //gets all files in the directory that has an extension included in the fileExtensions list and
            //stores them in an IEnumerable collection
            var allFiles = new DirectoryInfo(directory).GetFiles("*.*")
                .Where(s => fileExtensions.Contains(Path.GetExtension(s.Name)));
            //adds all file in the allFiles collection to the Hashset corpus
            foreach (FileInfo doc in allFiles)
            {
                _corpus.Add(doc);
            }

        }

        /// <summary>
        /// builds a foward index of all files and appends all its text
        /// </summary>
        /// <param name="path">a string parameter</param>
        public void buildIndex(string path)
        {
            fileIndexer(path);
            foreach (var doc in _corpus)
            {
                if (!_Index.ContainsKey(doc))
                {
                    _Index.Add(doc, extractText(doc));
                }
            }

        }


        /// <summary>
        /// extracts the texts from the document passed
        /// </summary>
        /// <param name="document">a Fileinfo parameter</param>
        /// <returns>a list of all the words or terms contained in the parameter</returns>
        public List<string> extractText(FileInfo document)
        {
            List<string> docTexts = new List<string> { };
            var textextractor = new TextExtractor();
            bool check = false;

            var result = textextractor.Extract(document.FullName);

            List<string> stopWords = new List<string> { "to", "from", "in", "on", "with", "without", "within",
                                                        "which", "a", "the", "an", "and", "upon", "by", "about", "for",
                                                       "after", "but", "above", "over", "at", "into", "until", "it" };
            MatchCollection matches = Regex.Matches(result.Text, "[a-z]([:']?[a-z])*", RegexOptions.IgnoreCase);
            //  foreach (Match match in matches)
            //  {

            //if(!stopWords.Contains(match.Value.ToLower()))
            //{
            //      _commonIndex.Add(match.Value.ToLower());
            //          docTexts.Add(match.Value);	
            //}
            //  }
            var texts =
                  from Match match in matches
                  where !stopWords.Contains(match.Value.ToLower())
                  select match.Value.ToLower();

            docTexts.AddRange(texts);
            _commonIndex.UnionWith(texts);

            return docTexts;
        }

        /// <summary>
        /// checkes if a list contains a term
        /// </summary>
        /// <param name="term">a string parameter</param>
        /// <param name="allTexts">a list of type string parameter</param>
        /// <returns>a boolean value</returns>
        public bool containsTerm(string term, List<string> allTexts)
        {
            //foreach (var keyword in allTexts)
            //{
            //    if (keyword.ToLower() == term.ToLower())
            //    {
            //        return true;
            //    }
            //}
           return allTexts.Exists(x => x.ToLower() == term);
        }

        /// <summary>
        /// counts the number of files in the corpus containing a term 
        /// </summary>
        /// <param name="term">a string parameter</param>
        /// <returns>an integer value</returns>
        public int docsContainingTerm(string term)
        {
            int _corpusContainingTerm = 0;
            foreach (var doc in _corpus)
            {
                var docs = _Index.SingleOrDefault(p => p.Key == doc);
                if (containsTerm(term, docs.Value))
                {
                    _corpusContainingTerm += 1;
                }

            }
            return _corpusContainingTerm;
        }

        /// <summary>
        /// calculates the frequency of a term in a doc
        /// </summary>
        /// <param name="term">a string parameter</param>
        /// <param name="doc">a Fileinfo parameter</param>
        /// <returns>an integer value</returns>
        public int termFrequency(string term, FileInfo doc)
        {
            int frequency = 0;
            var docs = _Index.SingleOrDefault(p => p.Key == doc);
            return docs.Value.FindAll(x => x.ToLower() == term.ToLower()).Count;
        }

        /// <summary>
        /// calculate the frequency of a term in a doc
        /// </summary>
        /// <param name="term">a string parameter</param>
        /// <param name="tokens">a string array parameter</param>
        /// <returns>an integer value</returns>
        public int termFrequency(string term, string[] tokens)
        {
            //int frequency = 0;

            //foreach (var word in tokens)
            //{
            //    if (term.ToLower() == word.ToLower())
            //    {
            //        frequency += 1;
            //    }
            //}
            //return frequency;
            return tokens.ToList().FindAll(x => x.ToLower() == term.ToLower()).Count;
        }

        
    }
}
