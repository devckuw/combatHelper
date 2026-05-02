using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using combatHelper.Utils;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Common.Component.BGCollision;
using Dalamud.Bindings.ImGui;
using Lumina.Data.Structs;
using Lumina.Excel.Sheets;
using Lumina.Models.Models;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.VfxContainer;

namespace combatHelper.Fights
{
    public class M12S : Fight
    {
        private string csv = "M12S.csv";
        private int currentMech = 0;

        private string rep2 =   "                boss(A)\n"+
                                "    cleave(4)       cleave(1)\n"+
                                "stack(D)                 stack(B)\n"+
                                "     defam(3)        defam(2)\n"+
                                "             nothing(C)\n";

        private string candies = string.Empty;
        private string memory1 = string.Empty;
        private string memory2 = string.Empty;
        private string memory3 = string.Empty;
        private string memory4 = string.Empty;
        private string memory5 = string.Empty;

        private int posns = 0;


        private string[] mechs = {
            "rep 1\nnew north => exte dark",
            "",
            "candies",
            "idyllic"
        };

        public M12S()
        {
            //csv = Path.Combine(InfoManager.Configuration.AssemblyLocation, csv);
            //GenerateLines();
            TimeManager.Instance.OnFightStart += Reset;
            mechs[1] = rep2;
        }

        public void GenerateLines()
        {
            lines = DataFrameManager.ProccessDF(csv);
        }

        public void Reset()
        {
            //currentMech = 0;
            candies = string.Empty;
            memory1 = string.Empty;
            memory2 = string.Empty;
            memory3 = string.Empty;
            memory4 = string.Empty;
            memory5 = string.Empty;
            posns = 0;
        }

        public override void DrawHelper()
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
            ImGui.SameLine();
            ImGui.TextUnformatted(mechs[currentMech]);
            if (currentMech == 2)
            {
                if (ImGui.Button("D + green orange")) candies = "D => In + N/S \nB => In + N/S";
                ImGui.SameLine();
                if (ImGui.Button("B + green orange")) candies = "B => In + N/S \nD => In + N/S";

                if (ImGui.Button("D + blue purple")) candies = "B => In + N/S \nD => In + N/S";
                ImGui.SameLine();
                if (ImGui.Button("B + blue purple")) candies = "D => In + N/S \nB => In + N/S";

                ImGui.Text(candies);
            }
            if (currentMech == 3)
            {
                if (ImGui.Button("M1 Card")) 
                {
                    memory1 = "Cardinal";
                    ChatHelper.Send(InfoManager.Configuration.ChatMode, memory1);
                }
                ImGui.SameLine();
                if (ImGui.Button("M1 Inter")) 
                {
                    memory1 = "Intercard";
                    ChatHelper.Send(InfoManager.Configuration.ChatMode, memory1);
                }
                ImGui.SameLine();
                ImGui.Text(memory1);

                if (ImGui.Button("M2 North")) 
                {
                    memory2 = "North is Sides Safe";
                    posns = 1;
                    ChatHelper.Send(InfoManager.Configuration.ChatMode, memory2);
                }
                ImGui.SameLine();
                if (ImGui.Button("M2 South")) 
                {
                    memory2 = "South is Sides Safe";
                    posns = 2;
                    ChatHelper.Send(InfoManager.Configuration.ChatMode, memory2);
                }
                ImGui.SameLine();
                ImGui.Text(memory2);

                if (ImGui.Button("Defam letters")) 
                {
                    memory3 = "Defam first";
                    ChatHelper.Send(InfoManager.Configuration.ChatMode, memory3);
                }
                ImGui.SameLine();
                if (ImGui.Button("Stack letters")) 
                {
                    memory3 = "Stack first";
                    ChatHelper.Send(InfoManager.Configuration.ChatMode, memory3);
                }
                ImGui.SameLine();
                ImGui.Text(memory3);

                if (ImGui.Button("North portaled")) 
                {
                    memory4 = "North portaled";
                    if (posns == 1)
                    {
                        memory4 = "Between NOT safe";
                    }
                    if (posns == 2)
                    {
                        memory4 = "Between safe";
                    }
                    ChatHelper.Send(InfoManager.Configuration.ChatMode, memory4);
                }
                ImGui.SameLine();
                if (ImGui.Button("South portaled")) 
                {
                    memory4 = "South portaled";
                    if (posns == 1)
                    {
                        memory4 = "Between safe";
                    }
                    if (posns == 2)
                    {
                        memory4 = "Between NOT safe";
                    }
                    ChatHelper.Send(InfoManager.Configuration.ChatMode, memory4);
                }
                ImGui.SameLine();
                ImGui.Text(memory4);

                if (ImGui.Button("A")) memory5 = "A/1 defam";
                ImGui.SameLine();
                if (ImGui.Button("1")) memory5 = "A/1 stack";
                ImGui.SameLine();
                if (ImGui.Button("B")) memory5 = "B/2 stack";
                ImGui.SameLine();
                if (ImGui.Button("2")) memory5 = "B/2 defam";
                ImGui.SameLine();
                if (ImGui.Button("C")) memory5 = "C/3 defam";
                ImGui.SameLine();
                if (ImGui.Button("3")) memory5 = "C/3 stack";
                ImGui.SameLine();
                if (ImGui.Button("D")) memory5 = "D/4 stack";
                ImGui.SameLine();
                if (ImGui.Button("4")) memory5 = "D/4 defam";
                ImGui.SameLine();
                ImGui.Text(memory5);
                
            }
        }

    }
}
