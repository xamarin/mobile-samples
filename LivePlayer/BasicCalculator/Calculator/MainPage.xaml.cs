using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Calculator
{    
    public partial class MainPage : ContentPage
    {    
		int currentState = 1;
		string mathOperator;
		double firstNumber, secondNumber;

        public MainPage ()
        {
            InitializeComponent ();
            OnClear(this, null);
        }

		void OnSelectNumber(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			string pressed = button.Text;

			if (this.resultText.Text == "0" || currentState < 0) {
				this.resultText.Text = "";
				if (currentState < 0)
					currentState *= -1;
			}

			this.resultText.Text += pressed;

			double number;
			if (double.TryParse(this.resultText.Text, out number)) {
				this.resultText.Text = number.ToString("N0");
				if (currentState == 1) {
					firstNumber = number;
				} else {
					secondNumber = number;
				}
			}
		}

		void OnSelectOperator(object sender, EventArgs e)
		{
			currentState = -2;
			Button button = (Button)sender;
			string pressed = button.Text;
			mathOperator = pressed;
		}

		void OnClear(object sender, EventArgs e)
		{
			firstNumber = 0;
			secondNumber = 0;
			currentState = 1;
			this.resultText.Text = "0";
		}

		void OnCalculate(object sender, EventArgs e)
		{
			if (currentState == 2)
            {
                var result = SimpleCalculator.Calculate(firstNumber, secondNumber, mathOperator);

                this.resultText.Text = result.ToString();
                firstNumber = result;
                currentState = -1;
			}
		}
	}
}
