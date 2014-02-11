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



namespace _28_1249_210
{
    /// <summary>
    /// http://acm.timus.ru/problem.aspx?num=1249
    /// </summary>
    class Program_28_1249_210
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
                int[] ar = ReadIntArray();
#if ONLINE_JUDGE
#else
                if (ar == null || ar.Length == 0)
                    break;
#endif

                rCnt = ar[0];
                cCnt = ar[1]; // <=3000

                matrix = new ulong[rCnt*cCnt/(sizeof (ulong)*8) + 1];
                used = new ulong[rCnt*cCnt/(sizeof (ulong)*8) + 1];

                for (int i = 0; i < rCnt; i++)
                {
                    ar = ReadIntArray();
                    for (int j = 0; j < cCnt; j++)
                    {
                        int val = ar[j];
                        SetBit(i, j, matrix, val == 1);
                    }
                }

                bool res = Solve();

                Console.WriteLine(res ? "Yes" : "No");

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


        static int rCnt;
        static int cCnt;

        static ulong[] matrix;
        static ulong[] used;

        const ulong one = 1;


        static bool Val(int r, int c, ulong[] ar)
        {
            int bitindex = cCnt*r + c;
            int bitoffset;
            int byteindex = Math.DivRem(bitindex, sizeof (ulong)*8, out bitoffset);

            ulong b = ar[byteindex];
            bool bit = (b & (one << bitoffset)) > 0;
            return bit;
        }


        static bool Val(int r, int c)
        {
            bool bit = Val(r, c, matrix);
            return bit;
        }


        static bool IsUsed(int r, int c)
        {
            bool bit = Val(r, c, used);
            return bit;
        }


        static void SetUsed(int r, int c)
        {
            SetBit(r, c, used, true);
        }


        static void SetBit(int r, int c, ulong[] ar, bool bit)
        {
            int bitindex = cCnt*r + c;
            int bitoffset;
            int byteindex = Math.DivRem(bitindex, sizeof (ulong)*8, out bitoffset);

            ulong mask = one << bitoffset;
            ulong was = ar[byteindex];
            ar[byteindex] = bit ? (was | mask) : (was & ~mask);
        }


        static bool Solve()
        {
            for (int i = 0; i < rCnt; i++)
                for (int j = 0; j < cCnt; j++)
                {
                    if (!Val(i, j))
                        continue;
                    if (IsUsed(i, j))
                        continue;

                    if (!Check(i, j))
                        return false;
                }

            return true;
        }


        static Tuple<int, int>[] dirs = new Tuple<int, int>[]
        {
            new Tuple<int, int>(1, 1),
            new Tuple<int, int>(1, -1),
            new Tuple<int, int>(-1, 1),
            new Tuple<int, int>(-1, -1)
        };


        static bool Check(int i, int j)
        {
            if (IsUsed(i, j))
                return true;
            SetUsed(i, j);

            foreach (Tuple<int, int> dir in dirs)
                if (!Check(i, j, dir.Item1, dir.Item2))
                    return false;

            return true;
        }


        static bool Check(int i, int j, int dirR, int dirC)
        {
            if ((CheckBoundR(i + dirR) && Val(i + dirR, j)) && (CheckBoundC(j + dirC) && Val(i, j + dirC)))
                if (!Val(i + dirR, j + dirC))
                    return false;

            if (CheckBoundR(i + dirR) && Val(i + dirR, j))
                if (!Check(i + dirR, j))
                    return false;
            if (CheckBoundC(j + dirC) && Val(i, j + dirC))
                if (!Check(i, j + dirC))
                    return false;

            return true;
        }


        static bool CheckBoundR(int r)
        {
            return r >= 0 && r <= rCnt - 1;
        }


        static bool CheckBoundC(int c)
        {
            return c >= 0 && c <= cCnt - 1;
        }
    }
}