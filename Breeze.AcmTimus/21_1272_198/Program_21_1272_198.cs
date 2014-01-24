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



namespace _21_1272_198
{
    /// <summary>
    /// http://acm.timus.ru/problem.aspx?num=1272
    /// </summary>
    class Program_21_1272_198
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

                int vertexCount = ar[0]; // <= 10000
                int tCount = ar[1]; // <=12000
                int mCount = ar[2]; // <=12000

                List<int>[] tGraf = ReadGraf(tCount, vertexCount);
                List<int>[] mGraf = ReadGraf(mCount, vertexCount);

                int neededM = Solve(tGraf, mGraf);

                Console.WriteLine(neededM);

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


        static List<int>[] ReadGraf(int tCount, int vertexCount)
        {
            List<int>[] graf = new List<int>[vertexCount];

            for (int t = 0; t < tCount; t++)
            {
                int[] ar = ReadIntArray();
                int v1 = ar[0] - 1;
                int v2 = ar[1] - 1;

                if (graf[v1] == null)
                    graf[v1] = new List<int>();
                graf[v1].Add(v2);
                if (graf[v2] == null)
                    graf[v2] = new List<int>();
                graf[v2].Add(v1);
            }

            return graf;
        }


        static int Solve(List<int>[] tGraf, List<int>[] mGraf)
        {
            int connectivityDomains = ConnectDomains(tGraf);

            return connectivityDomains - 1;
        }


        static int ConnectDomains(List<int>[] graf)
        {
            bool[] used = new bool[graf.Length];
            int usedCount = 0;

            int domains = 0;

            int kernelV = 0;

            do
            {
                kernelV = GetFirstFalse(used, kernelV);
                List<int> accessDomain = AccessDomain(graf, kernelV);

                domains++;
                usedCount += accessDomain.Count;
                foreach (int v in accessDomain)
                    used[v] = true;
            } while (usedCount < graf.Length);

            return domains;
        }


        static List<int> AccessDomain(List<int>[] graf, int kernelV)
        {
            bool[] domain = new bool[graf.Length];
            domain[kernelV] = true;

            List<int> l = new List<int>();
            l.Add(kernelV);

            do
            {
                int before = l.Count;

                var append = new List<List<int>>();
                foreach (int v in l)
                {
                    List<int> nei = graf[v];
                    if (nei != null && nei.Count > 0)
                        append.Add(nei);
                }

                Merge(domain, l, append);

                if (before == l.Count)
                    break;
            } while (true);

            return l;
        }


        static void Merge(bool[] domain, List<int> l, List<List<int>> add)
        {
            foreach (List<int> vs in add)
                foreach (int v in vs)
                    if (!domain[v])
                    {
                        domain[v] = true;
                        l.Add(v);
                    }
        }


        static int GetFirstFalse(bool[] ar, int first)
        {
            for (int i = first; i < ar.Length; i++)
                if (!ar[i])
                    return i;
            return -1;
        }
    }
}