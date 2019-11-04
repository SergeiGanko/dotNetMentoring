using System;
using NUnit.Framework;
using PowerStateManagement.Enum;

namespace PowerStateManagement.Tests
{
    public class PowerStateManagerTests
    {
        private IPowerStateManager _powerStateManager;

        [SetUp]
        public void Setup()
        {
            _powerStateManager = new PowerStateManager();
        }

        [Test]
        public void GetLastSleepTime_ReturnsNotZero()
        {
            var actual = _powerStateManager.GetLastSleepTime();

            Console.WriteLine($"Last Sleep Time: {actual}");
            Assert.NotZero(actual);
        }

        [Test]
        public void GetLastWakeTime_ReturnsNotZero()
        {
            var actual = _powerStateManager.GetLastWakeTime();

            Console.WriteLine($"Last Wake Time: {actual}");
            Assert.NotZero(actual);
        }

        [Test]
        public void GetSystemBatteryStateTest()
        {
            var batteryState = _powerStateManager.GetSystemBatteryState();
            
            Assert.NotNull(batteryState);
            Console.WriteLine($"System Battery State:\n{batteryState}");

        }

        [Test]
        public void GetSystemPowerInfoTest()
        {
            var sysPowerInfo = _powerStateManager.GetSystemPowerInfo();
            Assert.NotZero(sysPowerInfo.Idleness);
            Assert.NotZero(sysPowerInfo.TimeRemaining);
            Console.WriteLine($"System Power Info:\n{sysPowerInfo}");
        }

        [Test]
        public void ReserveHiberFileTest()
        {
            var isHiberFilePresent = _powerStateManager.ReserveOrRemoveHiberFile(HiberFileState.ReserveHiberFile);
            
            Console.WriteLine($"ReserveHiberFile result (HiberFilePresent): {isHiberFilePresent}");
            Assert.IsTrue(isHiberFilePresent);
        }

        [Test]
        public void RemoveHiberFileTest()
        {
            var isHiberFilePresent = _powerStateManager.ReserveOrRemoveHiberFile(HiberFileState.RemoveHiberFile);
            
            Console.WriteLine($"RemoveHiberFile result (HiberFilePresent): {isHiberFilePresent}");
            Assert.IsFalse(isHiberFilePresent);
        }

        [Test]
        [Ignore("Ignore a test to prevent shutting down the machine")]
        public void SetHibernateTest()
        {
            _powerStateManager.SetHibernate();
        }

        [Test]
        [Ignore("Ignore a test to prevent shutting down the machine")]
        public void SetSuspendTest()
        {
            _powerStateManager.SetSuspend();
        }
    }
}