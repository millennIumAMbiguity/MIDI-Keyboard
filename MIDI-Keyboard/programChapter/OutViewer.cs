using System;
using System.Collections.Generic;
using System.IO;
using keyBordL.dataFolder;

namespace keyBordL
{
    class OutViewer
    {

        private static readonly InputPort midi = new InputPort();
        static bool runOutViewer;

        public static bool OutViewer_(bool runOutViewer_)
        {
            runOutViewer = runOutViewer_;
            Console.WriteLine("what midi port do you whant to use");
            {
                Console.ForegroundColor = ConsoleColor.Black;
                for (int i = 0; i < midi.OutputCount(); i++) {
                    if (i % 2 == 0)
                        Console.BackgroundColor = ConsoleColor.Gray;
                    else
                        Console.BackgroundColor = ConsoleColor.White;

                    Console.WriteLine(i + ".\t" + NativeMethods.midiOutGetDevCaps((IntPtr)i).PadRight(32, ' '));
                }
                Console.ResetColor();
            }
            Console.Write("port: ");
            int.TryParse(Console.ReadLine(), out int resultat2);
            Console.WriteLine("value set to " + resultat2);
            Console.WriteLine("press ESC to exit");
            int chanel = resultat2;
            midi.OpenOut(chanel);

            List<byte> usedShit = new List<byte>();
            List<byte> usedShit2 = new List<byte>();

            while (runOutViewer) {
                int[] outMsg = new int[2];

                Console.Write("Pitch: ");
                outMsg[0] = InputV2();
                if (runOutViewer) {

                    Console.Write("Velocity: ");
                    outMsg[1] = InputV2();
                    Console.WriteLine();
                }
                if (runOutViewer) {

                    usedShit.Add((byte)outMsg[0]);
                    usedShit2.Add((byte)outMsg[1]);
                    midi.MidiOutMsg((byte)outMsg[0], (byte)outMsg[1]);
                }

            }

            if (File.Exists("data.txt")) {
                string[] allLines;
                Console.WriteLine();
                Console.Write("save to data.txt? \n1. overwrite old color profile.\n2. add to the existing color profile.\n3. dont save.\n ");
                switch (Console.ReadKey().Key) {
                    case ConsoleKey.D1:
                        allLines = File.ReadAllLines("data.txt");
                        allLines[0] = allLines[0].Split(',')[0] + "," + resultat2;

                        for (int i = 0; i < usedShit.Count; i++) {
                            allLines[0] += "," + usedShit[i] + "," + usedShit2[i];
                        }

                        File.WriteAllLines("data.txt", allLines);
                        break;
                    case ConsoleKey.D2:
                        allLines = File.ReadAllLines("data.txt");

                        for (int i = 0; i < usedShit.Count; i++) {
                            allLines[0] += "," + usedShit[i] + "," + usedShit2[i];
                        }

                        File.WriteAllLines("data.txt", allLines);
                        break;
                    default:
                        break;
                }
            }


            Console.WriteLine();
            Console.Write("0 set all? (y/n) ");
            switch (Console.ReadKey().Key) {
                case ConsoleKey.Enter:
                case ConsoleKey.D1:
                case ConsoleKey.Y:
                    foreach (byte item in usedShit) {
                        midi.MidiOutMsg(item, 0);
                    }
                    break;
                default:
                    break;
            }

            midi.CloseOut();

            return false;

        }

        static int InputV2()
        {
            bool loop = true;
            string outLoop = "";

            while (loop) {
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key) {
                    case ConsoleKey.Escape:
                        runOutViewer = false;
                        loop = false;
                        break;
                    case ConsoleKey.Enter:
                        loop = false;
                        break;
                    case ConsoleKey.D1:
                    case ConsoleKey.D2:
                    case ConsoleKey.D3:
                    case ConsoleKey.D4:
                    case ConsoleKey.D5:
                    case ConsoleKey.D6:
                    case ConsoleKey.D7:
                    case ConsoleKey.D8:
                    case ConsoleKey.D9:
                    case ConsoleKey.D0:
                        outLoop += key.KeyChar;
                        break;
                    case ConsoleKey.Backspace:
                    case ConsoleKey.Delete:
                        outLoop = "";
                        break;
                    default:
                        break;
                }
            }
            int.TryParse(outLoop, out int outP);
            return outP;
        }

    }
}
