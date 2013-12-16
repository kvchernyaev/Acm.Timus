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



namespace _12_1133_201
{
    class Program_12_1133_201
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
                long[] ar = ReadLongArray();

#if ONLINE_JUDGE
#else
                if (ar.Length <= 1)
                    break;
#endif

                //Console.WriteLine("FUCK " + CalcByPrev(100, new Dictionary<long, long>(){{0,1},{1,1}}));

                decimal res = Solve(ar);
#if ONLINE_JUDGE
                Console.WriteLine(res);
#else
                Console.WriteLine(res);
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


        static decimal Solve(long[] ar)
        {
            long ind1 = ar[0];
            long ind2 = ar[2];
            long indFind = ar[4];

            if (indFind == ind1)
                return ar[1];
            if (indFind == ind2)
                return ar[3];

            Dictionary<long, decimal> f = new Dictionary<long, decimal>();
            f.Add(ind1, ar[1]);
            f.Add(ind2, ar[3]);

            if (ind1 > ind2)
            {
                long tmp = ind1;
                ind1 = ind2;
                ind2 = tmp;
            }

            // ind1 < ind2

            long k = ind2 - ind1; // >=1
            _F = new long[k];

            if (k > 1)
            {
                long indMed = ind1 + 1;
                decimal val = (f[ind2] - F(k - 2) * f[ind1]) / F(k - 1);
                f.Add(indMed, val);
            }

            // calculated indexes: ind1, ind1+1
            if (indFind == ind1 + 1)
                return f[ind1 + 1];

            decimal rv;
            if (indFind > ind1)
                rv = CalcByPrev(indFind, f);
            else
                rv = CalcByNext(indFind, f);
            return rv;
        }


        static decimal CalcByPrev(long ind, Dictionary<long, decimal> f)
        {
            decimal rv;
            if (f.TryGetValue(ind, out rv))
                return rv;
            rv = CalcByPrev(ind - 1, f) + CalcByPrev(ind - 2, f);
            f.Add(ind, rv);
            return rv;
        }


        static decimal CalcByNext(long ind, Dictionary<long, decimal> f)
        {
            decimal rv;
            if (f.TryGetValue(ind, out rv))
                return rv;
            rv = CalcByNext(ind + 2, f) - CalcByNext(ind + 1, f);
            f.Add(ind, rv);
            return rv;
        }


        static long[] _F;


        static long F(long i)
        {
            if (i < 2)
                return 1;
            if (_F[i] == 0)
                _F[i] = F(i - 1) + F(i - 2);
            return _F[i];
        }
    }
}