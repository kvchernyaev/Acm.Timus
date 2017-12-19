using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;


namespace Timus_31_1024
{
    internal class Program
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


        static string InputFilePath
        {
            get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Input.txt"); }
        }
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
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(decimal.Parse)
                .ToArray();
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



        static void Log(string s, params object[] objs)
        {
#if ONLINE_JUDGE
#else
            Console.WriteLine(string.Format(s, objs));
#endif
        }
        #endregion


        private static long[] p;


        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;


            int count = ReadIntLine();
            p = ReadLongArray();

            Log(calcNok(6,9,1,2,4).ToString());
            //int k = StraightSolve();
            long k = QuickSolve();

            Console.WriteLine(k);
        }


        static long QuickSolve()
        {
            long[] q = new long[p.Length];
            for (int i = 0; i < q.Length; i++)
                q[p[i] - 1] = i;

            printcur(q, true, true, "inverted P: ");

            long[] factors = new long[p.Length];

            long cur = 0;
            for (int fi = 0; fi < p.Length; fi++)
            {
                cur = fi;

                int k = 0;
                do
                {
                    cur = q[cur];
                    k++;
                } while (cur != fi);
                factors[fi] = k;
            }

            printcur(factors, false, true, "factors: ");

            long nok = calcNok(factors);

            return nok;
        }


        private static long calcNok(params long[] v)
        {
            long[] cut = v.Where(x => x > 1).Distinct().ToArray();
            printcur(cut, false, false, "calcNok: ");

            if (v.Length == 0) return 1;
            if (v.Length == 1) return v[0];

            long nok = v[0];
            
            for (int i = 1; i < v.Length; i++)
            {
                nok = calcNok(nok, v[i]);
            }

            return nok;
        }


        static long calcNok(long a, long b)
        {
            return mlc(a, b);
        }
        static long gcd(long a, long b) { return a>0 ? gcd(b%a, a) : b; }
        static long mlc(long a, long b) { return a*b / gcd(a, b); }



        private static int StraightSolve()
        {
            int[] prev = new int[p.Length];
            int[] pprev = new int[p.Length];
            int[] cur = new int[p.Length];
            Array.Copy(p, cur, cur.Length);
            Array.Copy(p, prev, cur.Length);
            Array.Copy(p, pprev, cur.Length);
            for (int j = 0; j < cur.Length; j++)
                p[j]--;

            int i = 1;
            while (!isOne(cur))
            {
                printcur(cur);
                cur = Apply(prev, pprev);
                i++;
                pprev = prev;
                prev = cur;
            }
            printcur(cur);
            return i;
        }


        static void printcur(long[] cur, bool addOne = false, bool withIndex = true, string prefix = "")
        {
#if ONLINE_JUDGE
#else
            if (!string.IsNullOrEmpty(prefix)) Console.Write(prefix);
            Console.WriteLine(string.Join(" ", cur.Select((x, i) =>
                string.Format(withIndex ? "{0}:{1}" : "{1}", i,
                    (x + (addOne ? 1 : 0)).ToString()))));
#endif
        }
        static void printcur(int[] cur, bool addOne = false, bool withIndex = true, string prefix = "")
        {
#if ONLINE_JUDGE
#else
            if (!string.IsNullOrEmpty(prefix)) Console.Write(prefix);
            Console.WriteLine(string.Join(" ", cur.Select((x, i) =>
                string.Format(withIndex ? "{0}:{1}" : "{1}", i,
                    (x + (addOne ? 1 : 0)).ToString()))));
#endif
        }


        static bool isOne(int[] cur)
        {
            for (int i = 0; i < cur.Length; i++)
                if (cur[i] != i + 1)
                    return false;
            return true;
        }


        private static int[] Apply(int[] cur, int[] rv)
        {
            for (int i = 0; i < cur.Length; i++)
                rv[i] = cur[p[i]];
            return rv;
        }
    }
}