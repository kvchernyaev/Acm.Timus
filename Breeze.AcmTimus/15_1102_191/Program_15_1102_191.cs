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
                    string s = ReadLineTrim();
                    string res = Solve(s);
                    Console.WriteLine(res);
                    GC.Collect();
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