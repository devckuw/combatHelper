using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace combatHelper.Fights
{
    public class M2S : Fight
    {
        public override void DrawHelper() 
        {
            ImGui.Text("M2S figth.");
        }
    }
}
