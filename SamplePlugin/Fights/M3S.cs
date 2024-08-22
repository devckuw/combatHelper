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
    public class M3S : Fight
    {
        private string csv = "M3S.csv";

        public M3S()
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
            ImGui.Text("M3S fight.");
        }
    }
}
