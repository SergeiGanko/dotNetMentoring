using System.Runtime.InteropServices;
using PowerStateManagement.Enum;
using static PowerStateManagement.PowerManagementFunctionsWrapper;

namespace PowerStateManagement
{
    [ComVisible(true)]
    [Guid("DE999126-0550-46ED-988F-36D03FCEAD42")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPowerStateManager
    {
        long GetLastSleepTime();
        long GetLastWakeTime();
        SystemBatteryState GetSystemBatteryState();
        SystemPowerInformation GetSystemPowerInfo();
        bool ReserveOrRemoveHiberFile(HiberFileState state);
        void SetHibernate();
        void SetSuspend();
    }
}