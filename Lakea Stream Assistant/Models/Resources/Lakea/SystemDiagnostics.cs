using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Singletons;
using System.Diagnostics;

namespace Lakea_Stream_Assistant.Models.Resources.Lakea
{
    public class SystemDiagnostics
    {
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;

        public SystemDiagnostics()
        {
            try
            {
                //cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                //ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            }
            catch (Exception ex)
            {
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        public int GetCurrentCPUUsage()
        {
            return int.MinValue;
            //return (int)cpuCounter.NextValue();
        }

        public int GetCurrentRamAvaliable()
        {
            return int.MinValue;
            //return (int)ramCounter.NextValue();
        }
    }
}
