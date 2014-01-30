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



namespace _24_1207_201
{
    /// <summary>
    /// http://acm.timus.ru/problem.aspx?num=1207
    /// </summary>
    class Program_24_1207_201
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


        static string InputFilePath { get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Input.txt"); } }
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
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(decimal.Parse).ToArray();
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


        #endregion


        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
#if ONLINE_JUDGE
#else
            do
            {
#endif
                int? countN = ReadIntNLine();
#if ONLINE_JUDGE
#else
                if (countN == null)
                    break;
#endif
                int count = countN.Value;
                P[] points = new P[count];

                for (int i = 0; i < count; i++)
                {
                    int[] ar = ReadIntArray();
                    points[i] = new P(ar[0], ar[1], i);
                }

                int i1, i2;

                Solve(points, out i1, out i2);

                Console.WriteLine((i1 + 1) + " " + (i2 + 1));

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


        static void Solve(P[] points, out int i1, out int i2)
        {
            i1 = 0;
            // points[0] is null point, points[1] is axis point

            // make first is null
            points = points.Select(p => new P(p.X - points[0].X, p.Y - points[0].Y, p.Index)).ToArray();

            for (int i = 2; i < points.Length; i++)
                points[i].SetAxis(points[1]);

            // exclude them
            points = points.Reverse().Take(points.Length - 2).ToArray();

            P[] sorted = points.OrderBy(p => p.AngleToTurnAxis).ToArray();
            int lCount = points.Count(p => p.Side);
            int rCount = points.Count(p => !p.Side);
            if (lCount == rCount)
            {
                i2 = 1;
                return;
            }

            P prev = new P(0, 0, -1) {Side = true};
            for (int i = 0; i < sorted.Length; i++)
            {
                P current = sorted[i];
                if (prev.Side)
                    rCount++; // axis bacame on the right
                else
                    lCount++;
                prev = current;

                if (current.Side) // left
                    lCount--;
                else
                    rCount--;

                if (lCount == rCount)
                {
                    i2 = current.Index;
                    return;
                }
            }
            i2 = -1;
        }



        [DebuggerDisplay("{X} {Y} i={Index}")]
        public class P
        {
            public P(int x, int y, int i)
            {
                X = x;
                Y = y;
                Index = i;
                Phi = Math.Atan2(Y, X) + Math.PI;
            }


            /// <summary>
            /// [0, pi)
            /// </summary>
            public double AngleToTurnAxis { get; private set; }


            /// <summary>
            /// left = true, right = false
            /// </summary>
            public bool Side { get; set; }


            public void SetAxis(P pAxis)
            {
                bool side;
                AngleToTurnAxis = this.AngleToTurnLine(pAxis, out side);
                Side = side;
            }


            /// <summary>
            /// [0, 2pi) - angle by clockwise
            /// </summary>
            double RelPhi(P pAxis)
            {
                double rv = this.Phi - pAxis.Phi;
                if (rv < 0)
                    rv += Math.PI*2;
                return rv;
            }


            /// <summary>
            /// [0, pi)
            /// </summary>
            /// <param name="side">left = true, right = false</param>
            /// <returns></returns>
            double AngleToTurnLine(P pAxis, out bool side)
            {
                double relAngle = this.RelPhi(pAxis);
                if (relAngle < Math.PI)
                    side = true;
                else
                {
                    side = false;
                    relAngle -= Math.PI;
                }
                return relAngle;
            }


            public int X;
            public int Y;

            public int Index;

            public double Phi;
        }
    }
}