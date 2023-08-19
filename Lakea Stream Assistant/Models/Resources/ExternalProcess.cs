using Lakea_Stream_Assistant.Enums;
using System.Diagnostics;

namespace Lakea_Stream_Assistant.Models.Resources
{
    public class ExternalProcess
    {
        private string name;
        ProcessStartInfo startInfo;
        Process process;

        public string Name { get { return name; } }

        public ExternalProcess(ConfigApplication application)
        {
            EnumConverter enums = new EnumConverter();
            name = application.Name;
            startInfo = new ProcessStartInfo(application.Path);
            startInfo.WindowStyle = enums.ConvertWindowStyleString(application.WindowStyle);
            process = new Process();
        }

        public void StartProcess()
        {
            process = Process.Start(startInfo);
        }

        public void EndProcess()
        {
            process.Kill();
        }
    }
}
