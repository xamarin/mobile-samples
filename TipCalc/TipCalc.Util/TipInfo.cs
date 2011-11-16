using System;

namespace TipCalc.Util
{
	public class TipInfo
	{
		decimal total;
		public decimal Total {
			get {return total;}
			set {
				if (total != value) {
					total = value;
					OnTipValueChanged ();
				}
			}
		}

		decimal subTotal;
		public decimal Subtotal {
			get {return subTotal;}
			set {
				if (subTotal != value) {
					subTotal = value;
					OnTipValueChanged ();
				}
			}
		}

		decimal tipPercent;
		public decimal TipPercent {
			get {return tipPercent;}
			set {
				if (value != tipPercent) {
					tipPercent = value;
					OnTipValueChanged ();
				}
			}
		}

		private void OnTipValueChanged ()
		{
			var h = TipValueChanged;
			if (h != null)
				h (this, EventArgs.Empty);
		}

		public decimal Tax {
			get {return Total - Subtotal;}
		}

		public decimal TipValue {
			get {
				if (Total == 0m || Subtotal == 0m || TipPercent == 0m)
					return 0m;

				var percent = TipPercent;
				percent /= 100;
				decimal value = (Subtotal * (1+percent)) + (Total - Subtotal);
				decimal fract = value - Math.Truncate (value);
				int f = (int) (fract * 100);
				while ((f % 25) != 0)
					++f;
				fract = f;
				fract /= 100;
				value = Math.Truncate (value) + fract;

				return value - Total;
			}
		}

		public event EventHandler TipValueChanged;
	}
}

