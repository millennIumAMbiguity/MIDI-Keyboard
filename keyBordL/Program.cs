using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using keyBordL.dataFolder;
using WindowsInput;
using WindowsInput.Native;

namespace keyBordL
{
    // Token: 0x02000002 RID: 2
    internal class Program
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        private static void Main(string[] args)
        {

            while (Program.runForm)
            {
                Console.Clear();
                if (Program.runSetup)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Warning: you will now overwrite the data.txt file! any old setups will be deleted.\n");
                    Console.ResetColor();
                    Console.WriteLine("what midi port do you whant to use \n(they start at 0 and count up from that, remember that some devices use multiple ports)");
                    Console.Write("port: ");
                    int resultat;
                    int.TryParse(Console.ReadLine(), out resultat);
                    Console.WriteLine("value set to " + resultat);
                    Program.chanel = resultat;
                    Console.WriteLine("\npress the midi keys you whant to use, and wen you're done press any key on your keybord\n");
                    Program.midi.Open(Program.chanel);
                    Program.midi.Start();
                    int old = 0;
                    int value = 0;
                    string valueHex = "";
                    string hex4 = "0000";
                    while (Program.runSetup)
                    {
                        if (Console.KeyAvailable)
                        {
                            Program.runSetup = false;
                        }
                        value = Program.midi.p[0];
                        if (old != value)
                        {
                            valueHex = Program.midi.pS;
                            if (hex4 != valueHex.Substring(valueHex.Length - 4))
                            {
                                if (hex4.Substring(hex4.Length - 2) != "D0")
                                {
                                    hex4 = valueHex.Substring(valueHex.Length - 4);

                                    int hex4Con = int.Parse(hex4, System.Globalization.NumberStyles.HexNumber);

                                    bool x2 = true;
                                    using (IEnumerator<int[]> enumerator = Program.values.GetEnumerator())
                                    {
                                        while (enumerator.MoveNext())
                                        {
                                            if (enumerator.Current[0] == hex4Con)
                                            {
                                                x2 = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (x2)
                                    {
                                        Program.values.Add(new int[]
                                        {
                                            hex4Con,
                                            0,
                                            Program.getTeken()
                                        });
                                    }
                                }

                            }

                            if (valueHex.Substring(valueHex.Length - 2) == "D0")
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
                    Program.midi.Stop();
                    using (StreamWriter file = new StreamWriter("data.txt"))
                    {
                        file.WriteLine(Program.chanel);
                        foreach (int[] line in Program.values)
                        {
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
                }
                if (Program.runPogram)
                {
                    if (Program.values.Count < 1)
                    {
                        Console.WriteLine("loading from file...");
                        string[] array = File.ReadAllLines("data.txt");
                        Program.chanel = int.Parse(array[0]);
                        foreach (string line2 in array)
                        {
                            if (line2 != string.Concat(Program.chanel))
                            {
                                int[] tempValues = new int[line2.Split(new char[]
                                {
                                    ','
                                }).Length];
                                for (int i = 0; i < tempValues.Length; i++)
                                {
                                    string thiss = line2.Split(new char[]
                                    {
                                        ','
                                    })[i];
                                    tempValues[i] = int.Parse(thiss);
                                    Console.Write(thiss + " ");
                                }
                                Program.values.Add(tempValues);
                                Console.Write("\n");
                            }
                        }
                    }

                    string[] valuesHex = new string[Program.values.Count];

                    for (int i = 0; i < Program.values.Count; i++)
                    {
                        valuesHex[i] = Program.values[i][0].ToString("X").PadLeft(4, '0');
                    }

                    int old2 = 0;
                    Program.midi.Open(Program.chanel);
                    Program.midi.Start();
                    InputSimulator IS = new InputSimulator();
                    string valueHex = "", valueHex4 = "0000";
                    Console.WriteLine("runing...   //press any key to stop");
                    while (Program.runPogram)
                    {
                        if (Console.KeyAvailable)
                        {
                            Program.runPogram = false;
                        }
                        int value = Program.midi.p[0];
                        if (old2 != value)
                        {

                            valueHex = Program.midi.pS;
                            valueHex4 = valueHex.Substring(valueHex.Length - 4);

                            if (valueHex.Substring(valueHex.Length - 2) != "D0")
                            {
                                int lenght_ = Program.values.Count;
                                for (int j = 0; j < lenght_; j++)
                                {
                                    if (valuesHex[j] == valueHex4)
                                    {
                                        if (Program.values[j][1] == 0) // key
                                        {

                                            StringBuilder sb = new StringBuilder();
                                            if (valueHex.Length > 4)
                                            {
                                                sb.Append("Key_Down '");
                                                IS.Keyboard.KeyDown((VirtualKeyCode)((ushort)Program.values[j][2]));
                                            }
                                            else
                                            {
                                                sb.Append("Key_Up '");
                                                IS.Keyboard.KeyUp((VirtualKeyCode)((ushort)Program.values[j][2]));
                                            }
                                            sb.Append(Program.values[j][2]);
                                            sb.Append("' on:");
                                            Console.WriteLine(sb);
                                            break;
                                        }
                                        else if (Program.values[j][1] == 1 && valueHex.Length > 4) // macro
                                        {
                                            StringBuilder sb = new StringBuilder("macro '");
                                            for (int k = 2; k < Program.values[j].Length; k++)
                                            {
                                                if (Program.values[j][k] > 0)
                                                {
                                                    IS.Keyboard.KeyDown((VirtualKeyCode)((ushort)Program.values[j][k]));
                                                    sb.Append("+" + Program.values[j][k] + "'");
                                                }
                                                else
                                                {
                                                    IS.Keyboard.KeyUp((VirtualKeyCode)((ushort)(-(ushort)Program.values[j][k])));
                                                    sb.Append(Program.values[j][k] + "'");
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
                    }
                    Program.midi.Stop();
                }
                if (Program.runViewer)
                {
                    Console.WriteLine("what midi port do you whant to use \n(they start at 0 and count up from that, remember that some devices use multiple ports)");
                    Console.Write("port: ");
                    int resultat2;
                    int.TryParse(Console.ReadLine(), out resultat2);
                    Console.WriteLine("value set to " + resultat2);
                    Program.chanel = resultat2;
                    Program.midi.Open(Program.chanel);
                    Program.midi.Start();
                    int old = 0;
                    int value = 0;
                    string valueHex = "";
                    string hex4 = "0000";
                    while (Program.runViewer)
                    {
                        if (Console.KeyAvailable)
                        {
                            Program.runViewer = false;
                        }
                        value = Program.midi.p[0];
                        if (old != value)
                        {
                            valueHex = Program.midi.pS;
                           
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
                    Program.midi.Stop();
                }
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" ___ __ __    ________  ______    ________      ___   ___   ______   __  __   ______      \n/__//_//_/\\  /_______/\\/_____/\\  /_______/\\    /___/\\/__/\\ /_____/\\ /_/\\/_/\\ /_____/\\     \n\\::\\| \\| \\ \\ \\__.::._\\/\\:::_ \\ \\ \\__.::._\\/    \\::.\\ \\\\ \\ \\\\::::_\\/_\\ \\ \\ \\ \\\\::::_\\/_    \n \\:.      \\ \\   \\::\\ \\  \\:\\ \\ \\ \\   \\::\\ \\      \\:: \\/_) \\ \\\\:\\/___/\\\\:\\_\\ \\ \\\\:\\/___/\\   \n  \\:.\\-/\\  \\ \\  _\\::\\ \\__\\:\\ \\ \\ \\  _\\::\\ \\__    \\:. __  ( ( \\::___\\/_\\::::_\\/ \\_::._\\:\\  \n   \\. \\  \\  \\ \\/__\\::\\__/\\\\:\\/.:| |/__\\::\\__/\\    \\: \\ )  \\ \\ \\:\\____/\\ \\::\\ \\   /____\\:\\ \n    \\__\\/ \\__\\/\\________\\/ \\____/_/\\________\\/     \\__\\/\\__\\/  \\_____\\/  \\__\\/   \\_____\\/ \n");
                Console.ResetColor();
                Console.WriteLine("1. setup\n2. run\n3. viewer\n4. quit");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        Program.runSetup = true;
                        break;
                    case ConsoleKey.D2:
                        Program.runPogram = true;
                        break;
                    case ConsoleKey.D3:
                        Program.runViewer = true;
                        break;
                    case ConsoleKey.D4:
                        Program.runForm = false;
                        break;
                }
            }
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002888 File Offset: 0x00000A88
        private static int getTeken()
        {
            Program.teken++;
            switch (Program.teken)
            {
                case 1:
                    return 9;
                case 2:
                    return 13;
                case 3:
                    return 16;
                case 4:
                    return 17;
                case 5:
                    return 32;
                case 6:
                    return 35;
                case 7:
                    return 36;
                case 8:
                    return 37;
                case 9:
                    return 38;
                case 10:
                    return 39;
                case 11:
                    return 40;
                case 12:
                    return 45;
                case 13:
                    return 46;
                case 14:
                    return 48;
                case 15:
                    return 49;
                case 16:
                    return 50;
                case 17:
                    return 51;
                case 18:
                    return 52;
                case 19:
                    return 53;
                case 20:
                    return 54;
                case 21:
                    return 55;
                case 22:
                    return 56;
                case 23:
                    return 57;
                case 24:
                    return 65;
                case 25:
                    return 66;
                case 26:
                    return 67;
                case 27:
                    return 68;
                case 28:
                    return 69;
                case 29:
                    return 70;
                case 30:
                    return 71;
                case 31:
                    return 72;
                case 32:
                    return 73;
                case 33:
                    return 74;
                case 34:
                    return 75;
                case 35:
                    return 76;
                case 36:
                    return 77;
                case 37:
                    return 78;
                case 38:
                    return 79;
                case 39:
                    return 80;
                case 40:
                    return 81;
                case 41:
                    return 82;
                case 42:
                    return 83;
                case 43:
                    return 84;
                case 44:
                    return 85;
                case 45:
                    return 86;
                case 46:
                    return 87;
                case 47:
                    return 88;
                case 48:
                    return 89;
                case 49:
                    return 90;
                case 50:
                    return 90;
                case 51:
                    return 96;
                case 52:
                    return 97;
                case 53:
                    return 98;
                case 54:
                    return 99;
                case 55:
                    return 100;
                case 56:
                    return 102;
                case 57:
                    return 103;
                case 58:
                    return 104;
                case 59:
                    return 105;
                case 61:
                    return 106;
                case 62:
                    return 107;
                case 63:
                    return 108;
                case 64:
                    return 109;
                case 65:
                    return 110;
                case 66:
                    return 111;
                case 67:
                    return 160;
                case 68:
                    return 161;
                case 69:
                    return 162;
                case 70:
                    return 163;
                case 71:
                    return 186;
                case 72:
                    return 187;
                case 73:
                    return 188;
                case 74:
                    return 189;
                case 75:
                    return 190;
                case 76:
                    return 191;
                case 77:
                    return 192;
                case 78:
                    return 219;
                case 79:
                    return 220;
                case 80:
                    return 221;
                case 81:
                    return 222;
                case 82:
                    return 223;
                case 83:
                    return 226;
            }
            return 0;
        }

        // Token: 0x04000001 RID: 1
        private static bool runForm = true;

        // Token: 0x04000002 RID: 2
        private static bool runSetup = false;

        // Token: 0x04000003 RID: 3
        private static bool runPogram = false;

        // Token: 0x04000004 RID: 4
        private static bool runViewer = false;

        // Token: 0x04000005 RID: 5
        private static int chanel = 0;

        // Token: 0x04000006 RID: 6
        private static IList<int[]> values = new List<int[]>();

        // Token: 0x04000007 RID: 7
        private static InputPort midi = new InputPort();

        // Token: 0x04000008 RID: 8
        private static int teken = 0;
    }
}
