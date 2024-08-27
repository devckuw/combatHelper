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
        private byte[] bufferNewComs = new byte[128];
        private List<(ChatMode, string, bool, int)> listComs = new List<(ChatMode, string, bool, int)>();
        private List<(ChatMode, string, bool, int)> listComsEdit;
        private int counter = 0;
        private Dictionary<string, List<(ChatMode, string, bool, int)>> dicListCom;
        private string listSelected = "Select..";
        private bool isListSelected = false;

        public Custom()
        {
            dicListCom = new Dictionary<string, List<(ChatMode, string, bool, int)>>(InfoManager.Configuration.CustomHelper);
        }

        public override void DrawHelper()
        {

            if (isInEditMode)
            {
                if (ImGui.Button("Save & Quit"))
                { 
                    isInEditMode = false;
                    listComs = new List<(ChatMode, string, bool, int)>(listComsEdit);
                    ResetListIndices();
                    SaveToConfig();
                }
                ImGui.SameLine();
                if (ImGui.Button("Undo & Quit")) { isInEditMode = false; }
                DrawEdit();
                DrawResult();
            }
            else
            {
                if (isListSelected)
                {
                    if (ImGui.Button("Edit")) { isInEditMode = true; listComsEdit = new List<(ChatMode, string, bool, int)>(listComs); }
                    ImGui.SameLine();
                    if (ImGui.Button("Delete"))
                    {
                        dicListCom.Remove(listSelected);
                        isListSelected = false;
                        listSelected = "Select..";
                        SaveToConfig();
                    }
                    ImGui.SameLine();
                }
                ImGui.SetNextItemWidth(120f);
                if (ImGui.BeginCombo("", listSelected))
                {
                    foreach (var key in dicListCom.Keys)
                    {
                        if (ImGui.Selectable(key))
                        {
                            listSelected = key;
                            listComs = new List<(ChatMode, string, bool, int)>(dicListCom[key]);
                            isListSelected = true;
                            counter = listComs.Count;
                        }
                    }
                    ImGui.EndCombo();
                }
                ImGui.SameLine();
                if (ImGui.Button("Add New"))
                {
                    var name = Encoding.UTF8.GetString(bufferNewComs);
                    if (!dicListCom.ContainsKey(name))
                    {
                        listSelected = name;
                        dicListCom[name] = new List<(ChatMode, string, bool, int)>();
                        listComs = new List<(ChatMode, string, bool, int)>();
                        isListSelected = true;
                        bufferNewComs = new byte[128];
                        counter = 0;
                    }
                }
                ImGui.SameLine();
                ImGui.SetNextItemWidth(150f);
                ImGui.InputText(" ", bufferNewComs, 128);
                if (isListSelected) { DrawResult(); }
            }
        }

        public void SaveToConfig()
        {
            InfoManager.Configuration.CustomHelper = new Dictionary<string, List<(ChatMode, string, bool, int)>>(dicListCom);
            InfoManager.Configuration.Save();
        }

        public void ResetListIndices()
        {
            List<(ChatMode, string, bool, int)> tmp = new List<(ChatMode, string, bool, int)>();
            for (int i = 0; i < listComs.Count; i++)
            {
                tmp.Add((listComs[i].Item1, listComs[i].Item2, listComs[i].Item3, i));
            }
            listComs = tmp;
            counter = listComs.Count;
            dicListCom[listSelected] = new List<(ChatMode, string, bool, int)>(listComs);
        }

        public void DrawResult()
        {
            if (isInEditMode)
            {
                bool start = true;
                foreach (var item in listComsEdit)
                {
                    if (!start && item.Item3) { ImGui.SameLine(); }
                    start = false;
                    if (ImGui.Button(item.Item2 + "##" + item.Item4.ToString()))
                    {
                        ChatHelper.Send(item.Item1, item.Item2);
                    }
                }
            }
            else
            {
                bool start = true;
                foreach (var item in listComs)
                {
                    if (!start && item.Item3) { ImGui.SameLine(); }
                    start = false;
                    if (ImGui.Button(item.Item2 + "##" + item.Item4.ToString()))
                    {
                        ChatHelper.Send(item.Item1, item.Item2);
                    }
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
                listComsEdit.Add((chatModeSelected, Encoding.UTF8.GetString(buffer), sameLine, counter));
                counter++;
            }
            foreach(var item in listComsEdit)
            {
                ImGui.Text(item.Item1.ToString());
                ImGui.SameLine();
                ImGui.Text(item.Item2);
                ImGui.SameLine();
                ImGui.Text(item.Item3.ToString());
                ImGui.SameLine();
                if (ImGui.Button("Remove##"+ item.Item4.ToString()))
                {
                    listComsEdit.Remove(item);
                }
            }
        }
    }
}
