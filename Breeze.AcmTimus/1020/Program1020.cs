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



namespace _1020
{
    class Program1020
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
        #endregion


        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
#if ONLINE_JUDGE
#else
            do
            {
#endif
                decimal[] a = ReadDecimalArray();
#if ONLINE_JUDGE
#else
                if (a.Length < 2)
                    break;
#endif
                int n = (int) a[0];
                double r = (double) a[1];

                P[] ps = new P[n];
                for (int i = 0; i < n; i++)
                {
                    a = ReadDecimalArray();
                    ps[i] = new P {X = (double) a[0], Y = (double) a[1]};
                }

                double res = Solve(ps, r);
                Console.WriteLine(Math.Round(res, 2));
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


        static double Solve(P[] ps, double r)
        {
            var psEx = new P[ps.Length + 2];
            psEx[0] = ps[ps.Length - 1];
            psEx[ps.Length + 1] = ps[0];
            for (int i = 0; i < ps.Length; i++)
                psEx[i + 1] = ps[i];

            double strightsSum = 0;
            for (int i = 1; i <= ps.Length; i++)
                strightsSum += Stright(psEx[i - 1], psEx[i]);

            double rSum = 2*r*Math.PI;
//            for (int i = 1; i <= ps.Length; i++)
//                rSum += R(psEx[i - 1], psEx[i], psEx[i + 1], r);

            return strightsSum + rSum;
        }


        static double R(P p, P c, P n, double r)
        {
            return r*(Math.PI - Math.Acos(ScalarMult(p, c, n)/Stright(p, c)/Stright(c, n)));
        }


        static double ScalarMult(P p, P c, P n)
        {
            return (p.X - c.X)*(n.X - c.X) + (p.Y - c.Y)*(n.Y - c.Y);
        }


        static double Stright(P a, P b)
        {
            return Math.Sqrt((a.X - b.X)*(a.X - b.X) + (a.Y - b.Y)*(a.Y - b.Y));
        }
    }



    [DebuggerDisplay("{X} {Y}")]
    class P
    {
        public double X;
        public double Y;
    }
}