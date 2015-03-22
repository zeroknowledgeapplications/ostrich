using System;
using Java.Util;
using Android.Bluetooth;
using Android.Content;
using System.Threading.Tasks;
using System.IO;
using Android.App;
using System.Collections.Generic;

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
		private DateTime _lastDiscover = DateTime.Now;
		private Task _handleDiscoveries;
		private List<BluetoothDevice> _discoveries = new List<BluetoothDevice>();

		public Action<Stream, Stream> IncomingSocket { get; set; }
		public Action<Stream, Stream> OutgoingSocket { get; set; }

		Activity _activity;

		public BluetoothController (Activity activity)
		{
			_adapter = BluetoothAdapter.DefaultAdapter;
			_activity = activity;
			_listener = _adapter.ListenUsingRfcommWithServiceRecord (BluetoothName, ID);
			//_scanner = new System.Timers.Timer ();
			//_scanner.Elapsed += (sender, e) => HandleScan();
			//_scanner.Interval = TimeSpan.FromSeconds (5).TotalMilliseconds;

			var filter = new IntentFilter (BluetoothDevice.ActionFound);
			activity.RegisterReceiver (this, filter);
			activity.RegisterReceiver (this, new IntentFilter (BluetoothAdapter.ActionScanModeChanged));
			activity.RegisterReceiver (this, new IntentFilter (BluetoothAdapter.ActionDiscoveryFinished));
		}

		public void Start()
		{
			_adapter.StartDiscovery();

			var discover = new Intent (BluetoothAdapter.ActionRequestDiscoverable);
			discover.PutExtra (BluetoothAdapter.ExtraDiscoverableDuration, 3600);
			_activity.StartActivity (discover);
		
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

			if (DateTime.Now - _lastDiscover > TimeSpan.FromMinutes (3)) {
				//_adapter.StartDiscovery ();
				_lastDiscover = DateTime.Now;
			}
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


				BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra (BluetoothDevice.ExtraDevice);
				Console.WriteLine (device.Name);

				_discoveries.Add (device);
			}

			if (action == BluetoothAdapter.ActionScanModeChanged) {
				if (intent.Extras.GetInt (BluetoothAdapter.ExtraScanMode) == 21) { //None
					var discover = new Intent (BluetoothAdapter.ActionRequestDiscoverable);
					discover.PutExtra (BluetoothAdapter.ExtraDiscoverableDuration, 3600);
					_activity.StartActivity (discover);
				}
			}

			if (action == BluetoothAdapter.ActionDiscoveryFinished) {

				_handleDiscoveries = Task.Factory.StartNew (() => {
					foreach(var dev in _discoveries)
					{
						try {
							Console.WriteLine(dev.Name);
							if(dev.BondState != Bond.Bonded)
								continue;
							Console.WriteLine("bonded");
							var socket = dev.CreateRfcommSocketToServiceRecord(ID);
							socket.Connect();
							OutgoingSocket(socket.InputStream, socket.OutputStream);
							socket.Close();
							socket.Dispose();
						} catch (Exception e){
							Console.WriteLine(e);
						}
					}

					for(int i = 0; i < 20; i++)
					{
						try {
							Console.WriteLine("listen");
							var accept = _listener.Accept(1000);
							IncomingSocket(accept.InputStream, accept.OutputStream);
						} catch(Exception){}
					}

					Console.WriteLine("discovery");
					_adapter.StartDiscovery();
				});
			}
		}
	}
}

