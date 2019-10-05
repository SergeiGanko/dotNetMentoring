using AsyncAwait.CodeReviewChallenge.Models.Help;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AsyncAwait.Task2.CodeReviewChallenge.Tests
{
    [TestClass]
    public class ManualAssistentTests
    {
        [TestMethod]
        public void TestException()
        {
            var manualAssistent = new ManualAssistent();
            Assert.AreEqual(true, IsAllowedMessage(manualAssistent.RequestAssistanceAsync("User").Result));
        }

        #region private methods 

        private static bool IsAllowedMessage(string message) {
             List<string> allowedMessages = new List<string> {
                "Dear user, the assistance request is registered.",
                "Failed to register assistance request. Please try later. "
            };
                        
            return allowedMessages.Any(m => message.Contains(message));
        }

        #endregion
    }
}