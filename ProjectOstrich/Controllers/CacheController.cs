using Android.Content;
using System.IO;
using System;

namespace ProjectOstrich
{
	public static class CacheController
	{
		public static Cache Cache;

		public static void Load()
		{
			Cache = ReadCache ();
		}

		public static void Save()
		{
			WriteCache (Cache);
		}

		public static Cache ReadCache()
		{
			var tmpdir = System.IO.Path.GetTempPath ();
			FileInfo i = new FileInfo (Path.Combine (tmpdir, "cache.dat"));
			if (!i.Exists)
				return new Cache ();

			using (var s = i.OpenRead ()) {
				using (var r = new StreamReader (s)) {
					return Cache.FromJson (r.ReadToEnd ());
				}
			}
		}

		public static Cache WriteCache(Cache cache)
		{
			var data = cache.ToJson ();
			var tmpdir = System.IO.Path.GetTempPath ();
			FileInfo i = new FileInfo (Path.Combine (tmpdir, "cache.dat"));

			using (var s = i.OpenWrite ()) {
				using (var w = new StreamWriter (s)) {
					w.Write (data);
				}
			}
		}


		//public void SaveMessage(Message message) {



		/*
			FileOutputStream outputStream = ;

			File outputFile = File.CreateTempFile("prefix", ".message", _context.CacheDir);

			try {
				outputStream = OpenFileOutput(filename, Context.MODE_PRIVATE);
				outputStream.write(string.getBytes());
				outputStream.close();
			} catch (Exception e) {
				e.printStackTrace();
			}


			using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt"))
			{
				foreach (string line in lines)
				{
					// If the line doesn't contain the word 'Second', write the line to the file. 
					if (!line.Contains("Second"))
					{
						file.WriteLine(line);
					}
				}
			}*/

		//}
	}
}

