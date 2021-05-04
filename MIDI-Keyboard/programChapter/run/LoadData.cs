using System;
using System.IO;
using System.Text;
using MIDIKeyboard.dataFolder;
using static MIDIKeyboard.Miscellaneous.Miscellaneous;

namespace MIDIKeyboard.Run
{
	internal class LoadData
	{
		public static string[] rawData;
		public static bool     Load()                         => LoadFile()     && SendOutputData() & LoadKeyData();
		public static bool     Load(string              path) => LoadFile(path) && SendOutputData() & LoadKeyData();
		public static bool     LoadWithoutOutput(string path) => LoadFile(path) && LoadKeyData();

		private static bool LoadFile(string path = "data.txt")
		{
			Console.Write("Loading data from path \"{0}\"...", path);
			if (File.Exists(path) && (rawData = File.ReadAllLines(path)).Length > 2) {
				Console.WriteLine(" Done.");
				return true;
			}

			Console.WriteLine(" Error.");
			Error("Error -> No data.");
			return false;
		}

		private static bool SendOutputData()
		{
			if (rawData[0].Split(',').Length > 1) {
				Console.Write("Sending output data...");
				string[] ColorArray = rawData[0].Split(',');

				var midiOut = new InputPort();
				midiOut.OpenOut(int.Parse(ColorArray[1].Trim()));

				//send output
				for (int i = 2; i + 1 < ColorArray.Length; i += 2)
					midiOut.MidiOutMsg(
						(byte) int.Parse(ColorArray[i].Trim()), (byte) int.Parse(ColorArray[i + 1].Trim()));

				midiOut.CloseOut();
				Console.WriteLine(" Done.");
				return true;
			}

			Console.WriteLine("No output data detected.");
			return true;
		}

		public static bool SendOutputDataZero()
		{
			if (rawData[0].Split(',').Length <= 1) return true;
			Console.Write("Sending output data zero to set values...");
			string[] ColorArray = rawData[0].Split(',');

			var midiOut = new InputPort();
			midiOut.OpenOut(int.Parse(ColorArray[1]));

			for (int i = 2; i + 1 < ColorArray.Length; i += 2) {
				int x = int.Parse(ColorArray[i]);
				midiOut.MidiOutMsg((byte) x, 0);
			}

			midiOut.CloseOut();
			Console.WriteLine(" Done.");

			return true;
		}

