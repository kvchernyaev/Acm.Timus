#region usings
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#endregion



namespace _1002
{
    class Program1002
    {
#if ONLINE_JUDGE
        static string ReadAll()
        {
            return Console.In.ReadToEnd();
        }
        static string ReadLine()
        {
            return Console.ReadLine();
        }
#else
        static string ReadAll()
        {
            return File.ReadAllText(InputFilePath);
        }


        static int LineIndex = 0;


        static string ReadLine()
        {
            string[] lines = File.ReadAllLines(InputFilePath);
            if (LineIndex < lines.Length)
            {
                string rv = lines[LineIndex++];
                rv = rv.Trim();
                int i = rv.LastIndexOf("//");
                if (i >= 0)
                    return rv.Substring(0, i).Trim();
                return rv;
            }

            return "";
        }


        static string InputFilePath { get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Input.txt"); } }
#endif


        static void Main(string[] args)
        {
            /*
             * phone
             * words count
             * word1
             * ...
             * wordn
             * 
             * ... (repeat)
             * 
             * -1
             * */

            do
            {
                string phone = ReadLine();
                if (phone == "-1")
                    break;
                int wordsCount = int.Parse(ReadLine());
                string[] words = new string[wordsCount];
                for (int i = 0; i < wordsCount; i++)
                    words[i] = ReadLine();

                Solve(phone, words);
            } while (true);

#if ONLINE_JUDGE
#else
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
#endif
        }


        /// <summary>
        /// 0 oqz
        /// 1 ij    2 abc   3 def
        /// 4 gh    5 kl    6 mn
        /// 7 prs   8 tuv   9 wxy
        /// </summary>
        static Dictionary<char, char[]> M = new Dictionary<char, char[]>
        {
            {'0', new[] {'o', 'q', 'z'}},
            {'1', new[] {'i', 'j'}},
            {'2', new[] {'a', 'b', 'c'}},
            {'3', new[] {'d', 'e', 'f'}},
            {'4', new[] {'g', 'h'}},
            {'5', new[] {'k', 'l'}},
            {'6', new[] {'m', 'n'}},
            {'7', new[] {'p', 'r', 's'}},
            {'8', new[] {'t', 'u', 'v'}},
            {'9', new[] {'w', 'x', 'y'}}
        };


        static void Solve(string phone, string[] words)
        {
            char[][] choices = phone.Select(c => M[c]).ToArray();

            List<List<string>> chains = SolveRecur(-1, choices, new List<string>(), words);

            if (chains.Count == 0)
                Console.WriteLine(NoSolution);
            else
            {
                int minLen = chains.Min(chain => chain.Count);
                List<string>[] minChains = chains.Where(chain => chain.Count == minLen).ToArray();

                Console.WriteLine(string.Join(" ", minChains[0]));
            }
        }


        // list of chaines
        static List<List<string>> SolveRecur(int letterIndPrev, char[][] choices, List<string> chain, string[] words)
        {
            if (letterIndPrev == choices.Length - 1)
                return new List<List<string>> {chain};

            List<List<string>> rv = new List<List<string>>();

            string[] reduced = words;
            for (int letterInd = letterIndPrev + 1, i = 0; letterInd < choices.Length; letterInd++,i++)
            {
                reduced = ReduceByNextChar(reduced, choices[letterInd], i);
                if (reduced.Length == 0)
                    break;

                string[] complete = reduced.Where(x => x.Length == i + 1).ToArray();
                if (complete.Length == 0)
                    continue;

                List<List<string>> l =
                    complete.SelectMany(completeWord => SolveRecur(letterInd, choices, new List<string>(chain) {completeWord}, words))
                            .ToList();

                rv.AddRange(l);
            }

            return rv;
        }


        const string NoSolution = "No solution.";


        static string[] ReduceByNextChar(IEnumerable<string> words, char[] nextChars, int index)
        {
            return words.Where(w => w.Length > index && nextChars.Contains(w[index])).ToArray();
        }
    }
}