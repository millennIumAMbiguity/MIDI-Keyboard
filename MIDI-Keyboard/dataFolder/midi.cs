using System;
using System.Runtime.InteropServices;

namespace MIDIKeyboard.dataFolder
{
	public class InputPort
	{
		private readonly NativeMethods.MidiInProc  midiInProc;
		private readonly NativeMethods.MidiOutProc midiOutProc;
		private          IntPtr                    handle, handleOut;

		public int    p;
		public string pS = "";

		public InputPort()
		{
			midiInProc = MidiProc;
			handle     = IntPtr.Zero;

			midiOutProc = MidiProc;
			handleOut   = IntPtr.Zero;
		}


		public int InputCount() => NativeMethods.midiInGetNumDevs();


		public bool Close()
		{
			bool result = NativeMethods.midiInClose(handle)
			           == NativeMethods.MMSYSERR_NOERROR;
			handle = IntPtr.Zero;
			return result;
		}

		public bool Open(int id) =>
			NativeMethods.midiInOpen(
				out handle,
				id,
				midiInProc,
				IntPtr.Zero,
				NativeMethods.CALLBACK_FUNCTION)
		 == NativeMethods.MMSYSERR_NOERROR;

		public bool Start() =>
			NativeMethods.midiInStart(handle)
		 == NativeMethods.MMSYSERR_NOERROR;

		public bool Stop() =>
			NativeMethods.midiInStop(handle)
		 == NativeMethods.MMSYSERR_NOERROR;

		private void MidiProc(
			IntPtr hMidiIn,
			int    wMsg,
			IntPtr dwInstance,
			int    dwParam1,
			int    dwParam2)
		{
			pS = dwParam1.ToString("X").PadLeft(4, '0'); // Gives you hexadecimal
			p  = dwParam1;

			Program.waitHandle.Set();

			// Receive messages here
		}


		public bool MidiOutMsg(byte pitch, byte velocity)
		{
			//hmidi is an IntPtr obtained via midiOutOpen or other means.

			byte[] data = new byte[4];
			data[0] = 0x90;     //note on, channel 0
			data[1] = pitch;    //pitch
			data[2] = velocity; //velocity
			int msg = BitConverter.ToInt32(data, 0);

			return NativeMethods.midiOutShortMsg(handleOut, msg)
			    == NativeMethods.MMSYSERR_NOERROR;
		}

		public int OutputCount() => NativeMethods.midiOutGetNumDevs();

		public bool CloseOut()
		{
			bool result = NativeMethods.midiOutClose(handleOut)
			           == NativeMethods.MMSYSERR_NOERROR;
			handleOut = IntPtr.Zero;
			return result;
		}

		public bool OpenOut(int id) =>
			NativeMethods.midiOutOpen(
				out handleOut,
				id,
				midiOutProc,
				IntPtr.Zero,
				NativeMethods.CALLBACK_FUNCTION)
		 == NativeMethods.MMSYSERR_NOERROR;

		public bool MidiOutReset() =>
			NativeMethods.MidiOutReset(handleOut)
		 == NativeMethods.MMSYSERR_NOERROR;
	}

	#pragma warning disable IDE1006 // Naming Styles
	internal static class NativeMethods
	{
		internal const int MMSYSERR_NOERROR  = 0;
		internal const int CALLBACK_FUNCTION = 0x00030000;

		public static string midiInGetDevCaps(IntPtr uDeviceID)
		{
			midiInGetDevCaps(
				uDeviceID, out MIDIINCAPS caps,
				(uint) Marshal.SizeOf(typeof(MIDIINCAPS)));
			return caps.szPname;
		}

		public static string midiOutGetDevCaps(IntPtr uDeviceID)
		{
			midiOutGetDevCaps(
				uDeviceID, out MIDIINCAPS caps,
				(uint) Marshal.SizeOf(typeof(MIDIINCAPS)));
			return caps.szPname;
		}

		[DllImport("winmm.dll")] internal static extern int midiInGetNumDevs();

		[DllImport("winmm.dll", SetLastError = true)]
		internal static extern int midiInGetDevCaps(
			IntPtr uDeviceID, out MIDIINCAPS caps,
			uint   cbMidiInCaps);

		[DllImport("winmm.dll", SetLastError = true)]
		internal static extern int midiOutGetDevCaps(
			IntPtr uDeviceID, out MIDIINCAPS caps,
			uint   cbMidiInCaps);

		[DllImport("winmm.dll")]
		internal static extern int midiInClose(
			IntPtr hMidiIn);

		[DllImport("winmm.dll")]
		internal static extern int midiInOpen(
			out IntPtr lphMidiIn,
			int        uDeviceID,
			MidiInProc dwCallback,
			IntPtr     dwCallbackInstance,
			int        dwFlags);

		[DllImport("winmm.dll")]
		internal static extern int midiInStart(
			IntPtr hMidiIn);

		[DllImport("winmm.dll")]
		internal static extern int midiInStop(
			IntPtr hMidiIn);

		[DllImport("winmm.dll")]
		internal static extern int midiOutShortMsg(
			IntPtr hMidiOut,
			int    dwMsg
		);

		[DllImport("winmm.dll")] internal static extern int midiOutGetNumDevs();

		[DllImport("winmm.dll")]
		internal static extern int midiOutOpen(
			out IntPtr  lphMidiIn,
			int         uDeviceID,
			MidiOutProc dwCallback,
			IntPtr      dwCallbackInstance,
			int         dwFlags);


		[DllImport("winmm.dll")]
		internal static extern int midiOutClose(
			IntPtr hMidiIn);

		[DllImport("winmm.dll")]
		internal static extern int MidiOutReset(
			IntPtr hMidiIn);

		[StructLayout(LayoutKind.Sequential)]
		[Serializable]
		public struct MIDIINCAPS
		{
			public ushort wMid;
			public ushort wPid;
			public uint   vDriverVersion;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string szPname;

			public uint dwSupport;
		}

		internal delegate void MidiInProc(
			IntPtr hMidiIn,
			int    wMsg,
			IntPtr dwInstance,
			int    dwParam1,
			int    dwParam2);


		internal delegate void MidiOutProc(
			IntPtr hMidiIn,
			int    wMsg,
			IntPtr dwInstance,
			int    dwParam1,
			int    dwParam2);
	}
}