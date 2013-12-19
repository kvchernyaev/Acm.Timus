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



namespace _15_1102_191
{
    class Program_15_1102_191
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


        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
#if ONLINE_JUDGE
#else
            do
            {
#endif
            int? cnt = ReadIntNLine();
            if (cnt == null || cnt.Value <= 0)
#if ONLINE_JUDGE
                return;
#else
                    break;
#endif

            for (int i = 0; i < cnt; i++)
            {
                bool res = SolveByChars();
                Console.WriteLine(res ? Yes : No);
            }

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


        const string Yes = "YES";
        const string No = "NO";

        static string[] Ws = new string[] {"one", "puton", "out", "output", "in", "input"};


        static bool SolveByChars()
        {
            bool canStart = true;

            int[] indexes = new int[Ws.Length];
            for (int i = 0; i < indexes.Length; i++)
                indexes[i] = -1;

            char c = ReadChar();

            do
            {
                if (c == '\r')
                {
                    c = ReadChar();
                    return canStart;
                }
                if (c == '\n' || c == (char) 26)
                    return canStart;

                bool wasEnd = false;

                for (int i = 0; i < indexes.Length; i++)
                {
                    int indW = indexes[i];
                    if (indW < 0)
                        continue;

                    indW++;

                    string w = Ws[i];

                    if (w[indW] != c)
                        indexes[i] = -1;
                    else
                    {
                        indexes[i] = indW;
                        if (indW == w.Length - 1)
                        {
                            indexes[i] = -1;
                            wasEnd = true;
                        }
                    }
                }

                if (canStart)
                    for (int i = 0; i < Ws.Length; i++)
                        if (Ws[i][0] == c)
                            indexes[i] = 0;

                c = ReadChar();
                canStart = wasEnd;
            } while (true);
        }


        static string Solve(string s)
        {
            // может ли s быть конкатенацией над Ws ?

            Tested = new bool[s.Length];
            return Solve(s, 0) ? Yes : No;
        }


        static bool[] Tested;


        static bool Solve(string s, int startIndex)
        {
            if (startIndex == s.Length)
                return true;
            if (Tested[startIndex])
                return false;

            foreach (string ps in Ws.Where(w => Test(s, startIndex, w)))
                if (Solve(s, startIndex + ps.Length))
                    return true;

            Tested[startIndex] = true;
            return false;
        }


        static bool Test(string big, int ind, string piece)
        {
            //return big.IndexOf(piece, ind, StringComparison.InvariantCultureIgnoreCase) == ind;
            if (ind >= big.Length)
                return false;

            if (big.Length - ind < piece.Length)
                return false;

            for (int i = 0; i < piece.Length; i++)
                if (big[ind + i] != piece[i])
                    return false;
            return true;
        }
    }
}