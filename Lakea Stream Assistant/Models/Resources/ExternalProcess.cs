using Lakea_Stream_Assistant.Enums;
using System.Diagnostics;

namespace Lakea_Stream_Assistant.Models.Resources
{
    //This class stores information about a external application and controls it running
    public class ExternalProcess
    {
        private string name;
        private bool active;
        ProcessStartInfo startInfo;
        Process process;

        public string Name { get { return name; } }
        public bool Active { get { return active; } }

        //Constructor sets the information needed to start the external application
        public ExternalProcess(ConfigApplication application)
        {
            EnumConverter enums = new EnumConverter();
            name = application.Name;
            active = false;
            startInfo = new ProcessStartInfo(application.Path);
            startInfo.WindowStyle = enums.ConvertWindowStyleString(application.WindowStyle);
            process = new Process();
        }

        //Starts the external application
        public void StartProcess()
        {
            if (!active)
            {
                process = Process.Start(startInfo);
                active = true;
            }
        }

        //Stops the external application
        public void EndProcess()
        {
            if (active)
            {
                process.Kill();
                active = false;
            }
        }
    }
}
