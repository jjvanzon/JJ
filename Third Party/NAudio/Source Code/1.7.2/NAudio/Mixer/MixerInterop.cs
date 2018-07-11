// created on 09/12/2002 at 21:03
using System;
using System.Runtime.InteropServices;
using NAudio.Wave;

// TODO: add function help from MSDN
// TODO: Create enums for flags parameters
namespace NAudio.Mixer
{
    class MixerInterop
    {
        public const uint MIXERCONTROL_CONTROLF_UNIFORM = 0x00000001;
        public const uint MIXERCONTROL_CONTROLF_MULTIPLE = 0x00000002;
        public const uint MIXERCONTROL_CONTROLF_DISABLED = 0x80000000;

        public const int MAXPNAMELEN = 32;
        public const int MIXER_SHORT_NAME_CHARS = 16;
        public const int MIXER_LONG_NAME_CHARS = 64;

        // http://msdn.microsoft.com/en-us/library/dd757304%28VS.85%29.aspx
        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int mixerGetNumDevs();

        // http://msdn.microsoft.com/en-us/library/dd757308%28VS.85%29.aspx
        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern MmResult mixerOpen(out IntPtr hMixer, int uMxId, IntPtr dwCallback, IntPtr dwInstance, MixerFlags dwOpenFlags);

        // http://msdn.microsoft.com/en-us/library/dd757292%28VS.85%29.aspx
        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern MmResult mixerClose(IntPtr hMixer);

        // http://msdn.microsoft.com/en-us/library/dd757299%28VS.85%29.aspx
        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern MmResult mixerGetControlDetails(IntPtr hMixer, ref MIXERCONTROLDETAILS mixerControlDetails, MixerFlags dwDetailsFlags);

        // http://msdn.microsoft.com/en-us/library/dd757300%28VS.85%29.aspx
        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern MmResult mixerGetDevCaps(IntPtr nMixerID, ref MIXERCAPS mixerCaps, int mixerCapsSize);

        // http://msdn.microsoft.com/en-us/library/dd757301%28VS.85%29.aspx
        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern MmResult mixerGetID(IntPtr hMixer, out int mixerID, MixerFlags dwMixerIDFlags);

        // http://msdn.microsoft.com/en-us/library/dd757302%28VS.85%29.aspx
        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern MmResult mixerGetLineControls(IntPtr hMixer, ref MIXERLINECONTROLS mixerLineControls, MixerFlags dwControlFlags);

        // http://msdn.microsoft.com/en-us/library/dd757303%28VS.85%29.aspx
        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern MmResult mixerGetLineInfo(IntPtr hMixer, ref MIXERLINE mixerLine, MixerFlags dwInfoFlags);

        // http://msdn.microsoft.com/en-us/library/dd757307%28VS.85%29.aspx
        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern MmResult mixerMessage(IntPtr hMixer, uint nMessage, IntPtr dwParam1, IntPtr dwParam2);

        // http://msdn.microsoft.com/en-us/library/dd757309%28VS.85%29.aspx
        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern MmResult mixerSetControlDetails(IntPtr hMixer, ref MIXERCONTROLDETAILS mixerControlDetails, MixerFlags dwDetailsFlags);

        // http://msdn.microsoft.com/en-us/library/dd757294%28VS.85%29.aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        public struct MIXERCONTROLDETAILS
        {
            public int cbStruct; // size of the MIXERCONTROLDETAILS structure
            public int dwControlID;
            public int cChannels; // Number of channels on which to get or set control properties
            public IntPtr hwndOwner; // Union with DWORD cMultipleItems
            public int cbDetails; // Size of the paDetails Member
            public IntPtr paDetails; // LPVOID
        }

        // http://msdn.microsoft.com/en-us/library/dd757291%28VS.85%29.aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct MIXERCAPS
        {
            public ushort wMid;
            public ushort wPid;
            public uint vDriverVersion; // MMVERSION - major high byte, minor low byte
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
            public string szPname;
            public uint fdwSupport;
            public uint cDestinations;
        }

        // http://msdn.microsoft.com/en-us/library/dd757306%28VS.85%29.aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct MIXERLINECONTROLS
        {
            public int cbStruct; // size of the MIXERLINECONTROLS structure
            public int dwLineID; // Line identifier for which controls are being queried
            public int dwControlID; // union with UInt32 dwControlType
            public int cControls;
            public int cbmxctrl;
            public IntPtr pamxctrl; // see MSDN "Structs Sample"
        }

