using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using combatHelper.Utils;
using ImGuiNET;
using InteropGenerator.Runtime;

namespace combatHelper.Fights
{
    public class M5S : Fight
    {
        private string csv = "M5S.csv";
        private string lastMech = string.Empty;

        public M5S()
        {
            //csv = Path.Combine(InfoManager.Configuration.AssemblyLocation, csv);
            //GenerateLines();
            TimeManager.Instance.OnFightStart += Reset;
        }

        public void GenerateLines()
        {
            lines = DataFrameManager.ProccessDF(csv);
        }

        public void Reset()
        {
            lastMech = string.Empty;
        }

        public override void DrawHelper()
        {
            ImGui.TextUnformatted(lastMech);

            //ImGui.Separator();
            //ImGui.NewLine();

            if (ImGui.Button("A => T/H/D"))
            {
                lastMech = "T/H/D";
                ChatHelper.Send(InfoManager.Configuration.ChatMode, lastMech);
            }
            ImGui.SameLine();
            if (ImGui.Button("B => Light Party"))
            {
                lastMech = "Light Party";
                ChatHelper.Send(InfoManager.Configuration.ChatMode, lastMech);
            }

            //ImGui.Separator();
            //ImGui.NewLine();

            if (ImGui.Button("Saved A => T/H/D"))
            {
                lastMech = "Saved T/H/D";
                ChatHelper.Send(InfoManager.Configuration.ChatMode, lastMech);
            }
            ImGui.SameLine();
            if (ImGui.Button("Saved B => Light Party"))
            {
                lastMech = "Saved Light Party";
                ChatHelper.Send(InfoManager.Configuration.ChatMode, lastMech);
            }
        }
    }
}
