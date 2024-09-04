using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using combatHelper.Utils;
using ImGuiNET;

namespace combatHelper.Fights
{
    public class M2S : Fight
    {
        private string csv = "M2S.csv";
        private string spread_stack = String.Empty;

        public M2S()
        {
            csv = Path.Combine(InfoManager.Configuration.AssemblyLocation, csv);
            GenerateLines();
        }

        public void GenerateLines()
        {
            lines = DataFrameManager.ProccessDF(csv);
        }

        public override void DrawHelper() 
        {
            if (ImGui.Button("Spread"))
            {
                spread_stack = "SPREAD";
                ChatHelper.Send(InfoManager.Configuration.ChatMode, "Spread");
            }
            ImGui.SameLine();
            if (ImGui.Button("Stack"))
            {
                spread_stack = "STACK";
                ChatHelper.Send(InfoManager.Configuration.ChatMode, "Stack");
            }
            ImGui.Text(spread_stack);
        }
    }
}
