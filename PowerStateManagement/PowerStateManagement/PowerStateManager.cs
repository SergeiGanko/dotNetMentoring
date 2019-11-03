using System;
using System.Runtime.InteropServices;
using PowerStateManagement.Enum;
using static PowerStateManagement.PowerManagementFunctionsWrapper;

namespace PowerStateManagement
{
    public class PowerStateManager : IPowerStateManager
    {
        public long GetLastSleepTime()
        {
            IntPtr ptr = CallNtPowerInfo<long>(PowerInformationLevel.LastSleepTime);
            return Marshal.ReadInt64(ptr, 0) / 10000000;
        }

        public long GetLastWakeTime()
        {
            IntPtr ptr = CallNtPowerInfo<long>(PowerInformationLevel.LastWakeTime);
            return Marshal.ReadInt64(ptr, 0) / 10000000;
        }

        public SystemBatteryState GetSystemBatteryState()
        {
            IntPtr batteryStatePtr = CallNtPowerInfo<SystemBatteryState>(PowerInformationLevel.SystemBatteryState);
            return Marshal.PtrToStructure<SystemBatteryState>(batteryStatePtr);
        }

        public SystemPowerInformation GetSystemPowerInfo()
        {
            IntPtr sysPowerInfoPtr = CallNtPowerInfo<SystemPowerInformation>(PowerInformationLevel.SystemPowerInformation);
            return Marshal.PtrToStructure<SystemPowerInformation>(sysPowerInfoPtr);
        }

        private IntPtr CallNtPowerInfo<T>(PowerInformationLevel level) where T : struct
        {
            int size = Marshal.SizeOf<T>();
            IntPtr lpOutputBuffer = Marshal.AllocHGlobal(size);
            try
            {
                uint status = CallNtPowerInformation((int)level, IntPtr.Zero, 0, lpOutputBuffer, (uint)size);

                return status == 0 ? lpOutputBuffer : IntPtr.Zero;
            }
            finally
            {
                if (lpOutputBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(lpOutputBuffer);
                }
            }
        }

        public bool ReserveOrRemoveHiberFile(HiberFileState state)
        {
            int size = Marshal.SizeOf<int>();
            IntPtr pBool = Marshal.AllocHGlobal(size);

            if (state == HiberFileState.ReserveHiberFile)
            {
                Marshal.WriteInt32(pBool, 0, 1);  // last parameter is 1 (True)
            }
            else if(state == HiberFileState.RemoveHiberFile)
            {
                Marshal.WriteInt32(pBool, 0, 0);  // last parameter is 0 (False)
            }

            try
            {
                CallNtPowerInformation((int) PowerInformationLevel.SystemReserveHiberFile, pBool,
                    sizeof(bool), IntPtr.Zero, 0);

                GetPwrCapabilities(out var systemPowerCapabilites);

                return systemPowerCapabilites.HiberFilePresent;
            }
            finally
            {
                if (pBool != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pBool);
                }
            }
        }

        public void SetHibernate()
        {
            SetSuspendState(true, true, false);
        }

        public void SetSuspend()
        {
            SetSuspendState(false, true, false);
        }
    }
}