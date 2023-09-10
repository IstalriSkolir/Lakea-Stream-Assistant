using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Resources;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.Processes
{
    public class ExternalProcesses
    {
        private Dictionary<string, ExternalProcess> processes;

        public ExternalProcesses(ConfigApplication[] applications)
        {
            processes = new Dictionary<string, ExternalProcess>();
            if(applications != null)
            {
                foreach (ConfigApplication app in applications)
                {
                    try
                    {
                        processes.Add(app.Name.ToLower(), new ExternalProcess(app));
                    }
                    catch (Exception ex)
                    {
                        Terminal.Output("Error Loading External application -> " + app.Name + ", " + ex.Message);
                        Logs.Instance.NewLog(LogLevel.Error, ex);
                    }
                }
            }
            else
            {
                Terminal.Output("No External Applications Found");
                Logs.Instance.NewLog(LogLevel.Info, "No External Applications Found");
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
                    Terminal.Output("Error Starting Process -> " + app.Value.Name + ", " + ex.Message);
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
                    Terminal.Output("Error Ending Process -> " + app.Value.Name + ", " + ex.Message);
                    Logs.Instance.NewLog(LogLevel.Error, ex);
                }
            }
        }
    }
}
