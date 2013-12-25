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



namespace _18_1280_192
{
    class Program_18_1280_192
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
                string s = ReadLine();
#if ONLINE_JUDGE
#else
                if (s == "")
                    break;
#endif
                int[] ar = s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                int subjCount = ar[0];
                int limCount = ar[1];
                Tuple<int, int>[] lims = new Tuple<int, int>[limCount];

                for (int i = 0; i < limCount; i++)
                {
                    ar = ReadIntArray();
                    lims[i] = new Tuple<int, int>(ar[0], ar[1]);
                }

                ar = ReadIntArray();

                bool res = Solve(lims, ar);

                Console.WriteLine(res ? "YES" : "NO");

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


        static bool Solve(IEnumerable<Tuple<int, int>> limitations, int[] proposedOrder)
        {
            foreach (Tuple<int, int> limitation in limitations)
            {
                int less = limitation.Item1;
                int greater = limitation.Item2;
                if (less == greater)
                    return false; // wrong rule

                foreach (int currentSubj in proposedOrder)
                {
                    if (currentSubj == greater)
                        return false; // first is greater, so rule is contradicted
                    if (currentSubj == less)
                        break; // first is smaller, so rule is satisfied
                }
            }
            return true; // all rules was satisfied
        }


        #region bit util
        static void SetBit(ref uint number, int bitIndex)
        {
            int mask = 1 << bitIndex;
            number = (uint) (number | mask);
        }


        static uint SetBit(uint number, int bitIndex)
        {
            return (uint) (number | (1 << bitIndex));
        }


        static void SetBitOff(ref uint number, int bitIndex)
        {
            int mask = ~(1 << bitIndex);
            number = (uint) (number & mask);
        }


        static uint SetBitOff(uint number, int bitIndex)
        {
            return (uint) (number & (~(1 << bitIndex)));
        }


        static bool TestBit(uint number, int bitIndex)
        {
            int mask = 1 << bitIndex;
            return (number & mask) != 0;
        }


        static int FindFirstNulBit(uint number, int max)
        {
            for (int i = 0; i < max; i++)
                if (!TestBit(number, i))
                    return i;
            return -1;
        }


        static int FindFirstNulBit(uint number, int max, int startIndex)
        {
            for (int i = startIndex; i < max; i++)
                if (!TestBit(number, i))
                    return i;
            return -1;
        }


        static int FindFirstNotNulBit(uint number, int max)
        {
            for (int i = 0; i < max; i++)
                if (TestBit(number, i))
                    return i;
            return -1;
        }


        static int FindFirstNotNulBit(uint number, int max, int startIndex)
        {
            for (int i = startIndex; i < max; i++)
                if (TestBit(number, i))
                    return i;
            return -1;
        }


        static uint OneBit(int index)
        {
            uint number = 0;
            SetBit(ref number, index);
            return number;
        }
        #endregion
    }
}