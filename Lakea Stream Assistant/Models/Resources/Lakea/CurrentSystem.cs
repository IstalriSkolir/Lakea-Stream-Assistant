namespace Lakea_Stream_Assistant.Models.Resources.Lakea
{
    public class CurrentSystem
    {
        private SystemInfo info;
        private SystemDiagnostics diagnostics;

        public CurrentSystem()
        {
            info = new SystemInfo();
            diagnostics = new SystemDiagnostics();
        }

        public string ProcessorName { get { return info.ProcessorName; } }
        public string ProcessorSpeed { get { return info.ProcessorSpeed; } }
        public int ProcessorCount { get { return info.ProcessorCount; } }
        public int SystemMemory { get { return info.SystemMemory; } }

        public string GetCPUUsage()
        {
            int cpu = diagnostics.GetCurrentCPUUsage();
            return cpu + "%";
        }

        public string GetRAMUsage()
        {
            int ramAvaliable = diagnostics.GetCurrentRamAvaliable();
            int ramUsage = info.SystemMemory - ramAvaliable;
            return ramAvaliable + "MB";
        }
    }
}
