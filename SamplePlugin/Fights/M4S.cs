using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace combatHelper.Fights
{
    public class M4S : Fight
    {
        private string csv = "M4S.csv";
        private string safeSide = String.Empty;

        public M4S(string path)
        {
            csv = Path.Combine(path, csv);
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
            }
            ImGui.SameLine();
            if (ImGui.Button("Right"))
            {
                safeSide = "RIGHT SAFE";
            }
            ImGui.Text(safeSide);
        }
    }
}
