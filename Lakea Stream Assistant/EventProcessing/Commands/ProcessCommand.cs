using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Processes;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    //Class for processing commands for external processes
    public class ProcessCommand
    {
        private ExternalProcesses externalProcesses;

        //Constructor sets the reference to the external processes object
        public ProcessCommand(ExternalProcesses externalProcesses)
        {
            this.externalProcesses = externalProcesses;
        }

        //Called when a new process command is received, checks the first arguments for function and calls relevant function
        public Dictionary<string, string> NewProcessCommand(LakeaCommand eve)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            if (eve.Args.Command.ArgumentsAsList.Count > 0)
            {
                switch (eve.Args.Command.ArgumentsAsList[0].ToLower())
                {
                    case "list":
                        args = getAllApplications(eve.Args.Command.ChatMessage.DisplayName);
                        break;
                    case "listactive":
                        args = getApplicationsByStatus(true, eve.Args.Command.ChatMessage.DisplayName);
                        break;
                    case "listinactive":
                        args = getApplicationsByStatus(false, eve.Args.Command.ChatMessage.DisplayName);
                        break;
                    case "startall":
                        args = startAllApplications(eve.Args.Command.ChatMessage.DisplayName);
                        break;
                    case "stopall":
                        args = stopAllApplications(eve.Args.Command.ChatMessage.DisplayName);
                        break;
                    case "start":
                    case "stop":
                        args = checkForSecondArgument(eve.Args.Command.ArgumentsAsList, eve.Args.Command.ChatMessage.DisplayName);
                        break;
                    default:
                        args.Add("Message", "I have no idea what you mean by 'process " + eve.Args.Command.ArgumentsAsList[0] + "'. Your going to have to try again @" + eve.Args.Command.ChatMessage.DisplayName);
                        break;
                }
            }
            else
            {
                args.Add("Message", "Process what? I need more info than that @" + eve.Args.Command.ChatMessage.DisplayName + "! Help me out here instead of being a twit!");
            }
            return args;
        }

        //Gets a list of all external applications and returns a EventItem that will post them to Twitch Chat
        private Dictionary<string, string> getAllApplications(string userName)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            List<string> applications = externalProcesses.GetAllApplications();
            if(applications.Count > 0)
            {
                args = createMessageDictionaryFromList(applications, "Ok @" + userName + ", here's all the apps I'm working with; ");
            }
            else
            {
                args.Add("Message", "I'm not working with any apps at the moment!");
            }
            return args;
        }

        //Gets a list of all applications by their activity status and returns and EventItem that will post them to Twitch chat
        private Dictionary<string, string> getApplicationsByStatus(bool active, string userName)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            List<string> applications = externalProcesses.GetApplicationsByStatus(active);
            string activeString = active ? "active" : "inactive";
            if (applications.Count > 0)
            {
                args = createMessageDictionaryFromList(applications, "Ok @" + userName + ", here's my current " + activeString + " apps");
            }
            else
            {
                args.Add("Message", "I don't have any " + activeString + " apps at the moment!");
            }
            return args;
        }

        //Restarts all the applications that Lakea is managing
        private Dictionary<string, string> startAllApplications(string userName)
        {
            externalProcesses.StartAllExternalProcesses();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Message", "I'll get everything restarted!" }
            };
            return args;
        }

        //Stops all the applications that Lakea is managing
        private Dictionary<string, string> stopAllApplications(string userName)
        {
            externalProcesses.StopAllExternalProcesses();
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Message", "Stopping everything!" }
            };
            return args;
        }

        //Checks that a 2nd argument exists before calling the relevant function
        private Dictionary<string, string> checkForSecondArgument(List<string> listArgs, string userName)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            if(listArgs.Count >= 2)
            {
                switch(listArgs[0].ToLower())
                {
                    case "stop":
                        args = stopApplication(listArgs[1], userName);
                        break;
                    case "start":
                        args = startApplication(listArgs[1], userName);
                        break;
                    default:
                        args.Add("Message", listArgs[0] + "? What on earth is that meant to mean @" + userName + "?");
                        break;
                }
            }
            else
            {
                string functionString = "start".Equals(listArgs[1].ToLower()) ? "Start" : "Stop";
                args.Add("Message", functionString + " what? What do you want me to do @" + userName + "?");
            }
            return args;
        }

        //Starts a specific application
        private Dictionary<string, string> startApplication(string application, string userName)
        {
            bool success = externalProcesses.StartExternalProcess(application.ToLower());
            if(success)
            {
                return new Dictionary<string, string> { { "Message", "I've started " + application + " up for you!" } };
            }
            else
            {
                return new Dictionary<string, string> { { "Message", "Sorry, I couldn't start " + application + " up for some reason" } };
            }
        }

        //Stops a specific application
        private Dictionary<string, string> stopApplication(string application, string userName)
        {
            bool success = externalProcesses.StopExternalProcess(application.ToLower());
            if(success)
            {
                return new Dictionary<string, string> { { "Message", "I've stopped " + application + " for you!" } };
            }
            else
            {
                return new Dictionary<string, string> { { "Message", "Sorry, I couldn't stop " + application + " for some reason" } };
            }
        }

        //Creates a dictionary with a message value from a list of application strings
        private Dictionary<string, string> createMessageDictionaryFromList(List<string> applications, string message)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            for (int index = 0; index < applications.Count; index++)
            {
                if (index == 0)
                {
                    message += " " + applications[index];
                }
                else if (index < applications.Count - 1)
                {
                    message += ", " + applications[index];
                }
                else
                {
                    message += " & " + applications[index];
                }
            }
            args.Add("Message", message);
            return args;
        }
    }
}
