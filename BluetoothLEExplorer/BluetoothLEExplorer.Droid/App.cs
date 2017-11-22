using System;

namespace BluetoothLEExplorer.Droid
{
	public class App
	{
		public static App Current {
			get { return _current; }
		} private static App _current;

		public State State {
			get { return this._state; }
		}
		protected State _state;

		protected App ()
		{
			this._state = new State ();
		}

		static App ()
		{
			_current = new App ();
		}


	}
}

