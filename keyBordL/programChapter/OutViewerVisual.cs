using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using keyBordL.dataFolder;

namespace keyBordL
{
    class OutViewerVisual
    {

        private static InputPort midi = new InputPort();
        static bool runOutViewerVisual;

        public static bool OutViewerVisual_(bool runOutViewerVisual_)
        {
            runOutViewerVisual = runOutViewerVisual_;
            Console.WriteLine("what midi port do you whant to use \n(0 to {0})", midi.OutputCount());
            {
                Console.ForegroundColor = ConsoleColor.Black;
                for (int i = 0; i < midi.OutputCount(); i++)
                {
                    if (i % 2 == 0)
                        Console.BackgroundColor = ConsoleColor.Gray;
                    else
                        Console.BackgroundColor = ConsoleColor.White;

                    Console.WriteLine(i + ".\t" + NativeMethods.midiOutGetDevCaps((IntPtr)i).PadRight(32, ' '));
                }
                Console.ResetColor();
            }
            Console.Write("port: ");
            int resultat2;
            int.TryParse(Console.ReadLine(), out resultat2);
            Console.WriteLine("value set to " + resultat2);
            Console.WriteLine("press ESC to exit");
            int chanel = resultat2;
            midi.OpenOut(chanel);


            int[][] linesInt = new int[0][];

            string path = @"visualProfiles\";
            {


                bool exists = System.IO.Directory.Exists(path);
                if (!exists)
                    System.IO.Directory.CreateDirectory(path);

                string[] files = Directory.GetFiles(path);

                Console.ForegroundColor = ConsoleColor.Black;
                for (int i = 0; i < files.Length; i++)
                {
                    if (i % 2 == 0)
                        Console.BackgroundColor = ConsoleColor.Gray;
                    else
                        Console.BackgroundColor = ConsoleColor.White;

                    Console.WriteLine(i + ".\t" + files[i].Split('\\')[1].PadRight(32, ' '));
                }
                Console.ResetColor();
                int datat;
                int.TryParse(Console.ReadLine(), out datat);
                path = files[datat];


                string[] lines = File.ReadAllLines(path, Encoding.UTF8);
                linesInt = new int[lines.Length][];
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] linesS = lines[i].Split(',');
                    linesInt[i] = new int[linesS.Length];
                    for (int k = 0; k < linesS.Length; k++)
                    {
                        linesInt[i][k] = int.Parse(linesS[k]);
                    }
                }

            }


            List<byte[]> usedShit = new List<byte[]>();

            while (runOutViewerVisual)
            {
                Console.Clear();


                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\n");
                for (int x = 0; x < linesInt[0].Length; x++)
                {
                    Console.Write(" ");
                    for (int y = 0; y < linesInt.Length; y++)
                    {
                        if (linesInt[x][y] == -1)
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            foreach (byte[] item in usedShit)
                            {
                                if (item[0] == linesInt[x][y])
                                {
                                    Console.BackgroundColor = ConsoleColor.White;
                                    break;
                                }
                            }
                        }
                        Console.Write(" "+linesInt[x][y].ToString().PadRight(3, ' '));
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                    Console.Write("\n");
                    Console.Write(" ");
                    for (int y = 0; y < linesInt.Length; y++)
                    {
                        if (linesInt[x][y] == -1)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write("    ");
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            string xx = " ";
                            foreach (byte[] item in usedShit)
                            {
                                if (item[0] == linesInt[x][y])
                                {
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
                outMsg[0] = inputV2();
                if (runOutViewerVisual)
                {

                    Console.Write("Velocity: ");
                    outMsg[1] = inputV2();
                    Console.WriteLine();
                }

                if (runOutViewerVisual)
                {

                    usedShit.Add(new byte[] { (byte)outMsg[0], (byte)outMsg[1] });
                    midi.MidiOutMsg((byte)outMsg[0], (byte)outMsg[1]);
                }

            }
            Console.WriteLine();
            Console.Write("0 set all? (y/n) ");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Enter:
                case ConsoleKey.D1:
                case ConsoleKey.Y:
                    foreach (byte[] item in usedShit)
                    {
                        midi.MidiOutMsg(item[0], 0);
                    }
                    break;
                default:
                    break;
            }

            midi.CloseOut();

            return runOutViewerVisual;

        }

        static int inputV2()
        {
            bool loop = true;
            string outLoop = "";

            while (loop)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
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
                    case ConsoleKey.Backspace:
                        outLoop += key.KeyChar;
                        break;
                    default:
                        break;
                }
            }
            int outP;
            int.TryParse(outLoop, out outP);
            return outP;
        }

    }
}
