using System;
using Android.Content;
using Android.App;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace ProjectOstrich
{
	public class ExchangeManager
	{
		private BluetoothController _bluetooth;
		private Cache _cache;
		private Activity _activity;

		public ExchangeManager (Context context, Activity activity, Cache cache)
		{
			_cache = cache;
			_bluetooth = new BluetoothController (activity);
			_activity = activity;

			_bluetooth.IncomingSocket = HandleSocket;
			_bluetooth.OutgoingSocket = HandleSocket;

			_bluetooth.Start ();
		}

		private void HandleSocket(Stream input, Stream output)
		{
			Console.WriteLine ("Connection!");

			var outtask = Task.Factory.StartNew (() => {
				//using (var writer = new StreamWriter (output)) {
					Random random = new Random();
					var data = _cache.ToJson();
					var bytes = Encoding.UTF8.GetBytes(data);
				output.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
				if(bytes.Length != 0)
					output.Write(bytes, 0, bytes.Length);
				System.Threading.Thread.Sleep(1000);
				Console.WriteLine("out");
			});

			var intask = Task.Factory.StartNew (() => {

				var buffer = new byte[4];
				input.Read(buffer, 0, 4);
				var length = BitConverter.ToInt32(buffer, 0);
				if(length != 0)
				{
					var bytes = new byte[length];
					input.Read(bytes, 0, bytes.Length);
					var data = Encoding.UTF8.GetString(bytes);
					var remoteCache = Cache.FromJson (data);

					_activity.RunOnUiThread(() => {
						_cache.Add (remoteCache);
					});
				}
				System.Threading.Thread.Sleep(1000);

				Console.WriteLine("in");
			});

			Task.WaitAll (intask, outtask);
			System.Threading.Thread.Sleep (2000);
			Console.WriteLine ("done");
			input.Close ();
			output.Close ();
		}
	}
}

