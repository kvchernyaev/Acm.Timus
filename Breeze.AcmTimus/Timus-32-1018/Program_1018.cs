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
            int n = a[0], q = a[1];

            elems = new Elem[n + 1];
            for (int i = 1; i < elems.Length; i++)
                elems[i] = new Elem();

            input[] inps = new input[n - 1];

            for (int i = 0; i < n - 1; i++)
            {
                a = ReadIntArray();
                input inp = new input(a);
                inps[i] = inp;

                elems[inp.a].Childs.Add(new Child(inp.a, inp.b, inp.w));
                elems[inp.b].Childs.Add(new Child(inp.b, inp.a, inp.w));
            }

            print(elems);
            Log("---------------------------");

            clearBackLink(1, -1);

            print(elems);
            Log("---------------------------");
            
            
            
        }


        private static void clearBackLink(int cur, int from)
        {
            Elem cure = elems[cur];
            cure.Childs.RemoveAll(c => c.Cur == from);
            cure.Childs.ForEach(c => clearBackLink(c.Cur, cur));
        }


        private static void print(Elem[] elems)
        {
            for (int i = 1; i < elems.Length; i++)
            {
                Log($"{i}: {string.Join("; ", elems[i].Childs.Select(l => $"{l.Cur}->{l.Parent} ({l.W})"))}");
            }
        }


        private static Elem[] elems;



        class Elem
        {
            public List<Child> Childs = new List<Child>(3);
        }



        [DebuggerDisplay("{Cur}->{Parent}")]
        struct Child
        {
            public Child(int parent, int cur, int w)
            {
                Cur = cur;
                W = w;
                Parent = parent;
            }


            public int Parent;
            public int Cur;
            public int W;
        }



        class input
        {
            public input(int[] ar)
            {
                a = ar[0];
                b = ar[1];
                w = ar[2];
            }


            public int a;
            public int b;
            public int w;
        }



        static void EnsureLess(ref int a, ref int b)
        {
            if (a > b) swap(ref a, ref b);
        }


        static void swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }
    }
}