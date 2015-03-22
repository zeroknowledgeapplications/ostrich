using Android.Content;
using Java.IO;

namespace ProjectOstrich
{
	public class Cache
	{
	
		Context _context;

		public Cache (Context context)
		{
			_context = context;
		}



		public void SaveMessage(Message message) {

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

		}
	}
}

