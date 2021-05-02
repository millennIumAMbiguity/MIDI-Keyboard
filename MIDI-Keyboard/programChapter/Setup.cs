using System;
using System.Globalization;
using System.IO;
using System.Linq;
using MIDIKeyboard.dataFolder;
using MIDIKeyboard.Miscellaneous;
using MIDIKeyboard.Run;

namespace MIDIKeyboard
{
	internal class Setup
	{
		private static readonly InputPort midi = new InputPort();

		public static void Setup_(ref bool runSetup)
		{
			bool dataExist = Program.values.Count > 0;
			bool exist     = File.Exists(Settings.data.key_data_path);
			if (exist) {
				if (dataExist) {
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine(
						"Warning: you will now modify the data.txt file! any old setups will be modified.\n");
				} else {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(
						"Warning: you will now overwrite the data.txt file! any old setups will be deleted.\n");
				}

				Console.ResetColor();
			}

			Console.WriteLine("what midi port do you want to use");
			{
				Console.ForegroundColor = ConsoleColor.Black;
				for (int i = 0; i < midi.InputCount(); i++) {
					if (i % 2 == 0)
						Console.BackgroundColor = ConsoleColor.Gray;
					else
						Console.BackgroundColor = ConsoleColor.White;

					Console.WriteLine(i + ".\t" + NativeMethods.midiInGetDevCaps((IntPtr) i).PadRight(32, ' '));
				}

				Console.ResetColor();
			}
			Console.Write("port: ");
			int.TryParse(Console.ReadLine(), out int resultat);
			Console.WriteLine("value set to " + resultat);
			int chanel = resultat;
			/*
			Console.Write(
				"\nkeymode: \n  0. default \n  1. macro mode \n  2. 'F24' addon key (useful for autohotkey) \nmode: ");
			int.TryParse(Console.ReadLine(), out int keyMode);
			Console.WriteLine("value set to " + keyMode);
			*/
			int keyMode = 0;
			Console.WriteLine(
				"\npress a midi key and then press a keyboard key.\n");
			midi.Open(chanel);
			midi.Start();
			int    old     = 0;
			string hex4    = "0000";
			int    hex4Con = 0;
			while (runSetup) {
				if (Console.KeyAvailable) {
					ConsoleKeyInfo newKey = Console.ReadKey();
					if (newKey.Key == ConsoleKey.Escape) {
						runSetup = false;
						break;
					}

					Console.CursorLeft      = 0;
					Console.BackgroundColor = ConsoleColor.White;
					Console.ForegroundColor = ConsoleColor.Black;
					Console.Write(newKey.Key.ToString());
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.DarkGray;
					if (Console.CursorLeft < 3)
						Console.CursorLeft = 3;
					Console.Write(" => ");
					if (Program.values.All(t => t[0] != hex4Con)) {
						Program.values.Add(
							new[] {
								hex4Con,
								keyMode,
								Miscellaneous.Miscellaneous.GetId(newKey.KeyChar)
							});
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write($"assigned to {hex4Con}.\n");
					} else
						Console.Write($"{hex4Con} is already assigned to a key.\n");
				}

				int value = midi.p;
				if (old == value) continue;
				string valueHex = midi.pS;
				string newHex4  = valueHex.Substring(valueHex.Length - 4);
				if (hex4 != newHex4)
					if (newHex4.Substring(newHex4.Length - 2) != "D0") {
						hex4    = newHex4;
						hex4Con = int.Parse(hex4, NumberStyles.HexNumber);
					}

				if (valueHex.Substring(valueHex.Length - 2) == "D0") {
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.Write(valueHex.PadLeft(6, ' ').Substring(0, 4));
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.Write("D0 ");
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine(value);
					Console.ForegroundColor = ConsoleColor.White;
				} else {
					Console.Write(valueHex.PadLeft(6, ' ').Substring(0, 2));
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write(hex4 + " ");
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine(value);
					Console.ForegroundColor = ConsoleColor.White;
				}

				old = value;
			}

			midi.Stop();
			midi.Close();
			using (var file = new StreamWriter(Settings.data.key_data_path)) {
				if (!dataExist)
					file.WriteLine(chanel);
				else file.WriteLine(LoadData.rawData[0]);
				foreach (int[] line in Program.values)
					file.WriteLine(string.Concat(line[0], ",", line[1], ",", line[2]));
			}

			Program.teken = 0;
		}
	}
}