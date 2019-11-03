using System;
using System.Runtime.InteropServices;
using PowerStateManagement.Enum;

namespace PowerStateManagement
{
    public static class PowerManagementFunctionsWrapper
    {
        [DllImport("powrprof.dll", SetLastError = true)]
        public static extern uint CallNtPowerInformation(
            int informationLevel,
            IntPtr lpInputBuffer,
            uint nInputBufferSize,
            IntPtr lpOutputBuffer,
            uint nOutputBufferSize
        );

        [DllImport("powrprof.dll", SetLastError = true)]
        public static extern uint SetSuspendState(
            bool hibernate,
            bool forceCritical,
            bool disableWakeEvent);

        [DllImport("powrprof.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool GetPwrCapabilities(out SystemPowerCapabilities systemPowerCapabilites);

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemBatteryState
        {
            [MarshalAs(UnmanagedType.I1)]
            public bool AcOnLine;
            [MarshalAs(UnmanagedType.I1)]
            public bool BatteryPresent;
            [MarshalAs(UnmanagedType.I1)]
            public bool Charging;
            [MarshalAs(UnmanagedType.I1)]
            public bool Discharging;
            public byte Spare1;
            public byte Spare2;
            public byte Spare3;
            public byte Spare4;
            public uint MaxCapacity;
            public uint RemainingCapacity;
            public uint Rate;
            public uint EstimatedTime;
            public uint DefaultAlert1;
            public uint DefaultAlert2;

            public override string ToString()
            {
                return $"BatteryPresent: {BatteryPresent},\nCharging: {Charging},\nMaxCapacity: {MaxCapacity},\nRemainingCapacity: {RemainingCapacity}";
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemPowerInformation
        {
            public uint MaxIdlenessAllowed;
            public uint Idleness;
            public uint TimeRemaining;
            public byte CoolingMode;

            public override string ToString()
            {
                return $"MaxIdlenessAllowed: {MaxIdlenessAllowed},\nIdleness: {Idleness},\nTimeRemaining: {TimeRemaining},\nCoolingMode: {CoolingMode}";
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemPowerCapabilities
        {
            [MarshalAs(UnmanagedType.U1)]
            public bool PowerButtonPresent;
            [MarshalAs(UnmanagedType.U1)]
            public bool SleepButtonPresent;
            [MarshalAs(UnmanagedType.U1)]
            public bool LidPresent;
            [MarshalAs(UnmanagedType.U1)]
            public bool SystemS1;
            [MarshalAs(UnmanagedType.U1)]
            public bool SystemS2;
            [MarshalAs(UnmanagedType.U1)]
            public bool SystemS3;
            [MarshalAs(UnmanagedType.U1)]
            public bool SystemS4;
            [MarshalAs(UnmanagedType.U1)]
            public bool SystemS5;
            [MarshalAs(UnmanagedType.U1)]
            public bool HiberFilePresent;
            [MarshalAs(UnmanagedType.U1)]
            public bool FullWake;
            [MarshalAs(UnmanagedType.U1)]
            public bool VideoDimPresent;
            [MarshalAs(UnmanagedType.U1)]
            public bool ApmPresent;
            [MarshalAs(UnmanagedType.U1)]
            public bool UpsPresent;
            [MarshalAs(UnmanagedType.U1)]
            public bool ThermalControl;
            [MarshalAs(UnmanagedType.U1)]
            public bool ProcessorThrottle;
            public byte ProcessorMinThrottle;
            public byte ProcessorMaxThrottle;    // Also known as ProcessorThrottleScale before Windows XP
            [MarshalAs(UnmanagedType.U1)]
            public bool FastSystemS4;   // Ignore if earlier than Windows XP
            [MarshalAs(UnmanagedType.U1)]
            public bool Hiberboot;  // Ignore if earlier than Windows XP
            [MarshalAs(UnmanagedType.U1)]
            public bool WakeAlarmPresent;   // Ignore if earlier than Windows XP
            [MarshalAs(UnmanagedType.U1)]
            public bool AoAc;   // Ignore if earlier than Windows XP
            [MarshalAs(UnmanagedType.U1)]
            public bool DiskSpinDown;
            public byte HiberFileType;  // Ignore if earlier than Windows 10 (10.0.10240.0)
            [MarshalAs(UnmanagedType.U1)]
            public bool AoAcConnectivitySupported;  // Ignore if earlier than Windows 10 (10.0.10240.0)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            private readonly byte[] spare3;
            [MarshalAs(UnmanagedType.U1)]
            public bool SystemBatteriesPresent;
            [MarshalAs(UnmanagedType.U1)]
            public bool BatteriesAreShortTerm;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public BatteryReportingScale[] BatteryScale;
            public SystemPowerState AcOnLineWake;
            public SystemPowerState SoftLidWake;
            public SystemPowerState RtcWake;
            public SystemPowerState MinDeviceWakeState;
            public SystemPowerState DefaultLowLatencyWake;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BatteryReportingScale
        {
            public uint Granularity;
            public uint Capacity;
        }
    }
}
