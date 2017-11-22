using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace RpnCalculator
{
    public class Calculator
    {
        public Grid Make(ContentPage page)
        {

			var mainGrid = new Grid();
			page.Content = mainGrid;


            mainGrid.BackgroundColor = Color.Transparent;

            page.Padding = new Thickness(10, 10 + Device.OnPlatform(20, 0, 0), 10, 10);


			// Remove previous RowDefinition objects on re-execution:
			mainGrid.RowDefinitions.Clear();

			mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			for (int row = 0; row < 10; row++)
			{
				mainGrid.RowDefinitions.Add(new RowDefinition
				{
					Height = new GridLength(1, GridUnitType.Star)
				});
			}


			// Remove previous ColumnDefinition objects on re-execution:
			mainGrid.ColumnDefinitions.Clear();

			for (int col = 0; col < 4; col++)
			{
				mainGrid.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = new GridLength(1, GridUnitType.Star)
				});
			}



            // ENTERING NUMBERS

			Label entryLabel = new Label
			{
				Text = "0",
				BackgroundColor = new Color(0.85),
				HorizontalTextAlignment = TextAlignment.End
			};
			mainGrid.Children.Add(entryLabel, 0, 4, 1, 2);



            var numButtonInfos = new[]
            {
                new { text = "7", row = 7, col = 0, span = 1 },
                new { text = "8", row = 7, col = 1, span = 1 },
                new { text = "9", row = 7, col = 2, span = 1 },
                new { text = "4", row = 8, col = 0, span = 1 },
                new { text = "5", row = 8, col = 1, span = 1 },
                new { text = "6", row = 8, col = 2, span = 1 },
                new { text = "1", row = 9, col = 0, span = 1 },
                new { text = "2", row = 9, col = 1, span = 1 },
                new { text = "3", row = 9, col = 2, span = 1 },
                new { text = "0", row = 10, col = 0, span = 2 },
                new { text = ".", row = 10, col = 2, span = 1 }
            };

			foreach (var numButtonInfo in numButtonInfos)
			{
				var button = new Button
				{
					Text = numButtonInfo.text,
					Margin = new Thickness(5)
				};
				button.Clicked += (sender, args) =>
				{
					string text = entryLabel.Text == "0" ? "" : entryLabel.Text;
					text += (sender as Button).Text;

					if (text == ".")
					{
						text = "0.";
					}

					double number;
					if (Double.TryParse(text, out number))
					{
						entryLabel.Text = text;
					}
					else
					{
						Beeper.Error();
					}
				};
				mainGrid.Children.Add(button, numButtonInfo.col,
											  numButtonInfo.col + numButtonInfo.span,
											  numButtonInfo.row,
											  numButtonInfo.row + 1);
			}

			var resourceDictionary = new ResourceDictionary();
			page.Resources = resourceDictionary;


			var buttonStyle = new Style(typeof(Button));
			buttonStyle.Setters.Add(new Setter
			{
				Property = Button.BorderWidthProperty,
				Value = 1
			});

            resourceDictionary.Add(buttonStyle);



            // MANAGING THE STACK

            stack = new Stack<double>();


			var stackGrid = new Grid();
			stackGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			stackGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			stackGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			stackGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			mainGrid.Children.Add(stackGrid, 0, 4, 0, 1);


			var xLabel = new Label { Text = "x = " };
			stackGrid.Children.Add(xLabel, 0, 1);

			xStackNumber = new Label { HorizontalTextAlignment = TextAlignment.End };
			stackGrid.Children.Add(xStackNumber, 1, 1);

			var yLabel = new Label { Text = "y = " };
			stackGrid.Children.Add(yLabel, 0, 0);

			yStackNumber = new Label { HorizontalTextAlignment = TextAlignment.End };
			stackGrid.Children.Add(yStackNumber, 1, 0);



			var enterButton = new Button
			{
				Text = "Enter"
			};
			enterButton.Clicked += (args, sender) =>
			{
				stack.Push(Double.Parse(entryLabel.Text));
				entryLabel.Text = "0";
				UpdateStackDisplay();
			};
			mainGrid.Children.Add(enterButton, 0, 4, 11, 12);


			// CLEARING AND BACKSPACE

			var clearAllButton = new Button { Text = "C" };
			clearAllButton.Clicked += (args, sender) =>
			{
				stack.Clear();
				entryLabel.Text = "0";
				UpdateStackDisplay();
			};
			mainGrid.Children.Add(clearAllButton, 0, 6);


			var clearEntryButton = new Button { Text = "CE" };
			clearEntryButton.Clicked += (args, sender) =>
			{
				entryLabel.Text = "0";
			};
			mainGrid.Children.Add(clearEntryButton, 1, 6);


			var backspaceButton = new Button { Text = "\x21E6" };
			backspaceButton.Clicked += (args, sender) =>
			{
				entryLabel.Text = entryLabel.Text.Substring(0, entryLabel.Text.Length - 1);
				if (entryLabel.Text.Length == 0)
				{
					entryLabel.Text = "0";
				}
			};
			mainGrid.Children.Add(backspaceButton, 2, 4, 6, 7);

			// BINARY OPERATIONS


			var swapButton = new Button { Text = "x\x21D4y" };
			swapButton.Clicked += (args, sender) =>
			{
				if (stack.Count < 2)
				{
					Beeper.Error();
				}
				else
				{
					double x = stack.Pop();
					double y = stack.Pop();
					stack.Push(x);
					stack.Push(y);
					UpdateStackDisplay();
				}
			};
			mainGrid.Children.Add(swapButton, 1, 5);


            BinaryOpInfo[] binaryOpInfos =
            {
                new BinaryOpInfo { Text = "\x00F7", Row = 7, Col = 3, Operation = (x, y) => x / y },
                new BinaryOpInfo { Text = "\x00D7", Row = 8, Col = 3, Operation = (x, y) => x * y },
                new BinaryOpInfo { Text = "\x2013", Row = 9, Col = 3, Operation = (x, y) => x - y },
                new BinaryOpInfo { Text = "+", Row = 10, Col = 3, Operation = (x, y) => x + y },
                new BinaryOpInfo { Text = "y\x1D61", Row = 2, Col = 0, Operation = Math.Pow }
            };


            Dictionary<Button, Func<double, double, double>> binaryOpDictionary = 
                new Dictionary<Button, Func<double, double, double>>();

            foreach (BinaryOpInfo binaryOpInfo in binaryOpInfos)
            {
                var binaryOpButton = new Button // new BinaryOperationButton
                {
                    Text = binaryOpInfo.Text,
                //    Operation = binaryOpInfo.Operation,
                    Style = buttonStyle
                };

                binaryOpDictionary.Add(binaryOpButton, binaryOpInfo.Operation);

                binaryOpButton.Clicked += (sender, args) =>
                {
                    if (entryLabel.Text != "0")
                    {
                        stack.Push(Double.Parse(entryLabel.Text));
                        entryLabel.Text = "0";
                    }
                    if (stack.Count < 2)
                    {
                        Beeper.Error();
                    }
                    else
                    {
                        // BinaryOperationButton button = sender as BinaryOperationButton;
                        Button button = sender as Button;
                        double x = stack.Pop();
                        double y = stack.Pop();
                        stack.Push(binaryOpDictionary[button](y, x));
                        UpdateStackDisplay();
                    }
                };
                mainGrid.Children.Add(binaryOpButton, binaryOpInfo.Col, binaryOpInfo.Row);
            }



            // UNARY OPERATIONS


            UnaryOpInfo[] unaryOpInfos =
			{
			    new UnaryOpInfo { Text = "log", Row = 2, Col = 1, Operation = Math.Log10 },
			    new UnaryOpInfo { Text = "ln", Row = 2, Col = 2, Operation = Math.Log },
			    new UnaryOpInfo { Text = "e\x1D61", Row = 2, Col = 3, Operation = Math.Exp },
			    new UnaryOpInfo { Text = "\x221Ax", Row = 3, Col = 0, Operation = Math.Sqrt },
			    new UnaryOpInfo { Text = "sin", Row = 3, Col = 1, Operation = Math.Sin },
			    new UnaryOpInfo { Text = "cos", Row = 3, Col = 2, Operation = Math.Cos },
			    new UnaryOpInfo { Text = "tan", Row = 3, Col = 3, Operation = Math.Tan },
			    new UnaryOpInfo { Text = "1/x", Row = 4, Col = 0, Operation = x => 1 / x },
			    new UnaryOpInfo { Text = "asin", Row = 4, Col = 1, Operation = Math.Asin },
			    new UnaryOpInfo { Text = "acos", Row = 4, Col = 2, Operation = Math.Acos },
			    new UnaryOpInfo { Text = "atan", Row = 4, Col = 3, Operation = Math.Atan },
			    new UnaryOpInfo { Text = "+/\x2013", Row = 5, Col = 0, Operation = x => -x },
			    new UnaryOpInfo { Text = "rad", Row = 5, Col = 2, Operation = d => Math.PI * d / 180 },
			    new UnaryOpInfo { Text = "deg", Row = 5, Col = 3, Operation = r => 180 * r / Math.PI }
			};

            Dictionary<Button, Func<double, double>> unaryOpDictionary =
                new Dictionary<Button, Func<double, double>>();


            foreach (UnaryOpInfo unaryOpInfo in unaryOpInfos)
            {
                var unaryOpButton = new Button // new UnaryOperationButton
                {
                    Text = unaryOpInfo.Text,
                //    Operation = unaryOpInfo.Operation,
                    Style = buttonStyle
                };

                unaryOpDictionary.Add(unaryOpButton, unaryOpInfo.Operation);

                unaryOpButton.Clicked += (sender, args) =>
                {
                    if (entryLabel.Text != "0")
                    {
                        stack.Push(Double.Parse(entryLabel.Text));
                        entryLabel.Text = "0";
                    }
                    if (stack.Count < 1)
                    {
                        Beeper.Error();
                    }
                    else
                    {
                        Button button = sender as Button;
                        double x = stack.Pop();
                        stack.Push(unaryOpDictionary[button](x));
                        UpdateStackDisplay();
                    }
                };
                mainGrid.Children.Add(unaryOpButton, unaryOpInfo.Col, unaryOpInfo.Row);
            }




            return mainGrid;
        }

        Stack<double> stack;
        Label xStackNumber, yStackNumber;

		void UpdateStackDisplay()
		{
			xStackNumber.Text = stack.Count > 0 ? stack.Peek().ToString() : "";

			if (stack.Count > 1)
			{
				double hold = stack.Pop();
				yStackNumber.Text = stack.Peek().ToString();
				stack.Push(hold);
			}
			else
			{
				yStackNumber.Text = "";
			}
		}

    }

	static class Beeper
	{
		public static void Error()
		{
#if __ANDROID__
			Android.Media.ToneGenerator toneGenerator = new Android.Media.ToneGenerator(Android.Media.Stream.Music, 100);
			toneGenerator.StartTone(Android.Media.Tone.CdmaPip, 150);
#elif __IOS__
			AudioToolbox.SystemSound systemSound = systemSound = new AudioToolbox.SystemSound(1000);
			systemSound.PlaySystemSound();
#endif
		}
	}


	public struct BinaryOpInfo
	{
		public string Text { set; get; }
		public int Row { set; get; }
		public int Col { set; get; }
		public Func<double, double, double> Operation { set; get; }
	}

	public struct UnaryOpInfo
	{
		public string Text { set; get; }
		public int Row { set; get; }
		public int Col { set; get; }
		public Func<double, double> Operation { set; get; }
	}

}
