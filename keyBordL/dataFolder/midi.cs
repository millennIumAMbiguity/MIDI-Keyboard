using System;
using System.Runtime.InteropServices;

namespace keyBordL.dataFolder
{
    public class InputPort
    {
        private NativeMethods.MidiInProc midiInProc;
        private IntPtr handle;

        public int p = 0;
        public string pS = "";

        public InputPort()
        {
            midiInProc = new NativeMethods.MidiInProc(MidiProc);
            handle = IntPtr.Zero;
        }

        public static int InputCount
        {
            get { return NativeMethods.midiInGetNumDevs(); }
        }

        public bool Close()
        {
            bool result = NativeMethods.midiInClose(handle)
                == NativeMethods.MMSYSERR_NOERROR;
            handle = IntPtr.Zero;
            return result;
        }

        public bool Open(int id)
        {
            return NativeMethods.midiInOpen(
                out handle,
                id,
                midiInProc,
                IntPtr.Zero,
                NativeMethods.CALLBACK_FUNCTION)
                    == NativeMethods.MMSYSERR_NOERROR;
        }

        public bool Start()
        {
            return NativeMethods.midiInStart(handle)
                == NativeMethods.MMSYSERR_NOERROR;
        }

        public bool Stop()
        {
            return NativeMethods.midiInStop(handle)
                == NativeMethods.MMSYSERR_NOERROR;
        }

        private void MidiProc(
            IntPtr hMidiIn,
            int wMsg,
            IntPtr dwInstance,
            int dwParam1,
            int dwParam2)
        {

            pS = dwParam1.ToString("X").PadLeft(4, '0');  // Gives you hexadecimal
            p = dwParam1;

            // Receive messages here
        }

    }

    internal static class NativeMethods
    {
        internal const int MMSYSERR_NOERROR = 0;
        internal const int CALLBACK_FUNCTION = 0x00030000;

        internal delegate void MidiInProc(
            IntPtr hMidiIn,
            int wMsg,
            IntPtr dwInstance,
            int dwParam1,
            int dwParam2);

        [DllImport("winmm.dll")]
        internal static extern int midiInGetNumDevs();

        [DllImport("winmm.dll")]
        internal static extern int midiInClose(
            IntPtr hMidiIn);

        [DllImport("winmm.dll")]
        internal static extern int midiInOpen(
            out IntPtr lphMidiIn,
            int uDeviceID,
            MidiInProc dwCallback,
            IntPtr dwCallbackInstance,
            int dwFlags);

        [DllImport("winmm.dll")]
        internal static extern int midiInStart(
            IntPtr hMidiIn);

        [DllImport("winmm.dll")]
        internal static extern int midiInStop(
            IntPtr hMidiIn);
    }
}