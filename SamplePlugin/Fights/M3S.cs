using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;

namespace combatHelper.Fights
{
    public class M3S : Fight
    {
        private string csv = "M3S.csv";

        public M3S(string path)
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
            ImGui.Text("M3S fight.");
        }
    }
}
