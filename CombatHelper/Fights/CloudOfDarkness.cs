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
    public class CloudOfDarkness : Fight
    {
        private string csv = "CloudOfDarkness.csv";
        private string hands = String.Empty;
        private string saved = String.Empty;
        private bool isP1 = true;

        public CloudOfDarkness()
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
            hands = String.Empty;
            saved = String.Empty;
            isP1 = true;
        }

        public override void DrawHelper()
        {
            if (isP1)
            {
                if (ImGui.Button("Death"))
                {
                    saved = "Death saved";
                    ChatHelper.Send(InfoManager.Configuration.ChatMode, "Death saved");
                }
                ImGui.SameLine();
                if (ImGui.Button("Aero"))
                {
                    saved = "Aero saved";
                    ChatHelper.Send(InfoManager.Configuration.ChatMode, "Aero saved");
                }
                ImGui.SameLine();
                if (ImGui.Button("Reset##savedreset"))
                {
                    saved = String.Empty;
                }
                ImGui.Text(saved);

                if (ImGui.Button("Front"))
                {
                    hands = "Run Forward";
                }
                ImGui.SameLine();
                if (ImGui.Button("Back"))
                {
                    hands = "Run Backward";
                }
                ImGui.SameLine();
                if (ImGui.Button("Reset##handsreset"))
                {
                    hands = String.Empty;
                }
                ImGui.Text(hands);
                if (ImGui.Button("P2"))
                {
                    isP1 = false;
                    hands = String.Empty;
                    saved = String.Empty;
                }
            }
            else
            {
                if (ImGui.Button("Donnut"))
                {
                    ChatHelper.Send(InfoManager.Configuration.ChatMode, "Donnut");
                }
                ImGui.SameLine();
                if (ImGui.Button("Cross"))
                {
                    ChatHelper.Send(InfoManager.Configuration.ChatMode, "Cross");
                }
                if (ImGui.Button("P1"))
                {
                    isP1 = true;
                }
            }
        }
    }
}