        /// <summary>
        /// Mixer Line Flags
        /// </summary>
        [Flags]
        public enum MIXERLINE_LINEF
        {
            /// <summary>
            /// Audio line is active. An active line indicates that a signal is probably passing 
            /// through the line.
            /// </summary>
            MIXERLINE_LINEF_ACTIVE = 1,

            /// <summary>
            /// Audio line is disconnected. A disconnected line's associated controls can still be 
            /// modified, but the changes have no effect until the line is connected.
            /// </summary>
            MIXERLINE_LINEF_DISCONNECTED = 0x8000,

            /// <summary>
            /// Audio line is an audio source line associated with a single audio destination line. 
            /// If this flag is not set, this line is an audio destination line associated with zero 
            /// or more audio source lines.
            /// </summary>
            MIXERLINE_LINEF_SOURCE = (unchecked((int)0x80000000))
        }

        // http://msdn.microsoft.com/en-us/library/dd757305%28VS.85%29.aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct MIXERLINE
        {
            public int cbStruct;
            public int dwDestination;
            public int dwSource;
            public int dwLineID;
            public MIXERLINE_LINEF fdwLine;
            public IntPtr dwUser;
            public MixerLineComponentType dwComponentType;
            public int cChannels;
            public int cConnections;
            public int cControls;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_SHORT_NAME_CHARS)]
            public string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_LONG_NAME_CHARS)]
            public string szName;
            // start of target struct 'Target'
            public uint dwType;
            public uint dwDeviceID;
            public ushort wMid;
            public ushort wPid;
            public uint vDriverVersion; // MMVERSION - major high byte, minor low byte
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
            public string szPname;
            // end of target struct
        }

        /// <summary>
        /// BOUNDS structure
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct Bounds
        {
            /// <summary>
            /// dwMinimum / lMinimum / reserved 0
            /// </summary>
            public int minimum;
            /// <summary>
            /// dwMaximum / lMaximum / reserved 1
            /// </summary>
            public int maximum;
            /// <summary>
            /// reserved 2
            /// </summary>
            public int reserved2;
            /// <summary>
            /// reserved 3
            /// </summary>
            public int reserved3;
            /// <summary>
            /// reserved 4
            /// </summary>
            public int reserved4;
            /// <summary>
            /// reserved 5
            /// </summary>
            public int reserved5;
        }

        /// <summary>
        /// METRICS structure
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct Metrics
        {
            /// <summary>
            /// cSteps / reserved[0]
            /// </summary>
            public int step;
            /// <summary>
            /// cbCustomData / reserved[1], number of bytes for control details
            /// </summary>
            public int customData;
            /// <summary>
            /// reserved 2
            /// </summary>
            public int reserved2;
            /// <summary>
            /// reserved 3
            /// </summary>
            public int reserved3;
            /// <summary>
            /// reserved 4
            /// </summary>
            public int reserved4;
            /// <summary>
            /// reserved 5
            /// </summary>
            public int reserved5;
        }

        /// <summary>
        /// MIXERCONTROL struct
        /// http://msdn.microsoft.com/en-us/library/dd757293%28VS.85%29.aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct MIXERCONTROL
        {
            public uint cbStruct;
            public int dwControlID;
            public MixerControlType dwControlType;
            public uint fdwControl;
            public uint cMultipleItems;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_SHORT_NAME_CHARS)]
            public string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_LONG_NAME_CHARS)]
            public string szName;
            public Bounds Bounds;
            public Metrics Metrics;
        }

        // http://msdn.microsoft.com/en-us/library/dd757295%28VS.85%29.aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct MIXERCONTROLDETAILS_BOOLEAN
        {
            public int fValue;
        }

        // http://msdn.microsoft.com/en-us/library/dd757297%28VS.85%29.aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct MIXERCONTROLDETAILS_SIGNED
        {
            public int lValue;
        }

        // http://msdn.microsoft.com/en-us/library/dd757296%28VS.85%29.aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct MIXERCONTROLDETAILS_LISTTEXT
        {
            public uint dwParam1;
            public uint dwParam2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_LONG_NAME_CHARS)]
            public string szName;
        }

        // http://msdn.microsoft.com/en-us/library/dd757298%28VS.85%29.aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct MIXERCONTROLDETAILS_UNSIGNED
        {
            public uint dwValue;
        }
    }
}
