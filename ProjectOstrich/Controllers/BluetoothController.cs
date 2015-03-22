using System;
using Java.Util;
using Android.Bluetooth;
using Android.Content;
using System.Threading.Tasks;
using System.IO;
using Android.App;

namespace ProjectOstrich
{
	public class BluetoothController : BroadcastReceiver
	{
		public readonly string BluetoothName = "OstrichNet";
		public readonly UUID ID = UUID.FromString("3AD865EA-D036-11E4-B066-5EC48ED529EF");

		private BluetoothAdapter _adapter;
		private BluetoothServerSocket _listener;
		private System.Timers.Timer _scanner;
		private Task _acceptTask = Task.FromResult<object>(null);

		public Action<Stream, Stream> IncomingSocket { get; set; }
		public Action<Stream, Stream> OutgoingSocket { get; set; }

		public BluetoothController (Activity activity)
		{
			_adapter = BluetoothAdapter.DefaultAdapter;
			_listener = _adapter.ListenUsingInsecureRfcommWithServiceRecord (BluetoothName, ID);
			_scanner = new System.Timers.Timer ();
			_scanner.Elapsed += (sender, e) => {
				if(_acceptTask.IsCompleted)
					_acceptTask = _listener.AcceptAsync(30).ContinueWith((t) => {
						if(t.IsFaulted)
							return;

						IncomingSocket(t.Result.InputStream, t.Result.OutputStream);
					});

				if(!_adapter.IsDiscovering)
					_adapter.StartDiscovery();
			};
			_scanner.Interval = TimeSpan.FromSeconds (35).TotalMilliseconds;

			var filter = new IntentFilter (BluetoothDevice.ActionFound);
			activity.RegisterReceiver (this, filter);
		}

		public void Start()
		{
			if (_scanner.Enabled)
				return;

			_scanner.Start ();
			_adapter.StartDiscovery ();
		}

		public void Stop()
		{
			if (!_scanner.Enabled)
				return;

			_scanner.Stop ();
			_adapter.CancelDiscovery ();
		}

		public override void OnReceive (Context context, Intent intent)
		{
			string action = intent.Action;

			if (action == BluetoothDevice.ActionFound) {

				BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra (BluetoothDevice.ExtraDevice);

				var connection = device.CreateInsecureRfcommSocketToServiceRecord (ID);
				connection.ConnectAsync ().ContinueWith ((t) => {
					if (t.IsFaulted)
						return;

					OutgoingSocket(connection.InputStream, connection.OutputStream);
				});
			}
		}
	}
}

