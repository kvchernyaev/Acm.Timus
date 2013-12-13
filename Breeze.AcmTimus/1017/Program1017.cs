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



namespace _1017
{
    class Program1017
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


        static decimal[] ReadDecimalArray()
        {
            string s = ReadLine();
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(x => decimal.Parse(x)).ToArray();
        }


        static int ReadLineInt()
        {
            return int.Parse(ReadLine());
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
                int n = ReadLineInt();

#if ONLINE_JUDGE
#else
                if (n < 0)
                    break;
#endif

                long res = Solve(n);
#if ONLINE_JUDGE
                Console.WriteLine(res);
#else
                Console.WriteLine(string.Format("{0} : {1}", n, res));
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


        /*
         * <=2 => 0
         * 3,4 => 1
         * */


        static long Solve(int n)
        {
            if (n == 0)
                return 0;

            long rv = 0;
            for (int first = 1; first <= n/2; first++)
                rv += L(n, first);

            return rv;
        }


        static long L(int total, int first)
        {
            int left = total - first;

            if (left < 0)
                return 0;
            if (left == 0)
                return 1;
            if (left < first + 1)
                return 0;

            if (left <= first + 2)
                return 1;

            long sum = 0;

            for (int current = first; current <= total/2; current++)
                sum += L(total - current, current + 1);

            sum++; // current==total, all in one tower
            return sum;
        }
    }
}