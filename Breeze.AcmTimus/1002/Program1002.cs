﻿#region usings
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion



namespace _1002
{
    class Program1002
    {
        #region console
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
        #endregion


        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

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

                Solve1(phone, words);
            } while (true);

#if ONLINE_JUDGE
#else
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
#endif
        }


//        /// <summary>
//        /// 0 oqz
//        /// 1 ij    2 abc   3 def
//        /// 4 gh    5 kl    6 mn
//        /// 7 prs   8 tuv   9 wxy
//        /// </summary>
//        static Dictionary<char, char[]> M = new Dictionary<char, char[]>
//        {
//            {'0', new[] {'o', 'q', 'z'}},
//            {'1', new[] {'i', 'j'}},
//            {'2', new[] {'a', 'b', 'c'}},
//            {'3', new[] {'d', 'e', 'f'}},
//            {'4', new[] {'g', 'h'}},
//            {'5', new[] {'k', 'l'}},
//            {'6', new[] {'m', 'n'}},
//            {'7', new[] {'p', 'r', 's'}},
//            {'8', new[] {'t', 'u', 'v'}},
//            {'9', new[] {'w', 'x', 'y'}}
//        };


        static Dictionary<char, char> Mback = new Dictionary<char, char>
        {
            {'a', '2'},
            {'b', '2'},
            {'c', '2'},
            {'d', '3'},
            {'e', '3'},
            {'f', '3'},
            {'g', '4'},
            {'h', '4'},
            {'i', '1'},
            {'j', '1'},
            {'k', '5'},
            {'l', '5'},
            {'m', '6'},
            {'n', '6'},
            {'o', '0'},
            {'p', '7'},
            {'q', '0'},
            {'r', '7'},
            {'s', '7'},
            {'t', '8'},
            {'u', '8'},
            {'v', '8'},
            {'w', '9'},
            {'x', '9'},
            {'y', '9'},
            {'z', '0'},
        };


        const string NoSolution = "No solution.";


        static void Solve1(string number, string[] words)
        {
            // number->word
            Dictionary<string, string[]> numbersToWords = words.GroupBy(ConvertToNumber, x => x)
                                                               .ToDictionary(g => g.Key, g => g.ToArray());

            string[] numbers = numbersToWords.Keys.ToArray();

            List<string> chain = SplitOptimal(number, numbers);

            if (chain == null || chain.Count == 0)
                Console.WriteLine(NoSolution);
            else
                Console.WriteLine(string.Join(" ", chain.Select(x => ConvertBack(x, numbersToWords))));
        }


        static string ConvertToNumber(string word)
        {
            // from alphabetic to digits
            return string.Join("", word.ToCharArray().Select(c => Mback[c]));
        }


        static string ConvertBack(string digits, Dictionary<string, string[]> numbersToWords)
        {
            // from digit to first matching aphabetic
            string[] rv;
            numbersToWords.TryGetValue(digits, out rv);
            if (rv != null && rv.Length > 0)
                return rv[0];
            return null;
        }


        static string[] _words;
        static string _bigword;
        static List<string>[,] _opt;
        static bool[,] _calced;


        static List<string> SplitOptimal(string bigword, string[] words)
        {
            _bigword = bigword;
            int len = bigword.Length;
            _opt = new List<string>[len,len];
            _calced = new bool[len,len];

            _words = words.ToArray();

            SplitOptimalRecur(0, len - 1);

            List<string> rv = _opt[0, len - 1];
            return rv;
        }


        static void SplitOptimalRecur(int l, int r)
        {
            if (_calced[l, r])
                return; // already calculated

            string whole = TryWhole(l, r);
            if (whole != null)
            {
                SetCalced(l, r, new List<string> {whole});
                return;
            }

            string[] lefts = PutOnLeft(l, r);

            if (lefts == null || lefts.Length == 0)
            {
                SetCalced(l, r, null);
                // если нечего поставить, то пути нет
                return;
            }

            List<string> currentOpt = null;

            foreach (string left in lefts)
            {
                int med = l + left.Length - 1; // last of left

                SetCalced(l, med, new List<string> {left});

                // solve on right
                SplitOptimalRecur(med + 1, r);
                if (_opt[med + 1, r] == null)
                    continue;

                if (_opt[med + 1, r].Count == 1)
                {
                    SetCalced(l, r, new List<string> {left, _opt[med + 1, r][0]});
                    return;
                }

                if (currentOpt == null || _opt[l, med].Count + _opt[med + 1, r].Count < currentOpt.Count)
                    currentOpt = _opt[l, med].Concat(_opt[med + 1, r]).ToList();
            }

            SetCalced(l, r, currentOpt);
        }


        static void SetCalced(int l, int r, List<string> val)
        {
            _opt[l, r] = val;
            _calced[l, r] = true;
        }


        static string TryWhole(int l, int r)
        {
            int len = r - l + 1;
            return _words.FirstOrDefault(x => x.Length == len && string.Compare(x, 0, _bigword, l, len) == 0);
        }


        static string[] PutOnLeft(int l, int r)
        {
            // возвращает по одному слову каждой длины, подходящему для слева в данных границах. Первыми короткие
            //string[] wlefts = _words.Where(__substring.StartsWith).GroupBy(x => x.Length).Select(g => g.First()).ToArray();

            int len = r - l + 1;
            string[] wlefts = _words.Where(w => w.Length <= len && string.Compare(w, 0, _bigword, l, w.Length) == 0)
                                    .GroupBy(w => w.Length).Select(g => g.First()).ToArray();
            return wlefts;
        }
    }
}