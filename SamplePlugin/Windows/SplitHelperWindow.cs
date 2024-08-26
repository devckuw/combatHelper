using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using combatHelper.Utils;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace combatHelper.Windows
{
    internal class SplitHelperWindow : Window, IDisposable
    {
        public SplitHelperWindow() : base("Split", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoTitleBar)
        {

        }

        public void Dispose()
        {

        }

        public override void Draw()
        {
            DrawCommon.MenuBar();
            if (InfoManager.fightState == FightState.None) { return; }
            InfoManager.fight.DrawHelper();
        }
    }
}
