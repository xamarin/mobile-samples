using System;

namespace AsyncImageAndroid
{
	public class TimeTake
	{
			public static double LongMethod1 ()
			{
				double d = 1;
				for (int x = 0; x < 10000000; x++) {
					d += Math.Sin (d * x / 100.0);
				}
				return d;
			}

			public static double LongMethod2 ()
			{
				double d = 1;
				for (int x = 0; x < 10000000; x++) {
					d += Math.Cos (d * x / 100.0);
				}
				return d;
			}

			public static long Fib (long n)
			{
				if (n < 2)
					return n;
				return Fib (n - 1) + Fib (n - 2);
			}

			public static long LongMethod3 ()
			{
				return Fib (33);
			}

			public static void Main (string[] args)
			{
				for (var i = 0; i < 10; i++) {
					var d = LongMethod1 ();
					var e = LongMethod2 ();
					var f = LongMethod3 ();
					Console.WriteLine (d + e);
					Console.WriteLine (f);
				}

			}
	}
}

