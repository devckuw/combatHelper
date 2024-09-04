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
    public class M4S : Fight
    {
        private string csv = "M4S.csv";
        private string safeSide = String.Empty;

        public M4S()
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
            if (ImGui.Button("Left"))
            {
                safeSide = "LEFT SAFE";
                ChatHelper.Send(InfoManager.Configuration.ChatMode, "Left Safe First");
            }
            ImGui.SameLine();
            if (ImGui.Button("Right"))
            {
                safeSide = "RIGHT SAFE";
                ChatHelper.Send(InfoManager.Configuration.ChatMode, "Right Safe First");
            }
            ImGui.Text(safeSide);
        }
    }
}
