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



namespace _13_1213_170
{
    class Program_13_1213_170
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
                string gate = ReadLine();
                if (gate == "#")
                {
                    Console.WriteLine("0");

#if ONLINE_JUDGE
                    return;
#else
                    break;
#endif
                }

#if ONLINE_JUDGE
#else
                if (gate == "")
                    break;
#endif

                Dictionary<string, List<string>> edges = new Dictionary<string, List<string>>();

                string s;
                while ((s = ReadLine()) != "#")
                {
                    string[] ss = s.Split(new char[] {'-', ' ', '\t'}, StringSplitOptions.RemoveEmptyEntries)
                                   .Select(x => x.Trim()).ToArray();
                    if (!edges.ContainsKey(ss[0]))
                        edges[ss[0]] = new List<string>();
                    edges[ss[0]].Add(ss[1]);

                    if (!edges.ContainsKey(ss[1]))
                        edges[ss[1]] = new List<string>();
                    edges[ss[1]].Add(ss[0]);
                }

                int res = Solve(edges, gate);
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


        static int Solve(Dictionary<string, List<string>> edges, string gate)
        {
            int n = edges.Count;
            return n == 0 ? 0 : (n - 1);
        }
    }
}