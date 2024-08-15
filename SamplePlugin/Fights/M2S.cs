using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace combatHelper.Fights
{
    public class M2S : Fight
    {
        private string csv = "M2S.csv";

        public M2S(string path)
        {
            lines = DataFrameManager.ProccessDF(Path.Combine(path, csv));
        }

        public override void DrawHelper() 
        {
            ImGui.Text("M2S fight.");
        }
    }
}
