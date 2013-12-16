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



namespace _1023
{
    class Program1023
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
            return int.Parse(ReadLine());
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
                ulong n = ReadLineUlong();

#if ONLINE_JUDGE
#else
                if (n <= 0)
                    break;
#endif

                ulong res = Solve(n);
#if ONLINE_JUDGE
                Console.WriteLine(res);
#else
                Console.WriteLine(string.Format("for {0} ans: {1}", n, res));
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


        static ulong Solve(ulong n)
        {
            if (n == 1)
                return 0;
            if (n == 2)
                return 1;

            ulong edge = n/2;

            for (ulong i = 3; i <= edge; i++)
                if (n%i == 0)
                    return i - 1;

            return n - 1;
        }
    }
}