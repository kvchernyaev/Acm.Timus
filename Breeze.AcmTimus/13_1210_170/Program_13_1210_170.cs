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



namespace _13_1210_170
{
    class Program_13_1210_170
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


        static int[] ReadIntArray()
        {
            string s = ReadLine();
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
        }


        static long[] ReadLongArray()
        {
            string s = ReadLine();
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToArray();
        }


        static ulong[] ReadUlongArray()
        {
            string s = ReadLine();
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(x => ulong.Parse(x)).ToArray();
        }


        static decimal[] ReadDecimalArray()
        {
            string s = ReadLine();
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(x => decimal.Parse(x)).ToArray();
        }


        static int ReadLineInt()
        {
            string s = ReadLine();
            return int.Parse(s);
        }


        static long ReadLineLong()
        {
            return long.Parse(ReadLine());
        }


        static ulong ReadLineUlong()
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
                int levels = ReadLineInt();

#if ONLINE_JUDGE
#else
                if (levels <= 0)
                    break;
#endif

                // level first -> i1 -> i2 -> cost
                Dictionary<int, int>[][] d = new Dictionary<int, int>[levels][];

                int prevplanets = 1;
                for (int l = 0; l < levels; l++)
                {
                    int planets = ReadLineInt();
                    d[l] = new Dictionary<int, int>[prevplanets];

                    int curPlanet = 0; // 0-based
                    for (int asf = 0; asf < planets; asf++)
                    {
                        int[] ar = ReadIntArray();
                        for (int i = 0; i < ar.Length/2; i++)
                        {
                            int prevPlanet = ar[2*i] - 1; // 0-based
                            int cost = ar[2*i + 1];

                            if (d[l][prevPlanet] == null)
                                d[l][prevPlanet] = new Dictionary<int, int>();
                            d[l][prevPlanet][curPlanet] = cost;
                        }
                        curPlanet++;
                    }

                    if (l < levels - 1)
                        ReadLine();

                    prevplanets = planets;
                }

                int res = Solve(d);
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


        static int Solve(Dictionary<int, int>[][] dGeneral)
        {
            // [planet1 -> planet2 -> cost]
            _cache = new Dictionary<KeyValuePair<int, int>, int>();
            _dGeneral = dGeneral;
            _levels = dGeneral.Length;

            return Solve(0, 0);
        }


        static Dictionary<int, int>[][] _dGeneral;
        static int _levels;

        static Dictionary<KeyValuePair<int, int>, int> _cache;


        static int Solve(int curLevel, int curPlanet)
        {
            var kvp = new KeyValuePair<int, int>(curLevel, curPlanet);
            int rv;
            if (_cache.TryGetValue(kvp, out rv))
                return rv;

            Dictionary<int, int>[] planets = _dGeneral[curLevel];

            Dictionary<int, int> edges = planets[curPlanet];

            int bestLeftCost = int.MaxValue;

            foreach (int nextPlanet in edges.Keys)
            {
                int cost = edges[nextPlanet];

                int leftCost = curLevel == _levels - 1
                                   ? cost
                                   : (cost + Solve(curLevel + 1, nextPlanet));

                if (leftCost < bestLeftCost)
                    bestLeftCost = leftCost;
            }

            _cache.Add(kvp, bestLeftCost);
            return bestLeftCost;
        }
    }
}