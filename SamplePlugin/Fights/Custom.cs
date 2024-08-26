using combatHelper.Utils;
using FFXIVClientStructs.FFXIV.Common.Configuration;
using ImGuiNET;
using Lumina.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace combatHelper.Fights
{
    public class Custom : Fight
    {
        private bool isInEditMode = false;
        private string previewChat = "Select Chat";
        private ChatMode chatModeSelected = ChatMode.None;
        private bool sameLine = false;
        private byte[] buffer = new byte[128];
        private List<(ChatMode, string, bool, int)> listComs = new List<(ChatMode, string, bool, int)>();
        private int counter = 0;
        public Custom()
        {
            
        }

        public override void DrawHelper()
        {

            if (isInEditMode)
            {
                DrawEdit();
                if (ImGui.Button("Finish Edit")) { isInEditMode = false; }
                DrawResult();
            }
            else
            {
                DrawResult();
                if (ImGui.Button("Edit")) { isInEditMode = true; }
            }
        }

        public void DrawResult()
        {
            bool start = true;
            foreach (var item in listComs)
            {
                if (!start && item.Item3) { ImGui.SameLine(); }
                start = false;
                if (ImGui.Button(item.Item2+"##"+item.Item4.ToString()))
                {
                    ChatHelper.Send(item.Item1, item.Item2);
                }
            }
        }

        public void DrawEdit()
        {
            if (ImGui.BeginCombo("", previewChat))
            {
                if (ImGui.Selectable("Echo"))
                {
                    previewChat = ChatMode.Echo.ToString();
                    chatModeSelected = ChatMode.Echo;
                }
                if (ImGui.Selectable("Party"))
                {
                    previewChat = ChatMode.Party.ToString();
                    chatModeSelected = ChatMode.Party;
                }
                if (ImGui.Selectable("Alliance"))
                {
                    previewChat = ChatMode.Alliance.ToString();
                    chatModeSelected = ChatMode.Alliance;
                }
                ImGui.EndCombo();
            }

            ImGui.InputText(" ", buffer, 128);
            
            ImGui.Checkbox("Same Line", ref sameLine);

            if (ImGui.Button("Add"))
            {
                listComs.Add((chatModeSelected, Encoding.UTF8.GetString(buffer), sameLine, counter));
                counter++;
            }
            foreach(var item in listComs)
            {
                ImGui.Text(item.Item1.ToString());
                ImGui.SameLine();
                ImGui.Text(item.Item2);
                ImGui.SameLine();
                ImGui.Text(item.Item3.ToString());
                ImGui.SameLine();
                if (ImGui.Button("Remove##"+ item.Item4.ToString()))
                {
                    listComs.Remove(item);
                }
            }
        }
    }
}
