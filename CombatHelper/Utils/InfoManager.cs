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
        public static bool isHelperOpen = false;
        public static bool isMainOpen = false;
        //public static bool isSplitEnable = false;

        public static void UpdateSound()
        {
            soundPlayer = new SoundPlayer(Configuration.Sound);
        }

        public static void UpdateSplitToggle()
        {
            //isSplitEnable = Configuration.SplitTimeLineAndHelper;
            // split & show both
            if (!Configuration.SplitTimeLineAndHelper)
            {
                if (isHelperOpen)
                {
                    plugin.ToggleSplitHelperUI();
                    isHelperOpen = false;
                }
                return;
            }
            if (Configuration.ShowHelper && Configuration.ShowTimeLine)
            {
                if (isMainOpen == isHelperOpen)
                {
                    return;
                }
                if (isMainOpen)
                {
                    plugin.ToggleSplitHelperUI();
                    isHelperOpen = true;
                    return;
                }
                plugin.ToggleMainUI();
                isMainOpen = true;
                return;
            }
            if (!Configuration.ShowHelper && !Configuration.ShowTimeLine)
            {
                if (isHelperOpen)
                {
                    plugin.ToggleSplitHelperUI();
                    isHelperOpen = false;
                }
                if (isMainOpen)
                {
                    plugin.ToggleMainUI();
                    isMainOpen = false;
                }
                return;
            }
            if (!Configuration.ShowHelper && Configuration.ShowTimeLine)
            {
                if (isHelperOpen)
                {
                    plugin.ToggleSplitHelperUI();
                    isHelperOpen = false;
                }
                return;
            }
            if (Configuration.ShowHelper && !Configuration.ShowTimeLine)
            {
                if (isMainOpen)
                {
                    plugin.ToggleMainUI();
                    isMainOpen = false;
                }
                return;
            }
            /*
            if (Configuration.SplitTimeLineAndHelper && Configuration.ShowHelper && Configuration.ShowTimeLine)
            {
                if (isMainOpen == isHelperOpen)
                {
                    return;
                }
                if (isMainOpen)
                { 
                    plugin.ToggleSplitHelperUI();
                    isHelperOpen = true;
                    return; 
                }
                plugin.ToggleMainUI();
                isMainOpen = true;
                return;
            }
            if (!Configuration.SplitTimeLineAndHelper && Configuration.ShowHelper && Configuration.ShowTimeLine)
            {
                if (isHelperOpen)
                {
                    plugin.ToggleSplitHelperUI();
                    isHelperOpen = false;
                }
                return;
            }
            if (Configuration.SplitTimeLineAndHelper && Configuration.ShowHelper && !Configuration.ShowTimeLine)
            {
                if (isHelperOpen)
                {
                    plugin.ToggleSplitHelperUI();
                    isHelperOpen = false;
                }
                return;
            }
            if (Configuration.SplitTimeLineAndHelper && !Configuration.ShowHelper && Configuration.ShowTimeLine)
            {
                if (isMainOpen)
                {
                    plugin.ToggleMainUI();
                    isMainOpen = false;
                }
                return;
            }*/
        }

        public static void ProcessToggle()
        {
            if (!Configuration.SplitTimeLineAndHelper)
            {
                plugin.ToggleMainUI();
                isMainOpen = !isMainOpen;
                if (isHelperOpen)
                {
                    plugin.ToggleSplitHelperUI();
                    isHelperOpen = false;
                }
                return;
            }
            if (Configuration.ShowHelper && Configuration.ShowTimeLine)
            {
                if (isMainOpen == isHelperOpen)
                {
                    plugin.ToggleSplitHelperUI();
                    isHelperOpen = !isHelperOpen;
                    plugin.ToggleMainUI();
                    isMainOpen = !isMainOpen;
                    return;
                }
                if (isMainOpen)
                {
                    plugin.ToggleSplitHelperUI();
                    isHelperOpen = true;
                    return;
                }
                plugin.ToggleMainUI();
                isMainOpen = true;
                return;
            }
            if (Configuration.ShowHelper)
            {
                //plugin.ToggleMainUI();
                //isMainOpen = !isMainOpen;
                plugin.ToggleSplitHelperUI();
                isHelperOpen = !isHelperOpen;
                return;
            }
            if (Configuration.ShowTimeLine)
            {
                plugin.ToggleMainUI();
                isMainOpen = !isMainOpen;
                //plugin.ToggleSplitHelperUI();
                //isHelperOpen = !isHelperOpen;
                return;
            }
            plugin.ToggleMainUI();
            isMainOpen = !isMainOpen;
        }

    }
}