		private static bool LoadKeyData()
		{
			bool noErrors = true;
			Console.WriteLine("loading from file...");
			int chanel = int.Parse(rawData[0].Split(',')[0]);

			for (int lineId = 1; lineId < rawData.Length; lineId++)
				if (rawData[lineId] != string.Concat(chanel)) {
					string[] loadedValues = rawData[lineId].Split(',');
					if (loadedValues.Length == 2) //add missing mode id
						loadedValues = new[]
							{loadedValues[0], loadedValues[1].Contains("\"") ? "1" : "0", loadedValues[1]};
					else if (loadedValues[1].Contains("\"")) { //if mode is a string, assume it as a macro argument
						string[] newLoadedValues = new string[loadedValues.Length + 1];
						newLoadedValues[0] = loadedValues[0];
						newLoadedValues[1] = "1";
						for (int i = 1; i < loadedValues.Length; i++)
							newLoadedValues[i + 1] = loadedValues[i];
					}

					int[] tempValues = new int[loadedValues.Length];
					var   log        = new StringBuilder();
					bool  error      = false;
					for (int i = 0; i < tempValues.Length; i++) {
						string value = loadedValues[i].Trim();
						if (!int.TryParse(value, out tempValues[i])) {
							if (i < 2) {
								noErrors = false;
								error    = true;
								Error(
									"Error at line {0} -> \"{1}\", Value must be a full number: \"{2}\".", lineId,
									rawData[lineId],                                                       value);
								break;
							}

							string[] convertValue;
							if ((convertValue = value.Split('\'')).Length > 1) { //'a' -> GetId("a")
								tempValues[i] = GetId(convertValue[1][0]);
								if (convertValue[0][convertValue[0].Length - 1] == '-') {
									if (tempValues[1] != 1) {
										noErrors = false;
										error    = true;
										Error(
											"Error at line {0} -> \"{1}\", Negative value not allowed: \"{2}\".",
											lineId,
											rawData[lineId], value);
										break;
									}

									tempValues[i] = -tempValues[i];
								}
							} else if ((convertValue = value.Split('"')).Length > 1) {
								//"a" -> GetId("a") //"abc" -> GetId("a"), GetId("b"), GetId("c")

								if (convertValue.Length > 4) {
									noErrors = false;
									error    = true;
									Error(
										"Error at line {0} -> \"{1}\", Multiple strings not allowed: \"{2}\".", lineId,
										rawData[lineId],                                                        value);
									break;
								}

								if (tempValues[1] != 1) {
									if (convertValue[0].Length != 0) {
										noErrors = false;
										error    = true;
										Error(
											"Error at line {0} -> \"{1}\", Unexpected character{2}: \"{3}\".", lineId,
											rawData[lineId], convertValue[0].Length > 1 ? "s" : "", convertValue[0]);
										break;
									}

									if (convertValue[1].Length == 1) { tempValues[i] = GetId(convertValue[1][0]); }
								} else {
									if (convertValue[0].Length > 1 || (convertValue[0].Length != 0 &&
									                                   (convertValue[0][0] != '-' &&
									                                    convertValue[0][0] != '+'))) {
										noErrors = false;
										error    = true;
										Error(
											"Error at line {0} -> \"{1}\", Unexpected character{2}: \"{3}\".", lineId,
											rawData[lineId], convertValue[0].Length > 1 ? "s" : "", convertValue[0]);
										break;
									}

									if (convertValue[0].Length == 1 && convertValue[1].Length == 1) {
										tempValues[i] = GetId(convertValue[1][0]);
										if (convertValue[0][0] != '-')
											tempValues[i] = -tempValues[i];
									} else {
										int length = convertValue[0].Length == 1
											? convertValue[1].Length                          - 1
											: convertValue[1].Length + convertValue[1].Length - 1;

										//copy old values to new array with a gap for the new values.
										int[] newTempV = new int[tempValues.Length + length];
										for (int j = 0; j < tempValues.Length; j++) {
											if (j < i) { newTempV[j] = tempValues[j]; } else if (j > i) {
												newTempV[j + length] = tempValues[j];
											}
										}

										// insert new values
										if (convertValue[0].Length == 1) {
											if (convertValue[0][0] == '-')
												for (int j = 0; j < convertValue[1].Length; j++) {
													newTempV[j + i] = -GetId(convertValue[1][j]);
												}
											else
												for (int j = 0; j < convertValue[1].Length; j++) {
													newTempV[j + i] = GetId(convertValue[1][j]);
												}
										} else
											for (int j = 0; j < convertValue[1].Length; j++) {
												newTempV[j * 2 + i]     = GetId(convertValue[1][j]);
												newTempV[j * 2 + i + 1] = -GetId(convertValue[1][j]);
											}

										//apply new values
										tempValues =  newTempV;
										i          += length;
									}
								}
							} else if (value.Length == 1) //a -> GetId("a")
								tempValues[i] = GetId(value[0]);
							else { //-a -> -GetId("a")
								if (value.Length == 2 && value[0] == '-')
									tempValues[i] = -GetId(value[1]);
								else {
									noErrors = false;
									error    = true;
									Error(
										"Error at line {0} -> \"{1}\", Unexpected character{2}: \"{3}\".", lineId,
										rawData[lineId], value.Length > 1 ? "s" : "", value);
									break;
								}
							}
						}

						log.Append(value);
						log.Append(' ');
					}

					if (!error)
						Program.values.Add(tempValues);

					log.Append("-> ");
					foreach (int item in tempValues) {
						log.Append(item);
						log.Append(' ');
					}


					Console.WriteLine(log);
				}

			return noErrors;
		}
	}
}