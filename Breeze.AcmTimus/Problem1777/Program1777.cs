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



namespace _1777
{
	class Program1777
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
		#endregion


		static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			#if ONLINE_JUDGE
			#else
			do
			{
				#endif
				ulong[] ns = ReadUlongArray();

				#if ONLINE_JUDGE
				#else
				if (ns.Length==0 ||ns[0]<0)
					break;
				#endif

				var res = Solve(ns);
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
		
		
		static ulong Solve(ulong[] ns)
		{
			//List<long> l = new List<long>(ns);
			SortedList<ulong,ulong> l = new SortedList<ulong,ulong>(1024);
			
			foreach(ulong n in ns)
				l.Add(n,1);
			
			ulong cnt=0;
			
			do{
				cnt++;
				var dif = CalcClosestDif(l);
				if(dif==0)
					return cnt;
				try{
				l.Add(dif,1);
				}catch(ArgumentException)
				{return cnt+1;}
			}while(true);
		}
		
		
		static ulong CalcClosestDif(SortedList<ulong,ulong> l)
		{
			ulong prev=0;
			ulong bestdif=ulong.MaxValue;
			
			foreach(ulong cur in l.Keys)
			{
				if(prev<=0)
				{
					prev=cur;
					continue;
				}
				
				var dif =cur-prev;//Dif(prev, cur);
				if(dif==0)
					return 0;
				if(dif<bestdif)
					bestdif=dif;
				
				prev=cur;
			}
			
			return bestdif;
		}

		
		static ulong Dif(ulong a, ulong b)
		{
			if(a>b)
				return a-b;
			return b-a;
		}

	}
}