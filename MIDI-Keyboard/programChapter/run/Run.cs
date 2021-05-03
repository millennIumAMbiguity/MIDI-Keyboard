using System;
using MIDIKeyboard.dataFolder;
using MIDIKeyboard.Miscellaneous;

namespace MIDIKeyboard.Run
{
	internal class Run
	{
		private static readonly InputPort midi = new InputPort();

		public static void Run_(ref bool runPogram)
		{
			int  chanel = 0;
			bool error  = false;

			//load data
			if (Program.values.Count < 1 || Settings.data.force_load_data_on_run)
				error = LoadData.Load(Settings.data.key_data_path);
			else
				Console.WriteLine("Data already loaded in memory...");


			string[] valuesHex = new string[Program.values.Count];

			for (int i = 0; i < Program.values.Count; i++)
				valuesHex[i] = Program.values[i][0].ToString("X").PadLeft(4, '0');


			midi.Open(chanel);
			midi.Start();
			if (!error)
				Console.ForegroundColor = ConsoleColor.Yellow;
			else
				Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("\nRuning...");
			Console.ResetColor();
			Console.WriteLine("Press escape to exit.\n");

			//run application loop
			AppLoop(ref runPogram, valuesHex);

			//stop and clsoe MIDI port.
			midi.Stop();
			midi.Close();
			Program.teken = 0;

			//remove color
			LoadData.SendOutputDataZero();
		}

		private static void AppLoop(ref bool runPogram, string[] valuesHex)
		{
			int oldValue = 0;
			while (runPogram) {
				if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape) {
					runPogram = false;
					return;
				}

				if (Program.waitHandle.WaitOne(1000)) {
					int value = midi.p;
					if (oldValue != value) {
						string valueHex  = midi.pS;
						string valueHex4 = valueHex.Substring(valueHex.Length - 4);

						if (valueHex.Substring(valueHex.Length - 2) != "D0") {
							int lenght = Program.values.Count;
							for (int lineIndex = 0; lineIndex < lenght; lineIndex++)
								if (valuesHex[lineIndex] == valueHex4) {

									if (Program.values[lineIndex][1] == 0) { // key
										KeyMode.Key(lineIndex, valueHex);
										break;
									}
									if (Program.values[lineIndex][1] == 2) { // addonkey
										KeyMode.Addonkey(lineIndex, valueHex);
										break;
									}
									if (Program.values[lineIndex][1] == 1) { // macro
										KeyMode.Macro(lineIndex, valueHex);
										break;
									}
								}
						}

						Console.WriteLine(valueHex + "\n");
						oldValue = value;
					}
				}

				Program.waitHandle.Reset();
			}
		}
	}
}