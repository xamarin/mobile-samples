using System;
using System.Windows.Input;

namespace GeolocationSample
{
	public class DelegatedCommand
		: ICommand
	{
		public DelegatedCommand (Action<object> execute, Func<object, bool> canExecute)
		{
			if (execute == null)
				throw new ArgumentNullException ("execute");
			if (canExecute == null)
				throw new ArgumentNullException ("canExecute");

			this.execute = execute;
			this.canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged;

		public void ChangeCanExecute()
		{
			var changed = CanExecuteChanged;
			if (changed != null)
				changed (this, EventArgs.Empty);
		}

		public bool CanExecute (object parameter)
		{
			return this.canExecute (parameter);
		}

		public void Execute (object parameter)
		{
			this.execute (parameter);
		}

		private readonly Action<object> execute;
		private readonly Func<object, bool> canExecute;
	}
}
