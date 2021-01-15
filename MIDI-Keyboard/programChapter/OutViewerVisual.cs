using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MIDIKeyboard.dataFolder;

namespace MIDIKeyboard
{
    class OutViewerVisual
    {

        private static readonly InputPort midi = new InputPort();
        static bool runOutViewerVisual;

        public static bool OutViewerVisual_(bool runOutViewerVisual_)
        {
            runOutViewerVisual = runOutViewerVisual_;
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


            string path = @"visualProfiles\";

            int[][] linesInt;
            {


                bool exists = System.IO.Directory.Exists(path);
                if (!exists)
                    System.IO.Directory.CreateDirectory(path);

                string[] files = Directory.GetFiles(path);

                Console.ForegroundColor = ConsoleColor.Black;
                for (int i = 0; i < files.Length; i++) {
                    if (i % 2 == 0)
                        Console.BackgroundColor = ConsoleColor.Gray;
                    else
                        Console.BackgroundColor = ConsoleColor.White;

                    Console.WriteLine(i + ".\t" + files[i].Split('\\')[1].PadRight(32, ' '));
                }
                Console.ResetColor();
                int.TryParse(Console.ReadLine(), out int datat);
                path = files[datat];


                string[] lines = File.ReadAllLines(path, Encoding.UTF8);
                linesInt = new int[lines.Length][];
                for (int i = 0; i < lines.Length; i++) {
                    string[] linesS = lines[i].Split(',');
                    linesInt[i] = new int[linesS.Length];
                    for (int k = 0; k < linesS.Length; k++) {
                        linesInt[i][k] = int.Parse(linesS[k]);
                    }
                }

            }


            List<byte[]> usedShit = new List<byte[]>();

            while (runOutViewerVisual) {
                Console.Clear();


                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\n");
                for (int x = 0; x < linesInt.Length; x++) {
                    Console.Write(" ");
                    for (int y = 0; y < linesInt[0].Length; y++) {
                        if (linesInt[x][y] == -1)
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        else {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            foreach (byte[] item in usedShit) {
                                if (item[0] == linesInt[x][y]) {
                                    Console.BackgroundColor = ConsoleColor.White;
                                    break;
                                }
                            }
                        }
                        Console.Write(" " + linesInt[x][y].ToString().PadRight(3, ' '));
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                    Console.Write("\n");
                    Console.Write(" ");
                    for (int y = 0; y < linesInt.Length; y++) {
                        if (linesInt[x][y] == -1) {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write("    ");
                        } else {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            string xx = " ";
                            foreach (byte[] item in usedShit) {
                                if (item[0] == linesInt[x][y]) {
                                    Console.BackgroundColor = ConsoleColor.White;
                                    xx += item[1].ToString();
                                    break;
                                }
                            }
                            Console.Write(xx.PadRight(4, ' '));
                        }
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                    Console.Write("\n\n");
                }
                Console.ResetColor();

                int[] outMsg = new int[2];

                Console.Write("Pitch: ");
                outMsg[0] = InputV2();
                if (runOutViewerVisual) {

                    Console.Write("Velocity: ");
                    outMsg[1] = InputV2();
                    Console.WriteLine();
                }

                if (runOutViewerVisual) {

                    usedShit.Add(new byte[] { (byte)outMsg[0], (byte)outMsg[1] });
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
                            allLines[0] += "," + usedShit[i][0] + "," + usedShit[i][1];
                        }

                        File.WriteAllLines("data.txt", allLines);
                        break;
                    case ConsoleKey.D2:
                        allLines = File.ReadAllLines("data.txt");

                        for (int i = 0; i < usedShit.Count; i++) {
                            allLines[0] += "," + usedShit[i][0] + "," + usedShit[i][1];
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
                    foreach (byte[] item in usedShit) {
                        midi.MidiOutMsg(item[0], 0);
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
                        runOutViewerVisual = false;
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
