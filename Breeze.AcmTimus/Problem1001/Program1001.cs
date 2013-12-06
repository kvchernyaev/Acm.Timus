#region usings
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion



namespace Problem1001
{
    class Program1001
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            string[] all = Console.In.ReadToEnd().Split(new char[] {' ', '\t', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in all.Reverse())
                Console.WriteLine(Math.Sqrt(long.Parse(s)).ToString("F4"));

//            Console.WriteLine(
//                string.Join(Environment.NewLine,
//                            all.Reverse().Select(x => Math.Sqrt(long.Parse(x)).ToString("F4")))
//                );
        }
    }
}