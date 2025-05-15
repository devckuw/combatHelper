using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using combatHelper.Utils;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Common.Component.BGCollision;
using ImGuiNET;
using Lumina.Data.Structs;
using Lumina.Excel.Sheets;
using Lumina.Models.Models;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.VfxContainer;

namespace combatHelper.Fights
{
    public class M8S : Fight
    {
        private string csv = "M7S.csv";

        //p1
        private string savedFang = string.Empty;
        private bool p1 = true;

        //p2
        private int currentMech = 0;
        private int circuitPos = -1;
        private int circuitDir = 0;
        private string[] circuit = { "Donut", "In", "Out", "In", "Sides" };
        private string[] mechs = {"LP stacks SW / SE\nConga\nGroup South | Tanks TB",
            "All South\nBait boss cleave\nDodge cleave and in/out",
            "Conga\nLP SW / SE\nAll South bait Mooncleaver",
            "Tanks SW / SE for TB\nGroup NW / NE (check tank cleave)\nSplit into pairs for Towers\nTether / Line bait mech",
            "South is back\nAll South for 5 shape / bits dodges",
            "LP stacks SW / SE\nConga\nGroup South | Tanks TB",
            "Preposition for Blue / Green Tether / Tower\nTanks NW\nDPS SW\nHeal S",
            "Look for boss cleave and in/out, dodge\nConga in scuffed parties\nCheck for new South, defams, bits",
            "All South\nHe’s gonna start breaking platforms\nGo CW, invuln 3 and 4"};

        public M8S()
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
            currentMech = 0;
            circuitPos = -1;
            circuitDir = 0;
            savedFang = string.Empty;
            p1 = true;
        }

        public void DrawP1()
        {
            if (savedFang != string.Empty)
            {
                ImGui.TextUnformatted(savedFang);
            }
            else
            {
                if (ImGui.Button("Spread First"))
                {
                    savedFang = "Stack Later";
                }
                ImGui.SameLine();
                if (ImGui.Button("Stack First"))
                {
                    savedFang = "Spread Later";
                }
            }
            if (ImGui.Button("p2"))
            {
                p1 = false;
            }
        }

        public override void DrawHelper()
        {
            if (p1)
                DrawP1();
            else
                DrawP2();
        }

        public void DrawP2()
        {
            if (ImGui.Button("Prev"))
            {
                currentMech = Math.Max(currentMech-1, 0);
            }
            ImGui.SameLine();
            if (ImGui.Button("Next"))
            {
                currentMech = Math.Min(currentMech + 1, mechs.Length-1);
            }

            ImGui.SameLine();
            ImGui.TextUnformatted($"{currentMech+1}/{mechs.Length}");
            ImGui.TextUnformatted(mechs[currentMech]);

            if (currentMech == 4)
            {
                if (ImGui.Button("Donut")) { circuitPos = 0; }
                ImGui.SameLine();
                if (ImGui.Button("In##in1")) { circuitPos = 1; }
                ImGui.SameLine();
                if (ImGui.Button("Out")) { circuitPos = 2; }
                ImGui.SameLine();
                if (ImGui.Button("In##in2")) { circuitPos = 3; }
                ImGui.SameLine();
                if (ImGui.Button("Sides")) { circuitPos = 4; }
                if (ImGui.Button("CW")) { circuitDir = 1; }
                ImGui.SameLine();
                if (ImGui.Button("CWW")) { circuitDir = -1; }

                if ( circuitDir != 0 && circuitPos != -1)
                {
                    for (int i = 0; i < circuit.Length; i++)
                    {
                        var x = (circuitPos + i * circuitDir) % 5;
                        if (x < 0)
                            x += 5;
                        ImGui.TextUnformatted($"{circuit[x]}");
                        ImGui.SameLine();
                    }
                }
            }

        }
    }
}
