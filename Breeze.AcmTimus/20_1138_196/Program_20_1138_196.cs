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



namespace _20_1138_196
{
    class Program_20_1138_196
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


        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
#if ONLINE_JUDGE
#else
            do
            {
#endif
                int[] ar = ReadIntArray();
#if ONLINE_JUDGE
#else
                if (ar.Length == 0)
                    break;
#endif

                int first = ar[1];
                _max = ar[0];
                _d = new Dictionary<int, List<int>>();

                List<int> res = Solve(first);
#if ONLINE_JUDGE
                Console.WriteLine(res.Count);
#else
                Console.WriteLine(string.Format("first {0} max {1} : {3} : {2}", first, _max, string.Join(" ", res), res.Count));
#endif
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


        static int _max;

        static Dictionary<int, List<int>> _d;


        static List<int> Solve(int first)
        {
            if (first == _max)
                return new List<int>() {first};

            int rem100 = first%100;
            int kbase = GetKbase(rem100);

            int k = kbase;

            List<int> maxH = new List<int>();

            do
            {
                int next = first*(100 + k)/100; // it is integer
                if (next > _max)
                    break;

                List<int> h;
                if (!_d.TryGetValue(next, out h))
                    _d[next] = h = Solve(next);

                if (h.Count > maxH.Count)
                    maxH = h;

                k += kbase;
            } while (true);

            return new int[] {first}.Concat(maxH).ToList();
        }


        static int GetKbase(int b)
        {
            // 0<= b <100
            if (b == 0)
                return 1;
            return 100/Nod100(b);
        }


        static int Nod100(int b)
        {
            int rv = 1;
            int bb = b;
            if (bb%2 == 0)
            {
                bb /= 2;
                rv *= 2;
                if (bb%2 == 0)
                {
                    bb /= 2;
                    rv *= 2;
                }
            }
            if (bb%5 == 0)
            {
                bb /= 5;
                rv *= 5;
                if (bb%5 == 0)
                {
                    bb /= 5;
                    rv *= 5;
                }
            }
            return rv;
        }
    }
}