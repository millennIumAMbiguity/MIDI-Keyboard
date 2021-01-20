using System;
using MIDIKeyboard.dataFolder;

namespace MIDIKeyboard.Run
{
    class Run
    {

        private static readonly InputPort midi = new InputPort();

        public static bool Run_(ref bool runPogram)
        {

            int chanel = 0;
            bool error = false;

            //load data
            if (Program.values.Count < 1) {
                error = LoadData.Load();
            } else {
                Console.WriteLine("Data already loaded in memory...");
            }

            string[] valuesHex = new string[Program.values.Count];

            for (int i = 0; i < Program.values.Count; i++) {
                valuesHex[i] = Program.values[i][0].ToString("X").PadLeft(4, '0');
            }

            int oldValue = 0;
            midi.Open(chanel);
            midi.Start();
            if (!error)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nRuning...");
            Console.ResetColor();
            Console.WriteLine("Press escape to exit.\n");
            while (runPogram) {
                
                if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape) {
                    runPogram = false;
                    break;
                }
                
                if (Program.waitHandle.WaitOne(1000)) {
                    int value = midi.p;
                    if (oldValue != value) {

                        string valueHex = midi.pS;
                        string valueHex4 = valueHex.Substring(valueHex.Length - 4);

                        if (valueHex.Substring(valueHex.Length - 2) != "D0") {
                            int lenght_ = Program.values.Count;
                            for (int line = 0; line < lenght_; line++) {
                                if (valuesHex[line] == valueHex4) {
                                    if (Program.values[line][1] == 0) { // key
                                        KeyMode.Key(line, valueHex);
                                        break;
                                    }
                                    if (Program.values[line][1] == 2) { // addonkey
                                        KeyMode.Addonkey(line, valueHex);
                                        break;
                                    }
                                    if (Program.values[line][1] == 1) { // macro
                                        KeyMode.Macro(line, valueHex);
                                        break;
                                    }
                                }
                            }
                        }
                        Console.WriteLine(valueHex + "\n");
                        oldValue = value;
                    }
                }

                Program.waitHandle.Reset();
            }
            midi.Stop();
            midi.Close();
            Program.teken = 0;

            //remove color
            LoadData.SendOutputDataZero();

            return false;
        }

    }
}
