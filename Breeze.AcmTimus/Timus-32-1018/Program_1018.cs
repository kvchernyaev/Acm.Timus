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
    internal class Program
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
            int n = a[0], qLeft = a[1], q = n - qLeft;

            _elems = new Elem[n + 1];
            for (int i = 1; i < _elems.Length; i++)
                _elems[i] = new Elem(i);

            for (int i = 0; i < n - 1; i++)
            {
                a = ReadIntArray();
                Input inp = new Input(a);

                _elems[inp.a].Childs.Add(new Child(parentI: inp.a, childI: inp.b, w: inp.w));
                _elems[inp.b].Childs.Add(new Child(parentI: inp.b, childI: inp.a, w: inp.w));
            }

            Print(_elems);
            Log("---------------------------");

            ClearBackLink();

            Print(_elems);
            Log("---------------------------");
        }


        private static void ClearBackLink(int curI = 1, int? parentI = null)
        {
            Elem cure = _elems[curI];
            cure.Childs.RemoveAll(c => c.ChildI == parentI);
            cure.Childs.ForEach(c => ClearBackLink(c.ChildI, curI));
            cure.ParentI = parentI;
        }


        private static Elem[] _elems;



        class Elem
        {
            public readonly List<Child> Childs = new List<Child>(3);
            public int I;
            public int? ParentI = null;


            public Elem(int index)
            {
                I = index;
            }
        }



        [DebuggerDisplay("{ChildI}->{ParentI}")]
        struct Child
        {
            public Child(int parentI, int childI, int w)
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
                    $"{i}: parent {ar[i].ParentI ?? 0}, childs: {string.Join("; ", ar[i].Childs.Select(l => $"{l.ChildI}->{l.ParentI} ({l.W})"))}");
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