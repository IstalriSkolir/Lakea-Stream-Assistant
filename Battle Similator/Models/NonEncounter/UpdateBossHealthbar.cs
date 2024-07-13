using Battle_Similator.Models.Creatures;
using Battle_Similator.Models.Resources;

namespace Battle_Similator.Models.NonEncounter
{
    public class UpdateBossHealthbar
    {
        private IO io;
        private HealthBarImage healthBar;
        private string[] bossList;

        public UpdateBossHealthbar(IO io, string config, string resourcePath)
        {
            this.io = io;
            healthBar = new HealthBarImage(io, config, resourcePath);
            this.bossList = io.LoadBossList();
        }

        public void Start()
        {
            Monster monster;
            if (io.CurrentBossFileExists())
            {
                monster = io.LoadNPCData(CreatureType.Boss, "CURRENTBOSS");
            }
            else
            {
                monster = io.LoadNPCData(CreatureType.Boss, bossList[0]);
            }
            healthBar.GenerateHealthBarImage(monster);
        }
    }
}
