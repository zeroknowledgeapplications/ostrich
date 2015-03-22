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
		private bool _connecting = false;

		public Action<Stream, Stream> IncomingSocket { get; set; }
		public Action<Stream, Stream> OutgoingSocket { get; set; }

		Activity _activity;

		public BluetoothController (Activity activity)
		{
			_adapter = BluetoothAdapter.DefaultAdapter;
			_activity = activity;
			_listener = _adapter.ListenUsingInsecureRfcommWithServiceRecord (BluetoothName, ID);
			_scanner = new System.Timers.Timer ();
			_scanner.Elapsed += (sender, e) => HandleScan();
			_scanner.Interval = TimeSpan.FromSeconds (5).TotalMilliseconds;

			var filter = new IntentFilter (BluetoothDevice.ActionFound);
			activity.RegisterReceiver (this, filter);
			activity.RegisterReceiver (this, new IntentFilter (BluetoothAdapter.ActionScanModeChanged));
			activity.RegisterReceiver (this, new IntentFilter (BluetoothAdapter.ActionDiscoveryFinished));
		}

		public void Start()
		{
			if (_scanner.Enabled)
				return;

			Task.Factory.StartNew (() => {
				while(_scanner.Enabled)
				{
					try {
					var socket = _listener.Accept();
						IncomingSocket(socket.InputStream, socket.OutputStream);
						socket.Close();
						socket.Dispose();
					} catch (Exception){}

				}
			});

			var discover = new Intent (BluetoothAdapter.ActionRequestDiscoverable);
			discover.PutExtra (BluetoothAdapter.ExtraDiscoverableDuration, 3600);
			_activity.StartActivity (discover);

			_scanner.Start ();
			_adapter.CancelDiscovery ();
			_adapter.StartDiscovery ();
			HandleScan ();
		}

		private void HandleScan() {
			/*if(_acceptTask.IsCompleted)
				_acceptTask = _listener.AcceptAsync(5000).ContinueWith((t) => {
					if(t.IsFaulted)
						return;

					IncomingSocket(t.Result.InputStream, t.Result.OutputStream);
					t.Result.Close();
				});
				*/

			Console.WriteLine (_adapter.ScanMode);

			//if(!_adapter.IsDiscovering)
			//	_adapter.StartDiscovery();


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
				Console.WriteLine ("Found");
				_connecting = true;
				_adapter.CancelDiscovery ();
				BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra (BluetoothDevice.ExtraDevice);
				Console.WriteLine (device.Name);
				var connection = device.CreateInsecureRfcommSocketToServiceRecord (ID);
				connection.ConnectAsync ().ContinueWith ((t) => {
					Console.WriteLine(t.IsFaulted);
					if (!t.IsFaulted)
					{
						OutgoingSocket(connection.InputStream, connection.OutputStream);
						connection.Close();
					}
					_connecting = false;
					_adapter.StartDiscovery();
				});
			}

			if (action == BluetoothAdapter.ActionScanModeChanged) {
				if (intent.Extras.GetInt (BluetoothAdapter.ExtraScanMode) == 21) { //None
					var discover = new Intent (BluetoothAdapter.ActionRequestDiscoverable);
					discover.PutExtra (BluetoothAdapter.ExtraDiscoverableDuration, 3600);
					_activity.StartActivity (discover);
				}
			}

			if (action == BluetoothAdapter.ActionDiscoveryFinished) {
				if (!_connecting)
					_adapter.StartDiscovery ();
			}
		}
	}
}

