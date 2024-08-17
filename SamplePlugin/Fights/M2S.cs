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
        private string spread_stack = String.Empty;

        public M2S(string path)
        {
            lines = DataFrameManager.ProccessDF(Path.Combine(path, csv));
        }

        public override void DrawHelper() 
        {
            if (ImGui.Button("Spread"))
            {
                spread_stack = "SPREAD";
            }
            ImGui.SameLine();
            if (ImGui.Button("Stack"))
            {
                spread_stack = "STACK";
            }
            ImGui.Text(spread_stack);
        }
    }
}
