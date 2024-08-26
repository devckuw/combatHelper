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
        public static bool isSplitEnable = false;
        public static bool isSplitOpen = false;

        public static void UpdateSound()
        {
            soundPlayer = new SoundPlayer(Configuration.Sound);
        }

        public static void UpdateSplitToggle(bool setup = false)
        {
            // split & show both
            if (Configuration.SplitTimeLineAndHelper && Configuration.ShowHelper && Configuration.ShowTimeLine)
            {
                if (!isSplitOpen && !setup)
                {
                    plugin.ToggleSplitHelperUI();
                    isSplitEnable = true;
                    isSplitOpen = true;
                }
                else
                {
                    isSplitEnable = true;
                }
            }
            else
            {
                if (isSplitOpen)
                {
                    plugin.ToggleSplitHelperUI();
                    isSplitOpen = false;
                    isSplitEnable = false;
                }
            }
        }

    }
}
