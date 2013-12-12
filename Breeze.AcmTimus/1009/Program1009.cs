#region usings
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion



namespace _1009
{
    class Program1009
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
        #endregion


        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
#if ONLINE_JUDGE
#else
            do
            {
#endif
                int n = int.Parse(ReadLine()) - 1;
#if ONLINE_JUDGE
#else
                if (n < 0)
                    break;
#endif
                int k = int.Parse(ReadLine());

                if (n == 0)
                    Console.WriteLine(k);
                else
                {
                    long count = C(k, n);
                    count *= (k - 1);
                    Console.WriteLine(count);
                }

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


        static long C(long k, long n)
        {
            if (n == 1)
                return k;
            if (n == 2)
                return k*k - 1;
            if (n == 3)
                return k*k*k - 1 - (k - 1)*2;

            return C(k, n - 1)*(k - 1) + C(k, n - 2)*(k - 1);
        }
    }
}