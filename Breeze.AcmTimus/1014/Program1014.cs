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



namespace _1014
{
    class Program1014
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
                int mult = int.Parse(ReadLine());
#if ONLINE_JUDGE
#else
                if (mult < 0)
                    break;
#endif
                string c = Calc(mult);
                Console.WriteLine(c);

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


        static string Calc(int mult)
        {
            if (mult == 0)
                return "10";
            if (mult < 10)
                return mult.ToString();

            int[] pmults = new int[8];
            int[] ps = new int[] {2, 3, 5, 7};
            foreach (int p in ps)
                while (mult%p == 0)
                {
                    pmults[p]++;
                    mult /= p;
                }

            if (mult != 1)
                return "-1";

            int[] mults = new int[10];

            mults[9] = pmults[3]/2;
            pmults[3] %= 2;

            mults[8] = pmults[2]/3;
            pmults[2] %= 3;

            mults[7] = pmults[7];

            mults[6] = Math.Min(pmults[2], pmults[3]);
            pmults[2] -= mults[6];
            pmults[3] -= mults[6];

            mults[5] = pmults[5];

            mults[4] = pmults[2]/2;
            pmults[2] %= 2;

            mults[3] = pmults[3];

            mults[2] = pmults[2];

            return Calc(mults);
        }


        static string Calc(int[] mults)
        {
            int cnt = mults.Sum();
            char[] ar = new char[cnt];

            int ind = ar.Length - 1;

            for (int i = 9; i >= 2; i--)
                for (int k = 0; k < mults[i]; k++)
                {
                    ar[ind] = (char) (i + (int) '0');
                    ind--;
                }

            var sb = new StringBuilder(ar.Length);
            sb.Append(ar);
            return sb.ToString();
        }
    }
}