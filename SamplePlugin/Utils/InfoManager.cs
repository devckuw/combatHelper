using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using combatHelper.Fights;

namespace combatHelper.Utils
{
    public class InfoManager
    {
        public static Configuration Configuration =  null;
        public static SoundPlayer soundPlayer = null;
        public static FightState fightState = FightState.None;
        public static Fight fight = null;
        public static NbPots nbPots = NbPots.None;
        public static Plugin plugin;

        public static void UpdateSound()
        {
            soundPlayer = new SoundPlayer(Configuration.Sound);
        }

    }
}
