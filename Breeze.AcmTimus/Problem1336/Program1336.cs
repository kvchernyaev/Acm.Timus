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



namespace _1336
{
	class Program1336
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
				ulong n = ReadLineUlong();

				#if ONLINE_JUDGE
				#else
				if (n<=0)
					break;
				#endif

				Tuple<ulong,ulong> res = new Tuple<ulong, ulong>(n*n,n);//Solve(n);
				#if ONLINE_JUDGE
				Console.WriteLine(res.Item1);
				Console.WriteLine(res.Item2);
				#else
				Console.WriteLine(string.Format("for {0} ans: {1},{2}",n, res.Item1, res.Item2));
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
		
		
		static Tuple<ulong,ulong> Solve(ulong n)
		{
			
			List<KeyValuePair<ulong,int>> decomposition = Decompose(n);
			
			List<KeyValuePair<ulong,int>> mDec = new List<KeyValuePair<ulong, int>>();
			List<KeyValuePair<ulong,int>> kDec = new List<KeyValuePair<ulong, int>>();
			for(int i=0;i<decomposition.Count; i++)
			{
				mDec.Add(new KeyValuePair<ulong,int>(decomposition[i].Key, mi(decomposition[i].Value)));
				kDec.Add(new KeyValuePair<ulong,int>(decomposition[i].Key, ki(decomposition[i].Value)));
			}
			
			ulong m=Compose(mDec);
			ulong k=Compose(kDec);
			
			return new Tuple<ulong,ulong>(m,k);
		}
		
		
		static List<KeyValuePair<ulong,int>> Decompose(ulong n)
		{
			if(n==1)
				return new List<KeyValuePair<ulong,int>>();
			
			List<KeyValuePair<ulong,int>> rv = new List<KeyValuePair<ulong,int>>();
			
			int cnt2=CountMult(ref n, 2);
			if(cnt2>0)
				rv.Add(new KeyValuePair<ulong,int>(2,cnt2));
			var edge = n/2;
			for(ulong i=3;i<=edge;i+=2)
			{
				int cnt=CountMult(ref n, i);
				if(cnt>0)
					rv.Add(new KeyValuePair<ulong,int>(i,cnt));
			}			
			
			return rv;
		}	


		static int CountMult(ref ulong n, ulong mult)
		{
			int cnt=0;
			do{
				if(n%mult>0)
					return cnt;
				n/=mult;
				cnt++;
			}while(true);
		}
		
		static ulong Compose(List<KeyValuePair<ulong,int>> l)
		{
			ulong rv=1;
			foreach(KeyValuePair<ulong,int> kvp in l)
				for(int i=0;i<kvp.Value;i++)
					rv*=kvp.Key;
			return rv;			
		}
		
		static int mi(int ni)
		{
			return ni%2==0 ? ni/2 : (ni+3)/2;
		}
		static int ki(int ni)
		{
			return ni%2==0 ? 0 : 1;
		}

	}
}