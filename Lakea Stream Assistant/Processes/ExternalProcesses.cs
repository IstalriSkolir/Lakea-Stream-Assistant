using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Resources;
using Lakea_Stream_Assistant.Singletons;
using System.Diagnostics;

namespace Lakea_Stream_Assistant.Processes
{
    public class ExternalProcesses
    {
        private Dictionary<string, ExternalProcess> processes;

        public ExternalProcesses(ConfigApplication[] applications)
        {
            processes = new Dictionary<string, ExternalProcess>();
            foreach(ConfigApplication app in  applications)
            {
                try
                {
                    processes.Add(app.Name.ToLower(), new ExternalProcess(app));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Loading External application -> " + app.Name + ", " + ex.Message);
                    Logs.Instance.NewLog(LogLevel.Error, ex);
                }
            }
        }

        public void StartAllExternalProcesses()
        {
            foreach(var app in processes)
            {
                try
                {
                    Logs.Instance.NewLog(LogLevel.Info, "Starting Process " + app.Value.Name);
                    app.Value.StartProcess();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error Starting Process -> " + app.Value.Name + ", " + ex.Message);
                    Logs.Instance.NewLog(LogLevel.Error, ex);
                }
            }
        }

        public void StopAllExternalProcesses()
        {
            foreach(var app in processes)
            {
                try
                {
                    Logs.Instance.NewLog(LogLevel.Info, "Ending Process " + app.Value.Name);
                    app.Value.EndProcess();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error Ending Process -> " + app.Value.Name + ", " + ex.Message);
                    Logs.Instance.NewLog(LogLevel.Error, ex);
                }
            }
        }
    }
}
