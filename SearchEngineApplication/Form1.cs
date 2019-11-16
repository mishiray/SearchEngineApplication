using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Windows.Forms;

namespace SearchEngineApplication
{
    public partial class Form1 : Form
    {
        InvertedIndex res;


        public Form1()
        {
            InitializeComponent();
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            richTextBox1.Clear();

            //Console.WriteLine("\n\n\n Try search function");

            //string[] token = textBox1.Text.Split();

            //Console.WriteLine("Searching for " + token);

            

            var orderedQuery = Query.cosineSimilarity(Query.convertQuery(textBox1.Text.TrimStart()), res.data).OrderBy(c => c.Value);

            foreach (KeyValuePair<string, double> kvp in orderedQuery)
            {
                richTextBox1.AppendText($"{kvp.Key} \n");
               // Console.WriteLine(" doc:{0}  weight:{1}", kvp.Key, kvp.Value);
            }
            stopwatch.Stop();

            textBox2.Text = stopwatch.ElapsedMilliseconds.ToString();


            Console.WriteLine("Done");
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            var files = Directory.EnumerateFiles(@"D:\Documents\DataSet\", "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".pdf") || s.EndsWith(".doc")
            || s.EndsWith(".docx") || s.EndsWith(".ppt") || s.EndsWith(".ppts") ||
            s.EndsWith(".xls") || s.EndsWith(".xlsx") || s.EndsWith(".txt") ||
            s.EndsWith(".html") || s.EndsWith(".xml"));


            string[] filearr = files.ToArray();

            Console.WriteLine("Clicked!!"+filearr.Length);


            res = StringIndexer.CreateIndex(filearr);

            Console.WriteLine("\n\nWorked");




            AutoCompleteStringCollection sourceName = new AutoCompleteStringCollection();

            foreach (KeyValuePair<string, Dictionary<string, int>> kvp in res)
            {

                sourceName.Add(kvp.Key);
            }

            textBox1.AutoCompleteCustomSource = sourceName;
            
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
