using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Automation;

namespace _06.Debugging
{
    class Program
    {
        static void Main()
        {
            try
            {
                using (Process process = new Process())
                {
                    Console.WriteLine("Running process 'CrackMe.exe'...");
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = "CrackMe.exe";
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForInputIdle();

                    Thread.Sleep(500);
                    Console.WriteLine($"Generating a key...");
                    var key = GenerateKey();
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        throw new Exception("Error occured while generating key.");
                    }

                    Console.WriteLine($"Key '{key}' has been generated.");
                    Console.WriteLine($"Filling in textbox with key '{key}'...");

                    AutomationElement form = AutomationElement.FromHandle(process.MainWindowHandle);
                    var condition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit);
                    var element = form.FindFirst(TreeScope.Children, condition);

                    if (element != null && element.TryGetCurrentPattern(ValuePattern.Pattern, out var valuePattern))
                    {
                        if (((ValuePattern)valuePattern).Current.IsReadOnly)
                        {
                            throw new InvalidOperationException(
                                "Failed to fill in textbox! The control is read-only. Try to enter the key mannually and click 'Check'.");
                        }

                        ((ValuePattern)valuePattern).SetValue(key);
                        Console.WriteLine("Click 'Check' button to validate the key...");
                    }
                }

                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static string GenerateKey()
        {
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();

            if (networkInterface != null)
            {
                byte[] addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
                byte[] dateBytes = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());

                var length = Math.Min(addressBytes.Length, dateBytes.Length);

                var builder = new StringBuilder();
                for (int i = 0; i < length; i++)
                {
                    int keyChunk = (addressBytes[i] ^ dateBytes[i]) * 10;

                    builder.Append(keyChunk);
                    if (i != length - 1)
                    {
                        builder.Append("-");
                    }
                }

                return builder.ToString();
            }

            return string.Empty;
        }
    }
}
