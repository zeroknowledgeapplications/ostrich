using System;
using Java.Util;
using Android.Bluetooth;
using System.Threading.Tasks;

namespace ProjectOstrich
{
	public class BluetoothController
	{
		public readonly string BluetoothName = "OstrichNet";
		public readonly UUID ID = UUID.FromString("3AD865EA-D036-11E4-B066-5EC48ED529EF");

		private BluetoothAdapter _adapter;
		private BluetoothServerSocket _listener;
		private System.Timers.Timer _scanner;
		private Task _acceptTask = Task.FromResult(null);

		public BluetoothController ()
		{
			_adapter = BluetoothAdapter.DefaultAdapter;
			_listener = _adapter.ListenUsingInsecureRfcommWithServiceRecord (BluetoothName, ID);
			_scanner = new System.Timers.Timer ();
			_scanner.Elapsed += (sender, e) => {
				if(_acceptTask.IsCompleted)
					_acceptTask = _listener.AcceptAsync(30).ContinueWith((t) => {

					});


			};
		}

		public void Start()
		{
			if (_scanner.Enabled)
				return;

			_adapter.StartDiscovery ();
		}

		public void Stop()
		{
			if (!_scanner.Enabled)
				return;

			_adapter.CancelDiscovery ();
		}
	}
}

