using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace combatHelper.Utils
{
    public class InfoManager
    {
        public static Configuration Configuration =  null;
        public static SoundPlayer soundPlayer;
        public static FightState fight;

        public static void UpdateSound()
        {
            soundPlayer = new SoundPlayer(Configuration.Sound);
        }

    }
}
