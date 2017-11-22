using System;
using System.Collections.Generic;
using System.Linq;
using CoinTimeShared;
using Android.Views;

namespace CoinTimeGame.Input
{
	public class AmazonFireGameController : IGameController, IMenuController
	{
		// The Xamarin component for controllers doesn't seem to work
		// with CocosSharp, so we're going to handle the button pushes
		// ourselves. The analog stick works well, so we'll still rely on
		// the GameController class for analog stick movement
		static List<Keycode> toProcess = new List<Keycode>();
		static float dPadXToProcess;

		static List<Keycode> pressedKeys = new List<Keycode> ();
		float dPadX;
		float dPadXChange;

		float lastAnalogStickX;
		float analogStickX;
		static float analogStickToProcess;

		float IGameController.HorizontalRatio
		{ 
			get{ return analogStickX; }
		}

		bool IGameController.JumpPressed
		{ 
			get
			{ 
				return pressedKeys.Contains (Keycode.ButtonA);
			}
		}

		bool IGameController.BackPressed
		{
			get
			{
				return pressedKeys.Contains (Keycode.Back);
			}
		}

		static bool isConnected;
		public bool IsConnected
		{
			get
			{
				return isConnected;
			}
		}

		bool IMenuController.SelectPressed
		{
			get
			{
				return pressedKeys.Contains (Keycode.ButtonA);
			}
		}

		bool IMenuController.MovedLeft
		{
			get
			{
				
				return 
				// check the dpad:
					(dPadXChange < 0 && dPadX < 0) ||
					// and the analog stick:
					(lastAnalogStickX > -.5 && analogStickX <= -.5);
			}
		}

		bool IMenuController.MovedRight
		{
			get
			{
				return 
					// check the dpad:
					(dPadXChange > 0 && dPadX > 0) ||
					// and the analog stick:
					(lastAnalogStickX < .5 && analogStickX >= .5);
			}
		}

		public static void SetDPad(float value)
		{
			if (value != dPadXToProcess)
			{
				isConnected = true;
				dPadXToProcess = value;
			}
		}

		public static void SetLeftAnalogStick(float value)
		{
			if (value != analogStickToProcess)
			{
				isConnected = true;
				analogStickToProcess = value;
			}
		}

		public static void HandlePush(Keycode code)
		{
			// This handles all keys, a full implementation
			// may only be considered connected if certain keys
			// are pressed:
			isConnected = true;

			lock (toProcess)
			{
				toProcess.Add (code);
			}
		}

		public void UpdateInputValues()
		{
			pressedKeys.Clear ();

			lock (toProcess)
			{
				pressedKeys.AddRange (toProcess);
				toProcess.Clear ();
			}

			lastAnalogStickX = analogStickX;
			analogStickX = analogStickToProcess;

			dPadXChange = dPadXToProcess - dPadX;
			dPadX = dPadXToProcess;

		}

	}
}

