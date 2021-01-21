using System;
using System.Collections.Generic;
using System.IO;
using MIDIKeyboard.dataFolder;

namespace MIDIKeyboard
{
    class FastSetup
    {

        private static readonly InputPort midi = new InputPort();

        public static void FastSetup_(ref bool runFastSetup)
        {

            if (File.Exists(Miscellaneous.Settings.data.key_data_path)) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Warning: you will now overwrite the data.txt file! any old setups will be deleted.\n");
                Console.ResetColor();
            }
            Console.WriteLine("what midi port do you whant to use");
            {
                Console.ForegroundColor = ConsoleColor.Black;
                for (int i = 0; i < midi.InputCount(); i++) {
                    if (i % 2 == 0)
                        Console.BackgroundColor = ConsoleColor.Gray;
                    else
                        Console.BackgroundColor = ConsoleColor.White;

                    Console.WriteLine(i + ".\t" + NativeMethods.midiInGetDevCaps((IntPtr)i).PadRight(32, ' '));
                }
                Console.ResetColor();
            }
            Console.Write("port: ");
            int.TryParse(Console.ReadLine(), out int resultat);
            Console.WriteLine("value set to " + resultat);
            int chanel = resultat;
            Console.Write("\nkeymode: \n  0. default \n  1. macro mode \n  2. 'F24' addon key (useful for autohotkey) \nmode: ");
            int.TryParse(Console.ReadLine(), out int keyMode);
            Console.WriteLine("value set to " + keyMode);
            Console.WriteLine("\npress the midi keys you want to use, and when you're done press any key on your keyboard\n");
            midi.Open(chanel);
            midi.Start();
            int old = 0;
            string hex4 = "0000";
            while (runFastSetup) {
                if (Console.KeyAvailable) {
                    runFastSetup = false;
                    break;
                }
                int value = midi.p;
                if (old != value) {
                    string valueHex = midi.pS;
                    if (hex4 != valueHex.Substring(valueHex.Length - 4)) {
                        if (hex4.Substring(hex4.Length - 2) != "D0") {
                            hex4 = valueHex.Substring(valueHex.Length - 4);

                            int hex4Con = int.Parse(hex4, System.Globalization.NumberStyles.HexNumber);

                            bool x2 = true;
                            using (IEnumerator<int[]> enumerator = Program.values.GetEnumerator()) {
                                while (enumerator.MoveNext()) {
                                    if (enumerator.Current[0] == hex4Con) {
                                        x2 = false;
                                        break;
                                    }
                                }
                            }
                            if (x2) {
                                Program.values.Add(new int[]
                                {
                                    hex4Con,
                                    keyMode,
                                    Program.GetTeken()
                                });
                            }
                        }

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
            }
            midi.Stop();
            midi.Close();
            using (StreamWriter file = new StreamWriter(Miscellaneous.Settings.data.key_data_path)) {
                file.WriteLine(chanel);
                foreach (int[] line in Program.values) {
                    file.WriteLine(string.Concat(new object[]
                    {
                        line[0],
                        ",",
                        line[1],
                        ",",
                        line[2]
                    }));
                }
            }

            Program.teken = 0;
        }
    }
}
