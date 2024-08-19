using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using Microsoft.Data.Analysis;

namespace combatHelper.Fights
{
    public class M1S : Fight
    {
        private string csv = "M1S.csv";
        private string resolveA = "";
        private string resolveC = "";
        private bool firstMech = true;
        private bool firstIsA = false;
        private bool isTpLeft = false;
        private bool isCleaveLeft = false;
        private string mech1 = "";
        private string mech2 = "";
        private bool firsttpchosen = false;
        private bool firstcleavechosen = false;

        public M1S(string path) 
        {
            csv = Path.Combine(path,csv);
            GenerateLines();
        }

        public void GenerateLines()
        {
            lines = DataFrameManager.ProccessDF(csv);
        }

        public override void DrawHelper()
        {
            //ImGui.Text($"The random config bool is {Plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");
            if (firstMech) { drawFirstMech(); }
            else if (!firstMech && !firsttpchosen) { drawSecondMech(); }
            else
            {
                ImGui.Text(resolveA + "\n");
                ImGui.Text(resolveC);
            }
            ImGui.Text(mech1);
            ImGui.Text(mech2);
            if (ImGui.Button("Reset"))
            {
                resolveA = "";
                resolveC = "";
                firstMech = true;
                firstIsA = false;
                isTpLeft = false;
                isCleaveLeft = false;
                mech1 = "";
                mech2 = "";
                firsttpchosen = false;
                firstcleavechosen = false;
            }


        }

        public void drawSecondMech()
        {
            if (ImGui.Button("Tp Left"))
            {
                isTpLeft = true;
                firsttpchosen = true;
                mech2 += "Tp left, ";
            }
            ImGui.SameLine();
            if (ImGui.Button("Tp Right"))
            {
                isTpLeft = false;
                firsttpchosen = true;
                mech2 += "Tp right, ";
            }
            if (firsttpchosen) { resolveSecond(); }
        }

        public void resolveSecond()
        {
            if (!firstIsA)
            {
                mech2 += "From A.";
                if (isTpLeft) { resolveA += "A => between 2 and B + protean"; }
                else { resolveA += "A => between 1 and D + protean"; }
            }
            else
            {
                mech2 += "From C.";
                if (isTpLeft) { resolveC += "C => between 4 and D + protean"; }
                else { resolveC += "C => between 3 and B + protean"; }
            }
        }

        public void resolveFirst()
        {
            if (firstIsA)
            {
                if (isTpLeft)
                {
                    if (isCleaveLeft) { resolveA += "A => between 2 and B cleave outter"; }
                    else { resolveA += "A => between 2 and B cleave inner"; }

                }
                else
                {
                    if (isCleaveLeft) { resolveA += "A => between 1 and D cleave inner"; }
                    else { resolveA += "A => between 1 and D cleave outter"; }
                }
            }
            else
            {
                if (isTpLeft)
                {
                    if (isCleaveLeft) { resolveC += "C => between 4 and D cleave outter"; }
                    else { resolveC += "C => between 4 and D cleave inner"; }

                }
                else
                {
                    if (isCleaveLeft) { resolveC += "C => between 3 and B cleave inner"; }
                    else { resolveC += "C => between 3 and B cleave outter"; }
                }
            }
            firsttpchosen = false;
        }

        public void drawFirstMech()
        {
            if (ImGui.Button("Tp Left"))
            {
                isTpLeft = true;
                firsttpchosen = true;
                mech1 += "Tp left, ";
            }
            ImGui.SameLine();
            if (ImGui.Button("Tp Right"))
            {
                isTpLeft = false;
                firsttpchosen = true;
                mech1 += "Tp right, ";
            }
            if (ImGui.Button("Cleave Left"))
            {
                isCleaveLeft = true;
                firstcleavechosen = true;
                mech1 += "Cleave left, ";
            }
            ImGui.SameLine();
            if (ImGui.Button("Cleave Right"))
            {
                isCleaveLeft = false;
                firstcleavechosen = true;
                mech1 += "Cleave right, ";
            }
            if (ImGui.Button("A"))
            {
                mech1 += "From A.";
                firstIsA = true;
                firstMech = !(firstcleavechosen && firsttpchosen);
            }
            ImGui.SameLine();
            if (ImGui.Button("C"))
            {
                firstIsA = false;
                firstMech = !(firstcleavechosen && firsttpchosen);
                mech1 += "From C.";
            }
            if (!firstMech) { resolveFirst(); }
        }
    }
}
