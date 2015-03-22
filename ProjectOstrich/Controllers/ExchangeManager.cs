using System;
using Android.Content;
using Android.App;
using System.IO;

namespace ProjectOstrich
{
	public class ExchangeManager
	{
		private BluetoothController _bluetooth;
		private Cache _cache;

		public ExchangeManager (Context context, Activity activity)
		{
			_cache = new Cache (context);
			_bluetooth = new BluetoothController (activity);

			_bluetooth.IncomingSocket = HandleIncomingSocket;
			_bluetooth.OutgoingSocket = HandleOutgoingSocket;
		}

		private void HandleIncomingSocket(Stream input, Stream output)
		{
			using (var reader = new StreamReader (input)) {
				var remoteCache = Cache.FromJson (reader.ReadToEnd ());

				_cache.Add (remoteCache);
			}

			input.Close ();
			output.Close ();
		}

		private void HandleOutgoingSocket(Stream input, Stream output)
		{
			using (var writer = new StreamWriter (output)) {
				writer.Write (_cache.ToJson ());
			}

			input.Close ();
			output.Close ();
		}
	}
}

