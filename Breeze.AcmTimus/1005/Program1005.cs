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



namespace _1005
{
    class Program1005
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
                string countStr = ReadLine();
#if ONLINE_JUDGE
#else
                if (countStr == "-1")
                    break;
#endif

                int[] ws = ReadLine().Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                try
                {
                    int r = Solve(ws);
                    Console.WriteLine(r);
                }
                catch (Null)
                {
                    Console.WriteLine("0");
                }
                catch (One)
                {
                    Console.WriteLine("1");
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


        static void CheckMinDif(int dif)
        {
            if (dif == 0)
                throw new Null();
            if (dif == 1 || dif == -1)
                throw new One();
        }


        /// <exception cref="One"></exception>
        /// <exception cref="Null"></exception>
        static int Solve(int[] ws)
        {
            if (ws.Length == 0)
                return 0;
            if (ws.Length == 1)
                return ws[0];
            if (ws.Length == 2)
                return Math.Abs(ws[0] - ws[1]);

            // init
            int leftLack = -(2*ws[0] - ws.Sum()); // на сколько левая меньше правой (левая из одного камня сейчас) - >0
            CheckMinDif(leftLack);
            if (leftLack < 0)
                return Math.Abs(leftLack);

            // >= 2

            int toMove = leftLack/2;
            bool toMoveWithHalf = leftLack%2 == 1;

            int[] numbers = ws.Where((w, i) => i > 0).ToArray();
            int nearestSum = AssembleNearestSum(numbers, toMove, toMoveWithHalf);

            if (nearestSum == toMove)
                return toMoveWithHalf ? 1 : 0;
            if (nearestSum == toMove + 1 && toMoveWithHalf)
                return 1;
            return Math.Abs(2*(ws[0] + nearestSum) - ws.Sum());
        }


        /// <exception cref="One"></exception>
        /// <exception cref="Null"></exception>
        static int AssembleNearestSum(int[] numbers, int expected, bool withHalf)
        {
            // ближайшая сверху или снизу
            bool[] use = new bool[numbers.Length];

            int rv = AssembleNearestSumRecurse(numbers, expected, withHalf, use, -1, 0);
            return rv;
        }


        /// <exception cref="One"></exception>
        /// <exception cref="Null"></exception>
        static int AssembleNearestSumRecurse(int[] numbers, int expected, bool withHalf, bool[] use, int lastIndex, int currentSum)
        {
            int curOptSum = currentSum;

            for (int i = lastIndex + 1; i < numbers.Length; i++)
            {
                int newCurrentSum = currentSum + numbers[i];
                if (newCurrentSum == expected)
                    return newCurrentSum;
                if (newCurrentSum == expected + 1 && withHalf)
                    return newCurrentSum;
                if (newCurrentSum > expected)
                {
                    curOptSum = ChooseOpter(expected, withHalf, curOptSum, newCurrentSum);
                    continue;
                }

                bool[] newuse = use.ToArray();
                newuse[i] = true;
                int sum = AssembleNearestSumRecurse(numbers, expected, withHalf, newuse, i, newCurrentSum);
                curOptSum = ChooseOpter(expected, withHalf, curOptSum, sum);
            }

            return curOptSum;
        }


        static int ChooseOpter(int expected, bool withHalf, int oldOpt, int curopt)
        {
            return Math.Abs(expected + (withHalf ? 0.5m : 0m) - oldOpt) > Math.Abs(expected + (withHalf ? 0.5m : 0m) - curopt)
                       ? curopt
                       : oldOpt;
        }



        class Null : Exception {}



        class One : Exception {}
    }
}