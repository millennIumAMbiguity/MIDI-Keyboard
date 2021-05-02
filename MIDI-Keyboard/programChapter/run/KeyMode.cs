using System;
using System.Text;
using WindowsInput;
using WindowsInput.Native;

namespace MIDIKeyboard.Run
{
	internal class KeyMode
	{
		private static readonly InputSimulator IS = new InputSimulator();
		private static          bool           addonButtonActive;
		private static readonly VirtualKeyCode addonButtonKey = VirtualKeyCode.F24;

		private static string AddonButton(bool keyStatus)
		{
			if (addonButtonActive == keyStatus)
				return "";

			addonButtonActive = keyStatus;

			if (addonButtonActive)
				IS.Keyboard.KeyDown(addonButtonKey);
			else
				IS.Keyboard.KeyUp(addonButtonKey);

			return " + " + addonButtonKey;
		}

		public static void Key(int id, string valueHex)
		{
			var sb  = new StringBuilder();
			var key = (VirtualKeyCode) Program.values[id][2];
			if (valueHex.Length > 4) {
				sb.Append("Key_Down '");
				IS.Keyboard.KeyDown(key);
			} else {
				sb.Append("Key_Up '");
				IS.Keyboard.KeyUp(key);
			}

			sb.Append(key.ToString());
			sb.Append("' on:");
			Console.WriteLine(sb);
		}

		public static void Addonkey(int id, string valueHex)
		{
			var sb  = new StringBuilder();
			var key = (VirtualKeyCode) Program.values[id][2];
			if (valueHex.Length > 4) {
				sb.Append("Key_Down '");
				sb.Append(key.ToString());
				sb.Append(AddonButton(true));
				IS.Keyboard.KeyDown(key);
			} else {
				sb.Append("Key_Up '");
				IS.Keyboard.KeyUp(key);
				sb.Append(key.ToString());
				sb.Append(AddonButton(false));
			}

			sb.Append("' on:");
			Console.WriteLine(sb);
		}

		public static void Macro(int id, string valueHex)
		{
			if (valueHex.Length > 4) {
				var sb = new StringBuilder("macro '");
				for (int k = 2; k < Program.values[id].Length; k++)
					if (Program.values[id][k] > 0) {
						var key = (VirtualKeyCode) Program.values[id][k];
						IS.Keyboard.KeyDown(key);
						sb.Append("+");
						sb.Append(key.ToString());
						sb.Append('\'');
					} else {
						var key = (VirtualKeyCode) (-Program.values[id][k]);
						IS.Keyboard.KeyUp(key);
						sb.Append(key.ToString());
						sb.Append('\'');
					}

				sb.Append(" on:");
				Console.WriteLine(sb);
			}
		}
	}
}