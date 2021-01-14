using System;
using System.IO;
using System.Text;
using keyBordL.dataFolder;
using WindowsInput;
using WindowsInput.Native;
using static MIDIKeyboard.miscellaneous.Miscellaneous;

namespace keyBordL
{
    class Run
    {

        private static readonly InputPort midi = new InputPort();
        private static readonly InputSimulator IS = new InputSimulator();

        public static bool Run_(bool runPogram)
        {

            int chanel = 0;
            string[] array = new string[] { };
            bool error = false;

            if (Program.values.Count < 1) {
                Console.WriteLine("loading from file...");
                array = File.ReadAllLines("data.txt");
                chanel = int.Parse(array[0].Split(',')[0]);

                //color
                if (array[0].Split(',').Length > 1) {
                    string[] ColorArray = array[0].Split(',');

                    InputPort midiOut = new InputPort();
                    midiOut.OpenOut(int.Parse(ColorArray[1]));

                    for (int i = 2; i + 1 < ColorArray.Length; i += 2) {
                        midiOut.MidiOutMsg((byte)int.Parse(ColorArray[i]), (byte)int.Parse(ColorArray[i + 1]));
                    }

                    midiOut.CloseOut();
                }

                //load keys
                for (int lineID = 0; lineID < array.Length; lineID++) {
                    if (array[lineID] != string.Concat(chanel)) {
                        string[] loadedValues = array[lineID].Split(new char[] { ',' });
                        int[] tempValues = new int[loadedValues.Length];
                        StringBuilder log = new StringBuilder();
                        for (int i = 0; i < tempValues.Length; i++) {
                            string value = loadedValues[i].Trim();
                            if (!int.TryParse(value, out tempValues[i])) {
                                string[] convertValue;
                                if ((convertValue = value.Split('\'')).Length > 1) {
                                    tempValues[i] = GetID(convertValue[1][0]);
                                } else if ((convertValue = value.Split('"')).Length > 1) {
                                    tempValues[i] = GetID(convertValue[1][0]);
                                } else if (value.Length == 1) {
                                    tempValues[i] = GetID(value[0]);
                                } else {
                                    error = true;
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Error at {0} \"{1}\", Unexpected character{2}: \"{3}\".", lineID, array[lineID], value.Length > 1 ? "s" : "", value);
                                    Console.ResetColor();
                                    Console.WriteLine("Press any key to ignore and continue...");
                                    Console.ReadKey();
                                }
                            }
                            log.Append(value);
                            log.Append(' ');
                        }
                        Program.values.Add(tempValues);
                        Console.WriteLine(log);
                    }
                }
            }

            string[] valuesHex = new string[Program.values.Count];

            for (int i = 0; i < Program.values.Count; i++) {
                valuesHex[i] = Program.values[i][0].ToString("X").PadLeft(4, '0');
            }

            int old2 = 0;
            midi.Open(chanel);
            midi.Start();
            if (error)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else 
                Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nruning...\n");
            Console.ResetColor();
            while (runPogram) {
                /*
                if (Console.KeyAvailable) {
                    runPogram = false;
                }
                */
                Program.waitHandle.WaitOne();

                int value = midi.p;
                if (old2 != value) {

                    string valueHex = midi.pS;
                    string valueHex4 = valueHex.Substring(valueHex.Length - 4);

                    if (valueHex.Substring(valueHex.Length - 2) != "D0") {
                        int lenght_ = Program.values.Count;
                        for (int j = 0; j < lenght_; j++) {
                            if (valuesHex[j] == valueHex4) {
                                if (Program.values[j][1] == 0) { // key

                                    StringBuilder sb = new StringBuilder();
                                    VirtualKeyCode key = (VirtualKeyCode)Program.values[j][2];
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
                                    break;
                                } else if (Program.values[j][1] == 2) { // addonkey
                                    StringBuilder sb = new StringBuilder();
                                    VirtualKeyCode key = (VirtualKeyCode)Program.values[j][2];
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
                                    break;
                                } else if (Program.values[j][1] == 1 && valueHex.Length > 4) { // macro
                                    StringBuilder sb = new StringBuilder("macro '");
                                    for (int k = 2; k < Program.values[j].Length; k++) {
                                        if (Program.values[j][k] > 0) {
                                            VirtualKeyCode key = (VirtualKeyCode)Program.values[j][k];
                                            IS.Keyboard.KeyDown(key);
                                            sb.Append("+");
                                            sb.Append(key.ToString());
                                            sb.Append('\'');
                                        } else {
                                            VirtualKeyCode key = (VirtualKeyCode)(-Program.values[j][k]);
                                            IS.Keyboard.KeyUp(key);
                                            sb.Append(key.ToString());
                                            sb.Append('\'');
                                        }
                                    }
                                    sb.Append(" on:");
                                    Console.WriteLine(sb);
                                    break;
                                }
                            }
                        }
                    }
                    Console.WriteLine(valueHex + "\n");
                    old2 = value;


                }
                Program.waitHandle.Reset();
            }
            midi.Stop();
            midi.Close();
            Program.teken = 0;

            //color
            if (array[0].Split(',').Length > 1) {
                string[] ColorArray = array[0].Split(',');

                InputPort midiOut = new InputPort();
                midiOut.OpenOut(int.Parse(ColorArray[1]));

                for (int i = 2; i + 1 < ColorArray.Length; i += 2) {
                    int x = int.Parse(ColorArray[i]);
                    midiOut.MidiOutMsg((byte)x, (byte)(0));
                }

                midiOut.CloseOut();
            }

            return false;
        }


        private static bool addonButtonActive = false;
        private static readonly VirtualKeyCode addonButtonKey = VirtualKeyCode.F24;
        private static string AddonButton(bool keyStatus)
        {
            if (addonButtonActive == keyStatus)
                return "";

            addonButtonActive = keyStatus;

            if (addonButtonActive) {
                IS.Keyboard.KeyDown(addonButtonKey);
            } else {
                IS.Keyboard.KeyUp(addonButtonKey);
            }

            return (" + " + addonButtonKey.ToString());
        }

    }
}
