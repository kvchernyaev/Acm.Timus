#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion



namespace Problem1000
{
    class Program1000
    {
        static void Main(string[] args)
        {
            string[] tokens = Console.ReadLine().Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine(int.Parse(tokens[0]) + int.Parse(tokens[1]));
        }
    }
}