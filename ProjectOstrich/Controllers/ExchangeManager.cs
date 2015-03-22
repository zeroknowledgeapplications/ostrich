using System;
using Android.Content;
using Android.App;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace ProjectOstrich
{
	public class ExchangeManager
	{
		private BluetoothController _bluetooth;
		private Cache _cache;

		public ExchangeManager (Context context, Activity activity, Cache cache)
		{
			_cache = cache;
			_bluetooth = new BluetoothController (activity);

			_bluetooth.IncomingSocket = HandleSocket;
			_bluetooth.OutgoingSocket = HandleSocket;

			_bluetooth.Start ();
		}

		private void HandleSocket(Stream input, Stream output)
		{
			Console.WriteLine ("Connection!");

			var outtask = Task.Factory.StartNew (() => {
				using (var writer = new StreamWriter (output)) {
					Random random = new Random();
					var data = _cache.Messages.OrderBy((i) => random.NextDouble()).First().ToJson();
					writer.WriteLine (data);
				}
				System.Threading.Thread.Sleep(100);
				output.Close ();
				Console.WriteLine("out");
			});

			var intask = Task.Factory.StartNew (() => {
				using (var reader = new StreamReader (input)) {
					System.Threading.Thread.Sleep(1000);
					var data = reader.ReadLine ();
					var remoteCache = Cache.FromJson (data);

					_cache.Add (remoteCache);
				}
				System.Threading.Thread.Sleep(100);
				input.Close ();
				Console.WriteLine("in");
			});

			Task.WaitAll (intask, outtask);
			Console.WriteLine ("done");
		}
	}
}

