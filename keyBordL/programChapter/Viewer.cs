using System;
using System.Text;
using keyBordL.dataFolder;

namespace keyBordL
{
    class Viewer
    {

        private static InputPort midi = new InputPort();

        public static bool Viewer_(bool runViewer)
        {


            Console.WriteLine("what midi port do you whant to use");
            {
                Console.ForegroundColor = ConsoleColor.Black;
                for (int i = 0; i < midi.InputCount(); i++)
                {
                    if (i % 2 == 0)
                        Console.BackgroundColor = ConsoleColor.Gray;
                    else
                        Console.BackgroundColor = ConsoleColor.White;

                    Console.WriteLine(i + ".\t" + NativeMethods.midiInGetDevCaps((IntPtr)i).PadRight(32, ' '));
                }
                Console.ResetColor();
            }
            Console.Write("port: ");
            int resultat2;
            int.TryParse(Console.ReadLine(), out resultat2);
            Console.WriteLine("value set to " + resultat2);
            Console.WriteLine("press ESC to exit");
            int chanel = resultat2;
            midi.Open(chanel);
            midi.Start();
            int old = 0;
            int value = 0;
            string valueHex = "";
            string hex4 = "0000";
            bool sw = false;


            while (runViewer)
            {
                if (Console.KeyAvailable)
                {

                    StringBuilder sb = new StringBuilder();

                    ConsoleKeyInfo key = Console.ReadKey();

                    if (key.Key == ConsoleKey.Escape)
                        runViewer = false;

                    Console.Write("\b  ");

                    Console.ForegroundColor = ConsoleColor.Black;
                    if (sw)
                        Console.BackgroundColor = ConsoleColor.Gray;
                    else
                        Console.BackgroundColor = ConsoleColor.White;

                    sw = !sw;

                    byte bkey = (byte)key.KeyChar;
                    if (bkey > 96 && bkey < 123)
                        bkey -= 32;
                    else
                    {
                        switch (key.Key)
                        {
                            case ConsoleKey.Backspace:
                                bkey = 8;
                                break;
                            case ConsoleKey.Tab:
                                bkey = 9;
                                break;
                            case ConsoleKey.Clear:
                                bkey = 12;
                                break;
                            case ConsoleKey.Enter:
                                bkey = 13;
                                break;
                            case ConsoleKey.Pause:
                                bkey = 19;
                                break;
                            case ConsoleKey.Escape:
                                bkey = 27;
                                break;
                            case ConsoleKey.Spacebar:
                                bkey = 32;
                                break;
                            case ConsoleKey.PageUp:
                                bkey = 33;
                                break;
                            case ConsoleKey.PageDown:
                                bkey = 34;
                                break;
                            case ConsoleKey.End:
                                bkey = 35;
                                break;
                            case ConsoleKey.Home:
                                bkey = 36;
                                break;
                            case ConsoleKey.LeftArrow:
                                bkey = 37;
                                break;
                            case ConsoleKey.UpArrow:
                                bkey = 38;
                                break;
                            case ConsoleKey.RightArrow:
                                bkey = 39;
                                break;
                            case ConsoleKey.DownArrow:
                                bkey = 40;
                                break;
                            case ConsoleKey.Select:
                                bkey = 41;
                                break;
                            case ConsoleKey.Print:
                                bkey = 42;
                                break;
                            case ConsoleKey.Execute:
                                bkey = 43;
                                break;
                            case ConsoleKey.PrintScreen:
                                bkey = 44;
                                break;
                            case ConsoleKey.Insert:
                                bkey = 45;
                                break;
                            case ConsoleKey.Delete:
                                bkey = 46;
                                break;
                            case ConsoleKey.Help:
                                bkey = 47;
                                break;
                            case ConsoleKey.LeftWindows:
                                bkey = 91;
                                break;
                            case ConsoleKey.RightWindows:
                                bkey = 92;
                                break;
                            case ConsoleKey.Applications:
                                bkey = 93;
                                break;
                            case ConsoleKey.Sleep:
                                bkey = 95;
                                break;
                            case ConsoleKey.NumPad0:
                                bkey = 96;
                                break;
                            case ConsoleKey.NumPad1:
                                bkey = 97;
                                break;
                            case ConsoleKey.NumPad2:
                                bkey = 98;
                                break;
                            case ConsoleKey.NumPad3:
                                bkey = 99;
                                break;
                            case ConsoleKey.NumPad4:
                                bkey = 100;
                                break;
                            case ConsoleKey.NumPad5:
                                bkey = 101;
                                break;
                            case ConsoleKey.NumPad6:
                                bkey = 102;
                                break;
                            case ConsoleKey.NumPad7:
                                bkey = 103;
                                break;
                            case ConsoleKey.NumPad8:
                                bkey = 104;
                                break;
                            case ConsoleKey.NumPad9:
                                bkey = 105;
                                break;
                            case ConsoleKey.Multiply:
                                bkey = 106;
                                break;
                            case ConsoleKey.Add:
                                bkey = 107;
                                break;
                            case ConsoleKey.Separator:
                                bkey = 108;
                                break;
                            case ConsoleKey.Subtract:
                                bkey = 109;
                                break;
                            case ConsoleKey.Decimal:
                                bkey = 110;
                                break;
                            case ConsoleKey.Divide:
                                bkey = 111;
                                break;
                            case ConsoleKey.F1:
                                bkey = 112;
                                break;
                            case ConsoleKey.F2:
                                bkey = 113;
                                break;
                            case ConsoleKey.F3:
                                bkey = 114;
                                break;
                            case ConsoleKey.F4:
                                bkey = 115;
                                break;
                            case ConsoleKey.F5:
                                bkey = 116;
                                break;
                            case ConsoleKey.F6:
                                bkey = 117;
                                break;
                            case ConsoleKey.F7:
                                bkey = 118;
                                break;
                            case ConsoleKey.F8:
                                bkey = 119;
                                break;
                            case ConsoleKey.F9:
                                bkey = 120;
                                break;
                            case ConsoleKey.F10:
                                bkey = 121;
                                break;
                            case ConsoleKey.F11:
                                bkey = 122;
                                break;
                            case ConsoleKey.F12:
                                bkey = 123;
                                break;
                            case ConsoleKey.F13:
                                bkey = 124;
                                break;
                            case ConsoleKey.F14:
                                bkey = 125;
                                break;
                            case ConsoleKey.F15:
                                bkey = 126;
                                break;
                            case ConsoleKey.F16:
                                bkey = 127;
                                break;
                            case ConsoleKey.F17:
                                bkey = 128;
                                break;
                            case ConsoleKey.F18:
                                bkey = 129;
                                break;
                            case ConsoleKey.F19:
                                bkey = 130;
                                break;
                            case ConsoleKey.F20:
                                bkey = 131;
                                break;
                            case ConsoleKey.F21:
                                bkey = 132;
                                break;
                            case ConsoleKey.F22:
                                bkey = 133;
                                break;
                            case ConsoleKey.F23:
                                bkey = 134;
                                break;
                            case ConsoleKey.F24:
                                bkey = 135;
                                break;
                            case ConsoleKey.BrowserBack:
                                bkey = 166;
                                break;
                            case ConsoleKey.BrowserForward:
                                bkey = 167;
                                break;
                            case ConsoleKey.BrowserRefresh:
                                bkey = 168;
                                break;
                            case ConsoleKey.BrowserStop:
                                bkey = 169;
                                break;
                            case ConsoleKey.BrowserSearch:
                                bkey = 170;
                                break;
                            case ConsoleKey.BrowserFavorites:
                                bkey = 171;
                                break;
                            case ConsoleKey.BrowserHome:
                                bkey = 172;
                                break;
                            case ConsoleKey.VolumeMute:
                                bkey = 173;
                                break;
                            case ConsoleKey.VolumeDown:
                                bkey = 174;
                                break;
                            case ConsoleKey.VolumeUp:
                                bkey = 175;
                                break;
                            case ConsoleKey.MediaNext:
                                bkey = 176;
                                break;
                            case ConsoleKey.MediaPrevious:
                                bkey = 177;
                                break;
                            case ConsoleKey.MediaStop:
                                bkey = 178;
                                break;
                            case ConsoleKey.MediaPlay:
                                bkey = 179;
                                break;
                            case ConsoleKey.LaunchMail:
                                bkey = 180;
                                break;
                            case ConsoleKey.LaunchMediaSelect:
                                bkey = 181;
                                break;
                            case ConsoleKey.LaunchApp1:
                                bkey = 182;
                                break;
                            case ConsoleKey.LaunchApp2:
                                bkey = 183;
                                break;
                            case ConsoleKey.Oem1:
                                bkey = 186;
                                break;
                            case ConsoleKey.OemPlus:
                                bkey = 187;
                                break;
                            case ConsoleKey.OemComma:
                                bkey = 188;
                                break;
                            case ConsoleKey.OemMinus:
                                bkey = 189;
                                break;
                            case ConsoleKey.OemPeriod:
                                bkey = 190;
                                break;
                            case ConsoleKey.Oem2:
                                bkey = 191;
                                break;
                            case ConsoleKey.Oem3:
                                bkey = 192;
                                break;
                            case ConsoleKey.Oem4:
                                bkey = 219;
                                break;
                            case ConsoleKey.Oem5:
                                bkey = 220;
                                break;
                            case ConsoleKey.Oem6:
                                bkey = 221;
                                break;
                            case ConsoleKey.Oem7:
                                bkey = 222;
                                break;
                            case ConsoleKey.Oem8:
                                bkey = 223;
                                break;
                            case ConsoleKey.Oem102:
                                bkey = 226;
                                break;
                            case ConsoleKey.Process:
                                bkey = 229;
                                break;
                            case ConsoleKey.Packet:
                                bkey = 231;
                                break;
                            case ConsoleKey.Attention:
                                bkey = 246;
                                break;
                            case ConsoleKey.CrSel:
                                bkey = 247;
                                break;
                            case ConsoleKey.ExSel:
                                bkey = 248;
                                break;
                            case ConsoleKey.EraseEndOfFile:
                                bkey = 249;
                                break;
                            case ConsoleKey.Play:
                                bkey = 250;
                                break;
                            case ConsoleKey.Zoom:
                                bkey = 251;
                                break;
                            case ConsoleKey.NoName:
                                bkey = 252;
                                break;
                            case ConsoleKey.Pa1:
                                bkey = 253;
                                break;
                            case ConsoleKey.OemClear:
                                bkey = 254;
                                break;
                            default:
                                break;
                        }
                    }


                    sb.Append(key.Key);
                    sb.Append(" ");
                    sb.Append(bkey);
                    Console.WriteLine(sb);

                    Console.ResetColor();



                }
                value = midi.p;
                if (old != value)
                {


                    sw = false;

                    valueHex = midi.pS;

                    hex4 = valueHex.Substring(valueHex.Length - 4);


                    if (hex4.Substring(hex4.Length - 2) == "D0")
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(valueHex.PadLeft(6, ' ').Substring(0, 4));
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("D0 ");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(value);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        /*
                        if (valueHex.Length > 4)
                        {

                        }*/

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





            return false;

        }

    }
}
