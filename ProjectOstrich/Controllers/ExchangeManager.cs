using System;
using Android.Content;
using Android.App;
using System.IO;
using System.Threading.Tasks;

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
					writer.Write (_cache.ToJson ());
				}
				output.Close ();
			});

			var intask = Task.Factory.StartNew (() => {
				using (var reader = new StreamReader (input)) {
					var remoteCache = Cache.FromJson (reader.ReadToEnd ());

					_cache.Add (remoteCache);
				}
				input.Close ();
			});

			Task.WaitAll (intask, outtask);
		}
	}
}

