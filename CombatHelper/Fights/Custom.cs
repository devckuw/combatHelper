using combatHelper.Utils;
using combatHelper.Windows;
using Dalamud.Game.Gui;
using Dalamud.Interface.Components;
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
        private string newCom = string.Empty;
        private string nameNewCustom = string.Empty;
        private List<(ChatMode, string, bool, int)> listComs = new List<(ChatMode, string, bool, int)>();
        private List<(ChatMode, string, bool, int)> listComsEdit = new List<(ChatMode, string, bool, int)>();
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
            // edit mode
            if (isInEditMode)
            {
                if (ImGui.Button("Save & Quit"))
                { 
                    isInEditMode = false;
                    listComs = new List<(ChatMode, string, bool, int)>(listComsEdit);
                    ResetListIndices();
                    SaveToConfig();
                }
                DrawCommon.IsHovered("Save changes to file.");
                ImGui.SameLine();
                if (ImGui.Button("Undo & Quit")) { isInEditMode = false; }
                DrawCommon.IsHovered("Discard changes.");
                DrawEdit();
                DrawResult();
                return;
            }

            // normal mode
            if (isListSelected)
            {
                if (ImGui.Button("Edit")) { isInEditMode = true; listComsEdit = new List<(ChatMode, string, bool, int)>(listComs); }
                DrawCommon.IsHovered("Enter edit mode.");
                ImGui.SameLine();
                if (ImGui.Button("Delete"))
                {
                    dicListCom.Remove(listSelected);
                    isListSelected = false;
                    listSelected = "Select..";
                    SaveToConfig();
                }
                DrawCommon.IsHovered("Delete selected preset.");
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
                if (!dicListCom.ContainsKey(nameNewCustom))
                {
                    listSelected = nameNewCustom;
                    dicListCom[nameNewCustom] = new List<(ChatMode, string, bool, int)>();
                    listComs = new List<(ChatMode, string, bool, int)>();
                    isListSelected = true;
                    nameNewCustom = string.Empty;
                    counter = 0;
                }
            }
            DrawCommon.IsHovered("Add new preset mode.");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(150f);
            ImGui.InputTextWithHint("##inputNameAddCustom", "Name for new preset.", ref nameNewCustom, 64);
            if (isListSelected) { DrawResult(); }
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
            bool start = true;

            // edit mode
            if (isInEditMode)
            {
                for (int i = 0;i < listComsEdit.Count;i++)
                {
                    if (i != 0 && listComsEdit[i].Item3) { ImGui.SameLine(); }
                    if (ImGui.Button(listComsEdit[i].Item2 + "##" + listComsEdit[i].Item4.ToString()))
                    {
                        ChatHelper.Send(listComsEdit[i].Item1, listComsEdit[i].Item2);
                    }
                    if (i != 0)
                    {
                        ImGui.SameLine();
                        var tmp = listComsEdit[i].Item3;
                        if (ImGui.Checkbox("##" + listComsEdit[i].Item4.ToString(), ref tmp))
                        {
                            var newItem = (listComsEdit[i].Item1, listComsEdit[i].Item2, tmp, listComsEdit[i].Item4);
                            listComsEdit.RemoveAt(i);
                            listComsEdit.Insert(i, newItem);
                        }
                        DrawCommon.IsHovered("Stay on same line.");
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Del##" + listComsEdit[i].Item4.ToString()))
                    {
                        listComsEdit.RemoveAt(i);
                    }
                    DrawCommon.IsHovered("Delete command on left.");
                }
                return;
            }

            // normal mode
            for (int i = 0; i < listComs.Count; i++)
            {
                if (!start && listComs[i].Item3) { ImGui.SameLine(); }
                start = false;
                if (ImGui.Button(listComs[i].Item2 + "##" + listComs[i].Item4.ToString()))
                {
                    ChatHelper.Send(listComs[i].Item1, listComs[i].Item2);
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
            DrawCommon.Helper("Chat Mode to send the command.");

            //ImGui.InputText(" ", buffer, 128);
            ImGui.InputTextWithHint("##newComInput", "Enter new command here.", ref newCom, 64);
            
            ImGui.Checkbox("Same Line", ref sameLine);
            DrawCommon.Helper("Stay on same line ?");

            if (ImGui.Button("Add"))
            {
                listComsEdit.Add((chatModeSelected, newCom, sameLine, counter));
                counter++;
            }
            DrawCommon.IsHovered("Add the command.");

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
                DrawCommon.IsHovered("Delete the command.");
            }
        }
    }
}
