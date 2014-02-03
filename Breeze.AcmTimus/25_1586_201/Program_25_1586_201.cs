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



namespace _25_1586_201
{
    /// <summary>
    /// http://acm.timus.ru/problem.aspx?num=1586
    /// </summary>
    class Program_25_1586_201
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


        static uint[] primes3 = new uint[143]
        {
            101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173,
            179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257,
            263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349,
            353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439,
            443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541,
            547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631,
            641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733,
            739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829,
            839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941,
            947, 953, 967, 971, 977, 983, 991, 997
        }; // 11 with 0 in the middle


        static ulong mod = 1000000009;


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

                int n = nN.Value; // 3 <= n <= 10 000

                ulong rv = SolveMatrix(n);

#if ONLINE_JUDGE
                Console.WriteLine(rv);
#else
                Console.WriteLine(string.Format("{1} : {0}", rv, n));
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


        static List<int>[] nextPrimes3;


        static ulong Solve(int n)
        {
            //uint[] ordered = primes3.OrderBy(p => p%100).ToArray();

            nextPrimes3 = new List<int>[primes3.Length];

            for (int i = 0; i < primes3.Length; i++)
            {
                nextPrimes3[i] = new List<int>();

                for (int j = 0; j < primes3.Length; j++)
                    if (primes3[i]%100 == primes3[j]/10)
                        nextPrimes3[i].Add(j);
            }

            int count = n - 3;

            ulong total = 0;

            for (int i = 0; i < primes3.Length; i++)
            {
                ulong forOne = Recurr(i, count);
                total += forOne;
                if (total > mod)
                    total %= mod;
            }

            return total;
        }


        static ulong Recurr(int pIndex, int count)
        {
            if (count == 0)
                return 1;

            List<int> next = nextPrimes3[pIndex];

            if (count == 1)
                return (ulong) next.Count;

            count--;

            ulong total = 0;
            foreach (int nextIndex in next)
            {
                ulong forOne = Recurr(nextIndex, count);
                total += forOne;
                if (total >= mod)
                    total %= mod;
            }

            return total;
        }


        static ulong SolveMatrix(int n)
        {
            ulong[,] matrix = new ulong[143,143];

            for (int i = 0; i < 143; i++)
                for (int j = 0; j < 143; j++)
                    matrix[i, j] = (ulong) (primes3[i]%100 == primes3[j]/10 ? 1 : 0);

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            ulong[,] res = Pow(matrix, n - 3);
            sw.Stop();
            Log("_ " + sw.ElapsedMilliseconds);

            ulong rv = Sum(res);

            return rv;
        }


        static ulong Sum(ulong[,] m)
        {
            ulong rv = 0;
            foreach (ulong v in m)
            {
                rv += v;
                if (rv > mod)
                    rv %= mod;
            }
            return rv;
        }


        static ulong[,] Pow(ulong[,] m, int pow)
        {
            if (pow == 0)
                return Unity();
            if (pow == 1)
                return m;

//            var res = (ulong[,]) m.Clone();
//            for (int p = 0; p < pow - 1; p++)
//                res = Mult(res, m);
//            return res;

            int n = pow;
            ulong[,] curPowed = m;
            ulong[,] res = Unity();

            while (n > 0)
            {
                if (n%2 == 1)
                    res = Mult(res, curPowed);
                n /= 2;
                curPowed = Mult(curPowed, curPowed);
            }

            return res;
        }


        static ulong[,] Unity()
        {
            ulong[,] unity = new ulong[primes3.Length,primes3.Length];
            for (int i = 0; i < 143; i++)
                unity[i, i] = 1;
            return unity;
        }


        static ulong[,] Mult(ulong[,] m1, ulong[,] m2)
        {
            ulong[,] res = new ulong[143,143];

            for (int i = 0; i < 143; i++)
                for (int j = 0; j < 143; j++)
                {
                    ulong val = 0;
                    for (int k = 0; k < 143; k++)
                        val += m1[i, k]*m2[k, j];
                    if (val > mod)
                        val %= mod;
                    res[i, j] = val;
                }
            return res;
        }
    }
}