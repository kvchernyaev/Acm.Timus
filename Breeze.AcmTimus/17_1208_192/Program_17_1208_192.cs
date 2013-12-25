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


        static void SetBit(ref uint number, int bitIndex)
        {
            int mask = 1 << bitIndex;
            number = (uint) (number | mask);
        }


        static void SetBitOff(ref uint number, int bitIndex)
        {
            int mask = ~(1 << bitIndex);
            number = (uint) (number & mask);
        }


        static bool IsNeibour(IEnumerable<string> a, IEnumerable<string> b)
        {
            return !a.Intersect(b).Any();
        }


        static bool TestBit(uint number, int bitIndex)
        {
            int mask = 1 << bitIndex;
            return (number & mask) != 0;
        }


        static uint[] _graf;

        static uint _usedV = 0;


        static int SolveByBitmask(uint[] graf)
        {
            _graf = graf;

            List<uint> maxSubgrafs = new List<uint>();

            do
            {
                // choose not used vertex
                int v = FindFirstNulBit(_usedV);
                if (v < 0)
                    break;

                uint maxSubgraf = FindMaxFullSubgraf(v);
                maxSubgrafs.Add(maxSubgraf);
                _usedV |= maxSubgraf;
            } while (true);

            int maxSize = maxSubgrafs.Max(subgraf => SizeOfSubgraf(subgraf, _graf.Length));
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


        static uint FindMaxFullSubgraf(int startV)
        {
            uint currentGraf = 0;
            uint commonNeibours = uint.MaxValue;

            int currentV = startV;

            do
            {
                SetBit(ref currentGraf, currentV);
                uint neibours = _graf[currentV];
                commonNeibours &= neibours;
                currentV = FindFirstNotNulBit(commonNeibours);
                if (currentV < 0)
                    break;
            } while (true);

            return currentGraf;
        }


        static int FindFirstNulBit(uint number)
        {
            for (int i = 0; i < _graf.Length; i++)
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
    }
}