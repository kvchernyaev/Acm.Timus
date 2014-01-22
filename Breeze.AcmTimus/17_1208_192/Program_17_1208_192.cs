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



namespace _17_1208_192
{
    /// <summary>
    /// http://acm.timus.ru/problem.aspx?num=1208
    /// </summary>
    class Program_17_1208_192
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
                int? kn = ReadIntNLine();
#if ONLINE_JUDGE
#else
                if (kn == null || kn.Value < 0)
                    break;
#endif
                int k = kn.Value;

                string[][] commands = new string[k][];
                for (int i = 0; i < k; i++)
                {
                    string[] ps = ReadLine().Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                    commands[i] = ps;
                }

                int res = Solve(commands);
#if ONLINE_JUDGE
                Console.WriteLine(res);
#else
                Console.WriteLine(res);
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


        static int Solve(string[][] commands)
        {
            if (commands.Length == 0)
                return 0;
            if (commands.Length == 1)
                return 1
                    ;
            uint[] neibours = new uint[commands.Length];

            for (int i = 0; i < commands.Length - 1; i++)
                for (int j = i + 1; j < commands.Length; j++)
                    if (IsNeibour(commands[i], commands[j]))
                    {
                        SetBit(ref neibours[i], j);
                        SetBit(ref neibours[j], i);
                    }

            int res = SolveByBitmask(neibours);
            return res;
        }


        static bool IsNeibour(IEnumerable<string> a, IEnumerable<string> b)
        {
            return !a.Intersect(b).Any();
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


        static int FindFirstNulBit(uint number)
        {
            for (int i = 0; i < _graf.Length; i++)
                if (!TestBit(number, i))
                    return i;
            return -1;
        }


        static int FindFirstNulBit(uint number, int startIndex)
        {
            for (int i = startIndex; i < _graf.Length; i++)
                if (!TestBit(number, i))
                    return i;
            return -1;
        }


        static int FindFirstNotNulBit(uint number)
        {
            for (int i = 0; i < _graf.Length; i++)
                if (TestBit(number, i))
                    return i;
            return -1;
        }


        static int FindFirstNotNulBit(uint number, int startIndex)
        {
            for (int i = startIndex; i < _graf.Length; i++)
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


        static uint[] _graf;

        static List<uint> _maxSubgrafs;


        static int SolveByBitmask(uint[] graf)
        {
            _graf = graf;
            _maxSubgrafs = new List<uint>();

            for (int currentStartV = 0; currentStartV < graf.Length; currentStartV++)
                FindMaxFullSubgrafs(currentStartV);

            int maxSize = _maxSubgrafs.Max(subgraf => SizeOfSubgraf(subgraf, _graf.Length));
            return maxSize;
        }


        static int SizeOfSubgraf(uint subgraf, int grafSize)
        {
            int cnt = 0;
            for (int i = 0; i < grafSize; i++)
                if (TestBit(subgraf, i))
                    cnt++;
            return cnt;
        }


        /// <summary>
        /// Adds to _maxSubgrafs all subgrafs wich contain given point and does not contain all earlier
        /// </summary>
        static void FindMaxFullSubgrafs(int startV)
        {
            FindMaxFullSubgrafsRecur(OneBit(startV), _graf[startV], startV);
        }


        static void FindMaxFullSubgrafsRecur(uint currentGraf, uint commonNeibours, int startToSearchV)
        {
            if (commonNeibours == 0)
            {
                _maxSubgrafs.Add(currentGraf);
                return;
            }

            int nextV = startToSearchV;

            do
            {
                nextV = FindFirstNotNulBit(commonNeibours, nextV + 1);
                if (nextV < 0)
                    return;

                FindMaxFullSubgrafsRecur(SetBit(currentGraf, nextV), commonNeibours & _graf[nextV], nextV);
            } while (true);
        }
    }
}