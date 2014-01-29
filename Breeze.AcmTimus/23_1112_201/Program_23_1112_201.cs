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



namespace _23_1112_201
{
    /// <summary>
    /// http://acm.timus.ru/problem.aspx?num=1112
    /// </summary>
    class Program_23_1112_201
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
                int? countN = ReadIntNLine();
#if ONLINE_JUDGE
#else
                if (countN == null)
                    break;
#endif

                int count = countN.Value;
                Cut[] cuts = new Cut[count];

                for (int i = 0; i < count; i++)
                {
                    int[] ar = ReadIntArray();
                    cuts[i] = new Cut(ar[0], ar[1], i);
                }

                List<int> res = Solve(cuts);

                Console.WriteLine(res.Count);
                foreach (int index in res.OrderBy(i => cuts[i].L))
                    Console.WriteLine(cuts[index].L + " " + cuts[index].R);

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



        public struct Cut
        {
            public Cut(int l, int r, int index)
            {
                if (l < r)
                {
                    L = l;
                    R = r;
                }
                else
                {
                    L = r;
                    R = l;
                }

                Index = index;
            }


            public int L;
            public int R;

            public int Index;


            public bool Intersects(Cut c2)
            {
                return this.L < c2.R && c2.L < this.R;
            }


            public static bool Intersects(Cut c1, Cut c2)
            {
                return c1.Intersects(c2);
            }
        }



        static List<int> Solve(Cut[] cuts)
        {
            List<int>[] complementaryGraf;
            List<int>[] graf = MakeGraf(cuts, out complementaryGraf);

            List<int> maxFullSubGraf = FindMaxFullSubGraf(complementaryGraf);
            return maxFullSubGraf;
        }


        static List<int>[] MakeGraf(Cut[] cuts, out List<int>[] complementaryGraf)
        {
            List<int>[] graf = new List<int>[cuts.Length];
            complementaryGraf = new List<int>[cuts.Length];

            for (int i = 0; i < cuts.Length; i++)
            {
                graf[i] = new List<int>();
                complementaryGraf[i] = new List<int>();
            }

            for (int i = 0; i < cuts.Length; i++)
                for (int j = i + 1; j < cuts.Length; j++)
                    if (Cut.Intersects(cuts[i], cuts[j]))
                    {
                        // i < j
                        graf[i].Add(j);
                        graf[j].Add(i);
                    }
                    else
                        // i < j
                        complementaryGraf[i].Add(j);
            //complementaryGraf[j].Add(i);
            return graf;
        }


        static List<int> FindMaxFullSubGraf(List<int>[] graf)
        {
            // lists are sorted, for each i graf[i] contains > i

            if (graf.Length == 0)
                return new List<int>();
            if (graf.Length == 1)
                return new List<int> {0};

            Graf = graf;
            Max = new List<int> {0};

            CurrentSubGraf = new int[graf.Length];

            int m = 0;
            List<int> mi = new List<int>();
            for (int i = 0; i < graf.Length; i++)
                if (graf[i].Count == m)
                    mi.Add(i);
                else if (graf[i].Count > m)
                {
                    m = graf[i].Count;
                    mi.Add(i);
                }

            foreach (int v0 in mi)
            {
                CurrentSubGraf[0] = v0;
                Recurce(1);
            }

//            for (int v0 = 0; v0 < graf.Length - 1; v0++)
//            {
//                CurrentSubGraf[0] = v0;
//                Recurce(1);
//            }

            return Max;
        }


        static List<int> Max;
        static List<int>[] Graf;
        static int[] CurrentSubGraf;


        static void Recurce(int currentSubGrafLen)
        {
            int prevV = CurrentSubGraf[currentSubGrafLen - 1];
            List<int> neibours = Graf[prevV];

            currentSubGrafLen++;

            foreach (int currentV in neibours)
            {
                CurrentSubGraf[currentSubGrafLen - 1] = currentV;

                List<int> newNeibours = Graf[currentV];
                List<int> commonNeibours = Intersect(neibours, newNeibours);

                if (commonNeibours.Count == 0)
                {
                    if (Max.Count < currentSubGrafLen)
                        Max = CurrentSubGraf.Take(currentSubGrafLen).ToList();
                }
                else if (commonNeibours.Count + currentSubGrafLen < Max.Count)
                    continue;
                else
                    Recurce(currentSubGrafLen);
            }
        }


        static List<int> Intersect(List<int> l1, List<int> l2)
        {
            return l1.Intersect(l2).ToList();
        }
    }
}