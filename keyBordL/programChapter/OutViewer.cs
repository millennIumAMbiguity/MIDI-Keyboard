using System;
using System.Collections.Generic;
using System.Text;
using keyBordL.dataFolder;

namespace keyBordL
{
    class OutViewer
    {

        private static InputPort midi = new InputPort();
        static bool runOutViewer;

        public static bool OutViewer_(bool runOutViewer_)
        {
            runOutViewer = runOutViewer_;
            Console.WriteLine("what midi port do you whant to use");
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

            List<byte> usedShit = new List<byte>();

            while (runOutViewer)
            {
                int[] outMsg = new int[2];

                Console.Write("Pitch: ");
                outMsg[0] = inputV2();
                if (runOutViewer)
                {

                    Console.Write("Velocity: ");
                    outMsg[1] = inputV2();
                    Console.WriteLine();
                }
                if (runOutViewer)
                {

                    usedShit.Add((byte)outMsg[0]);
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
                    foreach (byte item in usedShit)
                    {
                        midi.MidiOutMsg(item, 0);
                    }
                    break;
                default:
                    break;
            }

            midi.CloseOut();

            return runOutViewer;

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
