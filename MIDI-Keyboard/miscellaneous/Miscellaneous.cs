using System;

namespace MIDIKeyboard.Miscellaneous
{
    static class Miscellaneous
    {
        static public int GetID(char c)
        {
            int cId = c;
            if (c > 96 && c < 123)
                cId -= 32;

            return cId;
        }

        public static void Error(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
            Console.WriteLine("Press any key to ignore and continue...");
            Console.ReadKey();
        }
        public static void Error(string s, object[] os)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s, os);
            Console.ResetColor();
            Console.WriteLine("Press any key to ignore and continue...");
            Console.ReadKey();
        }
        public static void Error(string s, object o1) => Error(s, new object[] { o1 });
        public static void Error(string s, object o1, object o2) => Error(s, new object[] { o1, o2 });
        public static void Error(string s, object o1, object o2, object o3) => Error(s, new object[] { o1, o2, o3 });
        public static void Error(string s, object o1, object o2, object o3, object o4) => Error(s, new object[] { o1, o2, o3, o4 });
    }
}
