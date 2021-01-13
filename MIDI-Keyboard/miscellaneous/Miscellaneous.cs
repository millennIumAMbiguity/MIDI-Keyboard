namespace MIDIKeyboard.miscellaneous
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
    }
}
