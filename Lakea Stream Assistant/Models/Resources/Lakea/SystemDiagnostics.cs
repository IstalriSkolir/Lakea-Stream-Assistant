using System.Diagnostics;

namespace Lakea_Stream_Assistant.Models.Resources.Lakea
{
    public class SystemDiagnostics
    {
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;

        public SystemDiagnostics()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public int GetCurrentCPUUsage()
        {
            return (int)cpuCounter.NextValue(); ;
        }

        public int GetCurrentRamAvaliable()
        {
            return (int)ramCounter.NextValue();
        }
    }
}
