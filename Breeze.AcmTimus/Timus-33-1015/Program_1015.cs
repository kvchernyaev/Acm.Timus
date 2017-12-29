using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;


namespace Timus_33_1015
{
    internal class Program1015
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


        static byte[] ReadByteArray()
        {
            string s = ReadLine();
            return s.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries).Select(byte.Parse).ToArray();
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

            int n = ReadIntLine();
            Cub[] cubs = new Cub[n];
            for (int i = 0; i < n; i++)
            {
                byte[] scheme = ReadByteArray(); // 0..5
                cubs[i] = new Cub(i, scheme);
            }

            PrintCubs(cubs);
            Log("--------------------------------");
            foreach (Cub cub in cubs)
            {
                cub.Normalize();
                cub.SetSchema();
            }

            PrintCubs(cubs);
            Log("--------------------------------");

            IEnumerable<IGrouping<int, int>> grouped = cubs.GroupBy(x => x.Schema, x => x.I);
            List<List<int>> rv = grouped.Select(g => g.OrderBy(i => i).ToList()).OrderBy(a => a[0])
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine(rv.Count.ToString());
            foreach (List<int> a in rv)
            {
                sb.AppendLine($"{string.Join(" ", a.Select(y => (y + 1).ToString()))}");
            }

            Console.Write(sb.ToString());
        }

        private static void PrintCubs(IReadOnlyList<Cub> cubs)
        {
#if ONLINE_JUDGE
#else
            for (int i = 0; i < cubs.Count; i++)
            {
                var cub = cubs[i];
                Log($"{cub.I}: {cub.Front} {cub.Up} {cub.Right} {cub.Down} {cub.Left} {cub.Back}");
            }
#endif
        }


        class Cub
        {
            public readonly int I;

            /// <summary>
            /// количество очков на левой, правой, верхней, передней, нижней и задней гранях
            /// </summary>
            //public readonly byte[] SchemaInput;
            public int Schema;

            public byte Left;
            public byte Right;
            public byte Up;
            public byte Front;
            public byte Down;
            public byte Back;

            #region rotate

            public void RotateLeft()
            {
                byte _left = Left;
                Left = Front;
                Front = Right;
                Right = Back;
                Back = _left;
            }

            public void RotateRight()
            {
                byte _right = Right;
                Right = Front;
                Front = Left;
                Left = Back;
                Back = _right;
            }

            public void RotateUp()
            {
                byte _up = Up;
                Up = Front;
                Front = Down;
                Down = Back;
                Back = _up;
            }

            public void RotateDown()
            {
                byte _down = Down;
                Down = Front;
                Front = Up;
                Up = Back;
                Back = _down;
            }

            public void RotateClock()
            {
                byte _up = Up;
                Up = Left;
                Left = Down;
                Down = Right;
                Right = _up;
            }

            public void RotateAnticlock()
            {
                byte _up = Up;
                Up = Right;
                Right = Down;
                Down = Left;
                Left = _up;
            }

            #endregion

            public void Normalize()
            {
                Set1ToFront();
                Set2Or3ToUp();
            }

            public void SetSchema()
            {
                Schema = Front + 6 * Up + 36 * Right + 216 * Down + 1296 * Left + 7776 * Back;
            }

            private void Set2Or3ToUp()
            {
                //if(Front==2){} // can not be - Front~1 now
                if (Up == 2)
                {
                }
                else if (Left == 2)
                    RotateClock();
                else if (Right == 2)
                    RotateAnticlock();
                else if (Down == 2)
                {
                    RotateClock();
                    RotateClock();
                }
                else if (Back == 2)
                {
                    // set 3 to up
                    // can not be Front(~1 now), Back(~2 now)
                    if (Up == 3)
                    {
                    }
                    else if (Left == 3)
                        RotateClock();
                    else if (Right == 3)
                        RotateAnticlock();
                    else if (Down == 3)
                    {
                        RotateClock();
                        RotateClock();
                    }
                }
            }

            private void Set1ToFront()
            {
                if (Front == 1)
                {
                }
                else if (Left == 1)
                    RotateRight();
                else if (Right == 1)
                    RotateLeft();
                else if (Up == 1)
                    RotateDown();
                else if (Down == 1)
                    RotateUp();
                else if (Back == 1)
                {
                    RotateRight();
                    RotateRight();
                }
            }


            public Cub(int i, byte[] schemaInput)
            {
                I = i;
                //SchemaInput = schemaInput;

                Left = schemaInput[0];
                Right = schemaInput[1];
                Up = schemaInput[2];
                Front = schemaInput[3];
                Down = schemaInput[4];
                Back = schemaInput[5];
            }
        }
    }
}