using System;

namespace MIDIKeyboard.Miscellaneous
{
	internal static class Miscellaneous
	{
		public static int GetId(char c)
		{
			int cId = c;
			if (c > 96 && c < 123)
				cId -= 32;

			return cId;
		}

		public static int GetId(ConsoleKeyInfo c)
		{
			if (c.KeyChar != 0)
				return GetId(c.KeyChar);
			else
				return GetId(c.Key);
		}

		public static int GetId(ConsoleKey c)
		{
			switch (c) {
				case ConsoleKey.Tab:              return 9;
				case ConsoleKey.Clear:            return 12;
				case ConsoleKey.Enter:            return 13;
				case ConsoleKey.Pause:            return 19;
				case ConsoleKey.Escape:           return 27;
				case ConsoleKey.Spacebar:         return 32;
				case ConsoleKey.End:              return 35;
				case ConsoleKey.Home:             return 36;
				case ConsoleKey.LeftArrow:        return 37;
				case ConsoleKey.UpArrow:          return 38;
				case ConsoleKey.RightArrow:       return 39;
				case ConsoleKey.DownArrow:        return 40;
				case ConsoleKey.Select:           return 41;
				case ConsoleKey.Print:            return 42;
				case ConsoleKey.Execute:          return 43;
				case ConsoleKey.Insert:           return 45;
				case ConsoleKey.Delete:           return 46;
				case ConsoleKey.Help:             return 47;
				case ConsoleKey.LeftWindows:      return 91;
				case ConsoleKey.RightWindows:     return 92;
				case ConsoleKey.Applications:     return 93;
				case ConsoleKey.Sleep:            return 95;
				case ConsoleKey.NumPad0:          return 96;
				case ConsoleKey.NumPad1:          return 97;
				case ConsoleKey.NumPad2:          return 98;
				case ConsoleKey.NumPad3:          return 99;
				case ConsoleKey.NumPad4:          return 100;
				case ConsoleKey.NumPad5:          return 101;
				case ConsoleKey.NumPad6:          return 102;
				case ConsoleKey.NumPad7:          return 103;
				case ConsoleKey.NumPad8:          return 104;
				case ConsoleKey.NumPad9:          return 105;
				case ConsoleKey.Multiply:         return 106;
				case ConsoleKey.Add:              return 107;
				case ConsoleKey.Separator:        return 108;
				case ConsoleKey.Subtract:         return 109;
				case ConsoleKey.Decimal:          return 110;
				case ConsoleKey.Divide:           return 111;
				case ConsoleKey.F1:               return 112;
				case ConsoleKey.F2:               return 113;
				case ConsoleKey.F3:               return 114;
				case ConsoleKey.F4:               return 115;
				case ConsoleKey.F5:               return 116;
				case ConsoleKey.F6:               return 117;
				case ConsoleKey.F7:               return 118;
				case ConsoleKey.F8:               return 119;
				case ConsoleKey.F9:               return 120;
				case ConsoleKey.F10:              return 121;
				case ConsoleKey.F11:              return 122;
				case ConsoleKey.F12:              return 123;
				case ConsoleKey.F13:              return 124;
				case ConsoleKey.F14:              return 125;
				case ConsoleKey.F15:              return 126;
				case ConsoleKey.F16:              return 127;
				case ConsoleKey.F17:              return 128;
				case ConsoleKey.F18:              return 129;
				case ConsoleKey.F19:              return 130;
				case ConsoleKey.F20:              return 131;
				case ConsoleKey.F21:              return 132;
				case ConsoleKey.F22:              return 133;
				case ConsoleKey.F23:              return 134;
				case ConsoleKey.F24:              return 135;
				case ConsoleKey.BrowserBack:      return 166;
				case ConsoleKey.BrowserForward:   return 167;
				case ConsoleKey.BrowserRefresh:   return 168;
				case ConsoleKey.BrowserStop:      return 169;
				case ConsoleKey.BrowserSearch:    return 170;
				case ConsoleKey.BrowserFavorites: return 171;
				case ConsoleKey.BrowserHome:      return 172;
				case ConsoleKey.VolumeMute:       return 173;
				case ConsoleKey.VolumeDown:       return 174;
				case ConsoleKey.VolumeUp:         return 175;
				case ConsoleKey.MediaNext:        return 176;
				case ConsoleKey.MediaPrevious:    return 177;
				case ConsoleKey.MediaStop:        return 178;
				case ConsoleKey.MediaPlay:        return 179;
				case ConsoleKey.LaunchMail:       return 180;
				case ConsoleKey.LaunchApp1:       return 181;
				case ConsoleKey.LaunchApp2:       return 182;
				case ConsoleKey.Oem1:             return 186;
				case ConsoleKey.OemPlus:          return 187;
				case ConsoleKey.OemComma:         return 188;
				case ConsoleKey.OemMinus:         return 189;
				case ConsoleKey.OemPeriod:        return 190;
				case ConsoleKey.Oem2:             return 191;
				case ConsoleKey.Oem3:             return 192;
				case ConsoleKey.Oem4:             return 219;
				case ConsoleKey.Oem5:             return 220;
				case ConsoleKey.Oem6:             return 221;
				case ConsoleKey.Oem7:             return 222;
				case ConsoleKey.Oem8:             return 223;
				case ConsoleKey.Oem102:           return 226;
				case ConsoleKey.Process:          return 229;
				case ConsoleKey.Packet:           return 231;
				case ConsoleKey.Attention:        return 246;
				case ConsoleKey.CrSel:            return 247;
				case ConsoleKey.ExSel:            return 248;
				case ConsoleKey.EraseEndOfFile:   return 249;
				case ConsoleKey.Play:             return 250;
				case ConsoleKey.Zoom:             return 251;
				case ConsoleKey.NoName:           return 252;
				case ConsoleKey.Pa1:              return 253;
				case ConsoleKey.OemClear:         return 254;
			}

			return 0;
		}

		public static void Error(string s)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(s);
			Console.ResetColor();
			Console.WriteLine("Press any key to ignore and continue...");
			Console.ReadKey();
		}

		public static void Error(string s, object[] os)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(s, os);
			Console.ResetColor();
			Console.WriteLine("Press any key to ignore and continue...");
			Console.ReadKey();
		}

		public static void Error(string s, object o1)                       => Error(s, new[] {o1});
		public static void Error(string s, object o1, object o2)            => Error(s, new[] {o1, o2});
		public static void Error(string s, object o1, object o2, object o3) => Error(s, new[] {o1, o2, o3});

		public static void Error(string s, object o1, object o2, object o3, object o4) =>
			Error(s, new[] {o1, o2, o3, o4});
	}
}