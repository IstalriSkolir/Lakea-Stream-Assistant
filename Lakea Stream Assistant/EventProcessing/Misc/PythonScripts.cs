using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using System.Diagnostics;

namespace Lakea_Stream_Assistant.EventProcessing.Misc
{
    public class PythonScripts
    {
        private List<Process> Processes;
        private string pythonPath;
        private string scriptFolder;

        public PythonScripts(string pythonPath, string resourcePath)
        {
            Processes = new List<Process>();
            this.pythonPath = pythonPath;
            if(resourcePath == null || resourcePath == string.Empty || resourcePath.Equals("default"))
            {
                scriptFolder = Environment.CurrentDirectory + "\\Resources\\Python\\";
            }
            else
            {
                scriptFolder = resourcePath + "\\Python\\";
            }
        }

        public void RunPythonScript(Dictionary<string, string> args)
        {
            try
            {
                Terminal.Output("Lakea: Running Python Script -> " + args["Script"] + ".py");
                Logs.Instance.NewLog(LogLevel.Info, "Running Python Script -> " + args["Script"]);
                string argsString = getScriptArguments(args);
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = pythonPath;
                startInfo.Arguments = "\"" + scriptFolder + args["Script"] + ".py\" + " + argsString;
                Process process = Process.Start(startInfo);
                Processes.Add(process);
            }
            catch(Exception ex)
            {
                Terminal.Output("Lakea: Python Script Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        private string getScriptArguments(Dictionary<string, string> args)
        {
            string argsString = "";
            foreach(string key in  args.Keys)
            {
                if (key.Contains("Arg"))
                {
                    argsString += " \"" + args[key] + "\"";
                }
            }
            return argsString;
        }
    }
}