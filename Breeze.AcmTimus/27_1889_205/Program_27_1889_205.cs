#region usings
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion



namespace _27_1889_205
{
    /// <summary>
    /// http://acm.timus.ru/problem.aspx?num=1889
    /// </summary>
    class Program_27_1889_205
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


        static char ReadChar()
        {
            int code = Console.Read();
            if (code < 0)
                return (char) 26;
            return (char) code;
            ConsoleKeyInfo key = Console.ReadKey(false);
            return key.KeyChar;
        }
#else
        static string ReadAll()
        {
            return File.ReadAllText(InputFilePath);
        }


        static int LineIndex = 0;
        static int CharIndex = 0;


        static string ReadLine()
        {
            string[] lines = File.ReadAllLines(InputFilePath);
            if (LineIndex < lines.Length)
            {
                string rv = lines[LineIndex++];
                if (rv.StartsWith("//"))
                    return ReadLine();

                rv = rv.Trim();
                int i = rv.LastIndexOf("//");
                if (i >= 0)
                    return rv.Substring(0, i).Trim();
                return rv;
            }

            return "";
        }


        static char ReadChar()
        {
            string[] lines = File.ReadAllLines(InputFilePath);
            if (LineIndex < lines.Length)
            {
                string rv = lines[LineIndex];
                if (rv.StartsWith("//"))
                    rv = ReadLine();

                rv = rv.Trim();
                int i = rv.LastIndexOf("//");
                if (i >= 0)
                    rv = rv.Substring(0, i).Trim();

                if (CharIndex < rv.Length)
                    return rv[CharIndex++];

                CharIndex = 0;
                LineIndex++;
                return '\n';
            }

            return '\n';
        }


        static string InputFilePath { get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Input.txt"); } }
#endif


        #region read specific
        static string ReadLineTrim()
        {
            return ReadLine().Trim();
        }


        static int[] ReadIntArray()
        {
            string s = ReadLine();
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }


        static long[] ReadLongArray()
        {
            string s = ReadLine();
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        }


        static ulong[] ReadUlongArray()
        {
            string s = ReadLine();
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray();
        }


        static decimal[] ReadDecimalArray()
        {
            string s = ReadLine();
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(decimal.Parse).ToArray();
        }


        static int ReadIntLine()
        {
            string s = ReadLine();
            return int.Parse(s);
        }


        static int? ReadIntNLine()
        {
            string s = ReadLine();
            s = s.TrimStart((char) 0xEF, (char) 0xBB, (char) 0xBf, (char) 1103, (char) 9559, (char) 9488);

            if (string.IsNullOrEmpty(s))
                return null;
            return int.Parse(s);
        }


        static decimal ReadDecimalLine()
        {
            string s = ReadLine();
            return decimal.Parse(s);
        }


        static long ReadLongLine()
        {
            return long.Parse(ReadLine());
        }


        static ulong ReadUlongLine()
        {
            return ulong.Parse(ReadLine());
        }
        #endregion


        #endregion


        static void Log(string s, params object[] objs)
        {
#if ONLINE_JUDGE
#else
            Console.WriteLine(string.Format(s, objs));
#endif
        }


        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
#if ONLINE_JUDGE
#else
            do
            {
#endif
                int? nN = ReadIntNLine();
#if ONLINE_JUDGE
#else
                if (nN == null)
                    break;
#endif
                int n = nN.Value;
                var ar = new string[n];

                for (int i = 0; i < n; i++)
                {
                    string s = ReadLineTrim();
                    ar[i] = s;
                }
                List<int> res = Solve(ar);

                if (res == null || res.Count == 0)
                    Console.WriteLine("Igor is wrong.");
                else
                    Console.WriteLine(string.Join(" ", ((IEnumerable<int>)res).Reverse()));

#if ONLINE_JUDGE
#else
            } while (true);
#endif

#if ONLINE_JUDGE
#else
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
#endif
        }


        static List<int> Solve(string[] ar)
        {
            Dictionary<string, List<int>> known = new Dictionary<string, List<int>>();
            Dictionary<int, string> indexes = new Dictionary<int, string>();

            for (int i = 0; i < ar.Length; i++)
            {
                string s = ar[i];
                if (s == "unknown")
                    continue;

                List<int> l;
                if (!known.TryGetValue(s, out l))
                    known.Add(s, l = new List<int>());

                l.Add(i);

                indexes.Add(i, s);
            }

            List<Tuple<int, int>> minmaxs = known.Select(kvp => new Tuple<int, int>(kvp.Value.Min(), kvp.Value.Max())).ToList();

            if (!Check(minmaxs))
                return null;

            List<int> res = new List<int>();

            for (int intervalSize = 1; intervalSize <= ar.Length/2; intervalSize++)
                if (ar.Length%intervalSize == 0 &&
                    Check(intervalSize, ar.Length, known, indexes))
                    res.Add(ar.Length/intervalSize);
            if (Check(ar.Length, ar.Length, known, indexes))
                res.Add(1);

            return res;
        }


        static bool Check(int intervalSize, int total, Dictionary<string, List<int>> known, Dictionary<int, string> indexes)
        {
            if (intervalSize == 1)
                return known.All(kvp => kvp.Value.Count == 1);
            if (intervalSize == total)
                return known.Count <= 1;

            Dictionary<string, List<int>> knownByIntervals = known.ToDictionary(kvp => kvp.Key,
                                                                                kvp =>
                                                                                kvp.Value.Select(ind => ind / intervalSize)
                                                                                .Distinct().ToList());

            // 1 язык в разных интервалах
            if (knownByIntervals.Any(kvp => kvp.Value.Count > 1))
                return false;

            var a = knownByIntervals.Values.SelectMany(l => l).GroupBy(i => i, (i, l) => l.Count()).ToArray();
            // в 1м интервале более одного языка
            if (knownByIntervals.Values.SelectMany(l => l).GroupBy(i => i, (i, l) => l.Count()).Any(cnt => cnt > 1))
                return false;

            return true;
        }


        static bool Check(List<Tuple<int, int>> minmaxs)
        {
            for (int i = 0; i < minmaxs.Count - 1; i++)
                for (int j = i + 1; j < minmaxs.Count; j++)
                    if (minmaxs[i].Item2 >= minmaxs[j].Item1 && minmaxs[i].Item1 <= minmaxs[j].Item2)
                        return false;
            return true;
        }
    }
}