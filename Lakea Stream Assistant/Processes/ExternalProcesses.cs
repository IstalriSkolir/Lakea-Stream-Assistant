using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Resources.Lakea;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.Processes
{
    //Controls the running of the external applications that Lakea runs
    public class ExternalProcesses
    {
        private Dictionary<string, ExternalProcess> processes;

        //Constructor sets up each ExternalProcess from the loaded configuration object
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

        //Gets a list of all the applications Lakea is managing
        public List<string> GetAllApplications()
        {
            List<string> applications = new List<string>();
            foreach(ExternalProcess app in processes.Values)
            {
                applications.Add(app.Name);
            }
            return applications;
        }

        //Gets a list of the applications by their active status
        public List<string> GetApplicationsByStatus(bool active)
        {
            List<string> activeApplications = new List<string>();
            foreach(ExternalProcess app in processes.Values)
            {
                if (app.Active == active)
                {
                    activeApplications.Add(app.Name);
                }
            }
            return activeApplications;
        }

        //Starts all the external processes
        public void StartAllExternalProcesses()
        {
            foreach(var app in processes)
            {
                try
                {
                    Terminal.Output("Lakea: Starting Process -> " + app.Value.Name);
                    Logs.Instance.NewLog(LogLevel.Info, "Starting Process " + app.Value.Name);
                    app.Value.StartProcess();
                }
                catch(Exception ex)
                {
                    Terminal.Output("Lakea: Error Starting Process -> " + app.Value.Name + ", " + ex.Message);
                    Logs.Instance.NewLog(LogLevel.Error, ex);
                }
            }
        }

        //Stops all the external processes
        public void StopAllExternalProcesses()
        {
            foreach(var app in processes)
            {
                try
                {
                    Terminal.Output("Lakea: Ending Process -> " + app.Value.Name);
                    Logs.Instance.NewLog(LogLevel.Info, "Ending Process " + app.Value.Name);
                    app.Value.EndProcess();
                }
                catch(Exception ex)
                {
                    Terminal.Output("Lakea: Error Ending Process -> " + app.Value.Name + ", " + ex.Message);
                    Logs.Instance.NewLog(LogLevel.Error, ex);
                }
            }
        }

        //Starts an external application by name
        public bool StartExternalProcess(string application)
        {
            try
            {
                if (processes.ContainsKey(application))
                {
                    if (!processes[application].Active)
                    {
                        processes[application].StartProcess();
                        Terminal.Output("Lakea: Starting Process -> " + processes[application].Name);
                        Logs.Instance.NewLog(LogLevel.Info, "Starting Process " + processes[application].Name);
                        return true;
                    }
                    else
                    {
                        Terminal.Output("Lakea: Starting Process -> " + processes[application].Name + " is already running!");
                        Logs.Instance.NewLog(LogLevel.Warning, processes[application].Name + " is already running!");
                        return false;
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Application -> " + application);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Application -> " + application);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Error Starting Process -> " + application + ", " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
                return false;
            }
        }

        //Stops an external applicaiton by name
        public bool StopExternalProcess(string application)
        {
            try
            {
                if (processes.ContainsKey(application))
                {
                    if (processes[application].Active)
                    {
                        processes[application].EndProcess();
                        Terminal.Output("Lakea: Ending Process -> " + processes[application].Name);
                        Logs.Instance.NewLog(LogLevel.Info, "Ending Process " + processes[application].Name);
                        return true;
                    }
                    else
                    {
                        Terminal.Output("Lakea: Ending Process -> " + processes[application].Name + " isn't running!");
                        Logs.Instance.NewLog(LogLevel.Warning, processes[application].Name + " isn't running!");
                        return false;
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Application -> " + application);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Application -> " + application);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Error Ending Process -> " + application + ", " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
                return false;
            }

        }
    }
}
