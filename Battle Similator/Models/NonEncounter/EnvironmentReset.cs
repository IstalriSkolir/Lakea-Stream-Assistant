using Battle_Similator.Models.Creatures;
using Battle_Similator.Models.Resources;

namespace Battle_Similator.Models.NonEncounter
{
    public class EnvironmentReset
    {
        private IO io;
        HealthBarImage healthBar;
        private string path;
        public EnvironmentReset(IO io, string config, string resourcePath)
        {
            this.io = io;
            if (config == "LAKEA")
            {
                path = resourcePath;
            }
            else if (config == "DEBUG")
            {
                path = Environment.CurrentDirectory + "\\";
            }
            else
            {
                Environment.Exit((int)ExitCode.Invalid_Args);
            }
            this.healthBar = new HealthBarImage(io, config, path);
        }

        public void Start()
        {
            resetBossBattle();
        }

        private void resetBossBattle()
        {
            if(io.CurrentBossFileExists())
            {
                File.Delete(path + "Bosses\\CURRENTBOSS.txt");
            }
            if(File.Exists(path + "Bosses\\CURRENTBOSSFIGHTERS.txt"))
            {
                io.DeleteCurrentBossFighters();
            }
            string[] bosses = io.LoadBossList();
            Monster firstBoss = io.LoadNPCData(CreatureType.Boss, bosses[0]);
            healthBar.GenerateHealthBarImage(firstBoss);
        }
    }
}
