using System;
using System.IO;
using System.Text;
using keyBordL.dataFolder;
using WindowsInput;
using WindowsInput.Native;

namespace keyBordL
{
    class Run
    {

        private static InputPort midi = new InputPort();

        public static bool Run_(bool runPogram)
        {

            int chanel = 0;

            if (Program.values.Count < 1)
            {
                Console.WriteLine("loading from file...");
                string[] array = File.ReadAllLines("data.txt");
                chanel = int.Parse(array[0]);
                foreach (string line2 in array)
                {
                    if (line2 != string.Concat(chanel))
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
            midi.Open(chanel);
            midi.Start();
            InputSimulator IS = new InputSimulator();
            string valueHex = "", valueHex4 = "0000";
            Console.WriteLine("runing...   //press any key to stop");
            while (runPogram)
            {
                if (Console.KeyAvailable)
                {
                    runPogram = false;
                }
                int value = midi.p;
                if (old2 != value)
                {

                    valueHex = midi.pS;
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
            midi.Stop();
            midi.Close();
            Program.teken = 0;
            return runPogram;
        }

    }
}
