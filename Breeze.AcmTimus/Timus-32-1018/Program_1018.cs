using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;


namespace Timus_32_1018
{
    internal static class Program1018
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


        static string InputFilePath
        {
            get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Input.txt"); }
        }
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
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(decimal.Parse)
                    .ToArray();
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



        static void Log(string s, params object[] objs)
        {
#if ONLINE_JUDGE
#else
            Console.WriteLine(string.Format(s, objs));
#endif
        }
        #endregion



        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            int[] a = ReadIntArray();
            int n = a[0], qLeft = a[1] + 1/*сохраняем ноды, а не ветки*/, q = n - qLeft/*сколько нод надо срезать*/;

            _elems = new Elem[n + 1];
            for (int i = 1; i < _elems.Length; i++)
                _elems[i] = new Elem(i);

            for (int i = 0; i < n - 1; i++)
            {
                a = ReadIntArray();
                Input inp = new Input(a);

                _elems[inp.a].Links.Add(new Link(parentI: inp.a, childI: inp.b, w: inp.w));
                _elems[inp.b].Links.Add(new Link(parentI: inp.b, childI: inp.a, w: inp.w));
            }

            Print(_elems);
            Log("---------------------------");

            SetupParent();
            SetupG();
            SetupW();

            Print(_elems);
            Log("---------------------------");

            // sort by G - to calc Min array leafs must be first, parents must be second
            int[] gs = _elems.Select(x => x?.G ?? 0).ToArray();
            _elemsSortedByG = _elems.ToArray();
            Array.Sort(gs, _elemsSortedByG, 1, n);
            Print(_elemsSortedByG);
            Log("---------------------------");

            SetupMin();
            PrintMin(_elemsSortedByG);
            Log("---------------------------");

            Log("answer:");
            Console.WriteLine(_elems[1].W - _elems[1].Min[q]);
        }


        private static void PrintMin(IReadOnlyList<Elem> ar)
        {
            Log("Min arrays:");
            for (int i = 1; i < ar.Count; i++)
            {
                Log($"{ar[i].I}({i}): parent {ar[i].ParentI ?? 0} W {ar[i].W} G {ar[i].G}");
                Log(string.Join(" ", ar[i].Min.Select((m, mi) => $"{mi}:{m}")));
            }
        }


        private static void SetupMin()
        {
            // elems are sorted by G
            foreach (Elem elem in _elemsSortedByG.Skip(1)) // 1..n
            {
                int[] min = elem.Min;
                // 0 .. elem.G, G: 1 if leaf .. n for root
                min[0] = 0;
                min[elem.G] = elem.W;
                for (int q = 1; q < elem.G; q++)
                {
                    if (elem.Links.Count == 0) break; // can not be here - elem.G==1
                    else if (elem.Links.Count == 1)
                    {
                        Elem elemTheOne = _elems[elem.Links[0].ChildI];
                        min[q] = elemTheOne.Min[q]; // it is already calced - because they are sorted by G
                    }
                    else if (elem.Links.Count == 2)
                    {
                        int[] formin = new int[q + 1];
                        for (int qLeft = 0; qLeft <= q; qLeft++)
                        {
                            int qRight = q - qLeft;
                            Elem elemLeft = _elems[elem.Links[0].ChildI];
                            Elem elemRight = _elems[elem.Links[1].ChildI];

                            if (elemLeft.G < qLeft) continue;
                            if (elemRight.G < qRight) continue;

                            int[] minLeft = elemLeft.Min;
                            int[] minRight = elemRight.Min;

                            formin[qLeft] = minLeft[qLeft] + minRight[qRight];
                        }
                        min[q] = formin.Where(x => x > 0).Min();
                    }
                    else
                        throw new Exception(
                            $"elem {elem.I} has {elem.Links.Count} childs - can calc only binary three");
                }
            }
        }


        private static int SetupG(int curI = 1)
        {
            int g = _elems[curI].Links.Sum(x => SetupG(x.ChildI)) + 1;
            _elems[curI].G = g;
            _elems[curI].Min = new int[g + 1]; // index 0..g
            return g;
        }


        private static int SetupW(int curI = 1)
        {
            Elem elem = _elems[curI];
            int w = elem.Links.Sum(x => SetupW(x.ChildI)) +
                    (elem.ParentI == null
                        ? 0
                        : _elems[elem.ParentI.Value].Links.FirstOrDefault(x => x.ChildI == curI).W);
            elem.W = w;
            return w;
        }


        private static void SetupParent(int curI = 1, int? parentI = null)
        {
            Elem cure = _elems[curI];
            cure.Links.RemoveAll(c => c.ChildI == parentI);
            cure.Links.ForEach(c => SetupParent(c.ChildI, curI));
            cure.ParentI = parentI;
        }


        private static Elem[] _elems;
        private static Elem[] _elemsSortedByG;



        class Elem
        {
            public int I;
            public int? ParentI = null;
            public readonly List<Link> Links = new List<Link>(3);

            /// <summary>
            /// Сколько потеряем веса при срезании этой ноды (вес всех снизу плюс вес родительской ветки)
            /// </summary>
            public int W;

            /// <summary>
            /// Сколько нод низу вместе с этой
            /// </summary>
            public int G;


            /// <summary>
            /// index: 0..G, len G+1
            /// </summary>
            public int[] Min;


            public Elem(int index)
            {
                I = index;
            }
        }



        [DebuggerDisplay("{ChildI}->{ParentI}")]
        struct Link
        {
            public Link(int parentI, int childI, int w)
            {
                ChildI = childI;
                W = w;
                ParentI = parentI;
            }


            public readonly int ParentI;
            public readonly int ChildI;
            public readonly int W;
        }



        private static void Print(IReadOnlyList<Elem> ar)
        {
            for (int i = 1; i < ar.Count; i++)
                Log(
                    $"{ar[i].I}({i}): parent {ar[i].ParentI ?? 0} W {ar[i].W} G {ar[i].G}, childs: {string.Join("; ", ar[i].Links.Select(l => $"{l.ChildI}->{l.ParentI} ({l.W})"))}");
        }



        class Input
        {
            public Input(int[] ar)
            {
                a = ar[0];
                b = ar[1];
                w = ar[2];
            }


            public int a;
            public int b;
            public int w;
        }
    }
}