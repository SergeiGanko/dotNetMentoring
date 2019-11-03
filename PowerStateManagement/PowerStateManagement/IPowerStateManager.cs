using PowerStateManagement.Enum;
using static PowerStateManagement.PowerManagementFunctionsWrapper;

namespace PowerStateManagement
{
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