using System;
using System.IO;
using System.Text;
using MIDIKeyboard.dataFolder;
using static MIDIKeyboard.miscellaneous.Miscellaneous;

namespace MIDIKeyboard.Run
{
    class LoadData
    {
        public static bool Load() => LoadFile() && SendOutputData() & LoadKeyData();

        static string[] array;

        static bool LoadFile(string path = "data.txt")
        {
            Console.Write("Loading data from path \"{0}\"...", path);
            if (File.Exists(path) && (array = File.ReadAllLines(path)).Length > 2) {
                Console.WriteLine(" Done.");
                return true;
            }
            Console.WriteLine(" Error.");
            Error("Error -> No data.");
            return false;
        }

        static bool SendOutputData()
        {
            if (array[0].Split(',').Length > 1) {
                Console.Write("Sending output data...");
                string[] ColorArray = array[0].Split(',');

                InputPort midiOut = new InputPort();
                midiOut.OpenOut(int.Parse(ColorArray[1].Trim()));

                //send output
                for (int i = 2; i + 1 < ColorArray.Length; i += 2) {
                    midiOut.MidiOutMsg((byte)int.Parse(ColorArray[i].Trim()), (byte)int.Parse(ColorArray[i + 1].Trim()));
                }

                midiOut.CloseOut();
                Console.WriteLine(" Done.");
                return true;
            }
            Console.WriteLine("No output data detected.");
            return true;
        }

        public static bool SendOutputDataZero()
        {
            if (array[0].Split(',').Length > 1) {
                Console.Write("Sending output data zero to set values...");
                string[] ColorArray = array[0].Split(',');

                InputPort midiOut = new InputPort();
                midiOut.OpenOut(int.Parse(ColorArray[1]));

                for (int i = 2; i + 1 < ColorArray.Length; i += 2) {
                    int x = int.Parse(ColorArray[i]);
                    midiOut.MidiOutMsg((byte)x, 0);
                }

                midiOut.CloseOut();
                Console.WriteLine(" Done.");
            }
            return true;
        }

        static bool LoadKeyData()
        {
            bool noErrors = true;
            Console.WriteLine("loading from file...");
            int chanel = int.Parse(array[0].Split(',')[0]);

            for (int lineID = 0; lineID < array.Length; lineID++) {
                if (array[lineID] != string.Concat(chanel)) {
                    string[] loadedValues = array[lineID].Split(new char[] { ',' });
                    int[] tempValues = new int[loadedValues.Length];
                    StringBuilder log = new StringBuilder();
                    bool isConverted = false;
                    bool error = false;
                    for (int i = 0; i < tempValues.Length; i++) {
                        string value = loadedValues[i].Trim();
                        if (isConverted |= !int.TryParse(value, out tempValues[i])) {
                            string[] convertValue;
                            if ((convertValue = value.Split('\'')).Length > 1) {
                                tempValues[i] = GetID(convertValue[1][0]);
                                if (convertValue[0][convertValue[0].Length - 1] == '-') {
                                    if (i < 2 || i > 1 && tempValues[1] != 1) {
                                        noErrors = false;
                                        error = true;
                                        Error("Error at line {0} -> \"{1}\", Negativ value not allowed: \"{2}\".", lineID, array[lineID], value);
                                        break;
                                    } else
                                        tempValues[i] = -tempValues[i];
                                }
                            } else if ((convertValue = value.Split('"')).Length > 1) {
                                tempValues[i] = GetID(convertValue[1][0]);
                                if (convertValue[0][convertValue[0].Length - 1] == '-') {
                                    if (i < 2 || i > 1 && tempValues[1] != 1) {
                                        noErrors = false;
                                        error = true;
                                        Error("Error at line {0} -> \"{1}\", Negativ value not allowed: \"{2}\".", lineID, array[lineID], value);
                                        break;
                                    } else
                                        tempValues[i] = -tempValues[i];
                                }
                            } else if (value.Length == 1) {
                                tempValues[i] = GetID(value[0]);
                            } else {
                                if (value.Length == 2 && value[0] == '-') {
                                    tempValues[i] = -GetID(value[1]);
                                } else {
                                    noErrors = false;
                                    error = true;
                                    Error("Error at line {0} -> \"{1}\", Unexpected character{2}: \"{3}\".", lineID, array[lineID], value.Length > 1 ? "s" : "", value);
                                    break;
                                }
                            }
                        }
                        log.Append(value);
                        log.Append(' ');
                    }
                    if (!error)
                    Program.values.Add(tempValues);
                    if (isConverted) {
                        log.Append("-> ");
                        foreach (int item in tempValues) {
                            log.Append(item);
                            log.Append(' ');
                        }
                    }
                    Console.WriteLine(log);
                }
            }
            return noErrors;
        }
    }
}
