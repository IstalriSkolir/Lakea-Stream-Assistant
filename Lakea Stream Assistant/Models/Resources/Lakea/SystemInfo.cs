using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Lakea_Stream_Assistant.Models.Resources.Lakea
{
    public class SystemInfo
    {
        private string processorName;
        private string processorSpeed;
        private int processorCount;
        private int systemMemory;

        [DllImport("kernel32.dll")] //https://www.geoffchappell.com/studies/windows/win32/kernel32/api/index.htm
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);

        public SystemInfo()
        {
            RegistryKey processorRegistry = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);
            processorName = processorRegistry.GetValue("ProcessorNameString").ToString().Trim();
            processorSpeed = processorRegistry.GetValue("~MHZ").ToString() + "MHz";
            processorCount = Environment.ProcessorCount;
            long memoryKb;
            GetPhysicallyInstalledSystemMemory(out memoryKb);
            systemMemory = (int)memoryKb / 1024;
        }

        public string ProcessorName { get { return processorName; } }
        public string ProcessorSpeed { get { return processorSpeed; } }
        public int ProcessorCount { get { return processorCount; } }
        public int SystemMemory { get { return systemMemory; } }
    }
}
