using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.System.Memory;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.Interop;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dalamud.Interface.Utility.Raii.ImRaii;
using static FFXIVClientStructs.FFXIV.Client.UI.RaptureAtkHistory.Delegates;
using combatHelper.Utils;
using ImGuiNET;

namespace combatHelper.Tweaks
{
    public unsafe class ShieldTweaks : IDisposable
    {

        //private uint id_base = 12332162;
        private uint[] ids = [12332162, 12332163, 12332164, 12332165, 12332166, 12332167, 12332168, 12332169];
        private bool[] visible = [false, false, false, false, false, false, false, false];
        private List<(byte, uint)> actorsStats;

        public ShieldTweaks()
        {
            actorsStats = new List<(byte, uint)>();
            Plugin.Framework.Update += OnUpdate;
        }

        public void Dispose()
        {
            CleanShieldTweaks();
            Plugin.Framework.Update -= OnUpdate;
        }

        private void OnUpdate(IFramework framework)
        {

            UpdateVisibility();
            FillActors();

            var partyList = (AddonPartyList*)Plugin.GameGui.GetAddonByName("_PartyList");
            if (partyList == null) return;

            for (var j = 0; j < 8; j++)
            {
                AtkTextNode* textNode = null;
                for (var i = 0; i < partyList->UldManager.NodeListCount; i++)
                {
                    if (partyList->UldManager.NodeList[i] == null) continue;
                    if (partyList->UldManager.NodeList[i]->NodeId == ids[j])
                    {
                        textNode = (AtkTextNode*)partyList->UldManager.NodeList[i];
                        if (!visible[j])
                        {
                            partyList->UldManager.NodeList[i]->ToggleVisibility(false);
                            continue;
                        }

                        break;
                    }
                }

                if (textNode == null && !visible[j]) return;
                if (textNode == null)
                {
                    var newTextNode = IMemorySpace.GetUISpace()->Create<AtkTextNode>();
                    if (newTextNode != null)
                    {
                        var lastNode = partyList->RootNode;
                        if (lastNode == null) return;

                        textNode = newTextNode;

                        newTextNode->AtkResNode.Type = NodeType.Text;
                        newTextNode->AtkResNode.NodeFlags = NodeFlags.AnchorLeft | NodeFlags.AnchorTop;
                        newTextNode->AtkResNode.DrawFlags = 0;
                        textNode->AtkResNode.SetPositionFloat(133, 65+40*j);
                        newTextNode->AtkResNode.SetWidth(50);
                        newTextNode->AtkResNode.SetHeight(12);

                        newTextNode->LineSpacing = 24;
                        newTextNode->AlignmentFontType = 0x15;
                        newTextNode->FontSize = 10;
                        newTextNode->TextFlags = (byte)(TextFlags.Edge);
                        newTextNode->TextFlags2 = 0;

                        newTextNode->AtkResNode.NodeId = ids[j];

                        newTextNode->AtkResNode.Color.A = 0xFF;
                        newTextNode->AtkResNode.Color.R = 0xFF;
                        newTextNode->AtkResNode.Color.G = 0xFF;
                        newTextNode->AtkResNode.Color.B = 0xFF;

                        if (lastNode->ChildNode != null)
                        {
                            lastNode = lastNode->ChildNode;
                            while (lastNode->PrevSiblingNode != null)
                            {
                                lastNode = lastNode->PrevSiblingNode;
                            }

                            newTextNode->AtkResNode.NextSiblingNode = lastNode;
                            newTextNode->AtkResNode.ParentNode = partyList->RootNode;
                            lastNode->PrevSiblingNode = (AtkResNode*)newTextNode;
                        }
                        else
                        {
                            lastNode->ChildNode = (AtkResNode*)newTextNode;
                            newTextNode->AtkResNode.ParentNode = lastNode;
                        }

                        textNode->TextColor.RGBA = 4294967295;
                        textNode->EdgeColor.RGBA = (UInt32)4286996785;
                        textNode->BackgroundColor.RGBA = (UInt32)0;

                        textNode->SetFont(FontType.MiedingerMed);
                        //node->SetAlignment(AlignmentType.Right);
                        //node->NodeFlags = NodeFlags.AnchorTop | NodeFlags.AnchorLeft | NodeFlags.Visible | NodeFlags.Enabled;

                        partyList->UldManager.UpdateDrawNodeList();
                    }
                }
                if (!visible[j])
                {
                    textNode->AtkResNode.ToggleVisibility(false);
                    return;
                }
                textNode->AtkResNode.ToggleVisibility(true);

                if (InfoManager.Configuration.ShieldDisplay == ShieldDisplay.K && InfoManager.Configuration.ShowShieldParty)
                {
                    //Plugin.Log.Debug($"{(float)actorsStats[j].Item1}, {(float)actorsStats[j].Item2}, {(float)actorsStats[j].Item1 * (float)actorsStats[j].Item2 / 100000}");
                    double shieldAmount = (float)actorsStats[j].Item1 * (float)actorsStats[j].Item2 / 100000;
                    if (shieldAmount > 100)
                        shieldAmount = Math.Round(shieldAmount, 0);
                    else if (shieldAmount > 10)
                        shieldAmount = Math.Round(shieldAmount, 1);
                    else
                        shieldAmount = Math.Round(shieldAmount, 2);
                    //Plugin.Log.Debug(shieldAmount.ToString());
                    textNode->SetText($"{shieldAmount}K");
                }
                if (InfoManager.Configuration.ShieldDisplay == ShieldDisplay.P && InfoManager.Configuration.ShowShieldParty)
                    textNode->SetText($"{actorsStats[j].Item1}%");
                if (actorsStats[j].Item1 == 0 || !InfoManager.Configuration.ShowShieldParty)
                    textNode->SetText("");
            }
        }

        private void UpdateVisibility()
        {
            var agentHUD = AgentHUD.Instance();
            short nb = agentHUD->PartyMemberCount;
            visible = [false, false, false, false, false, false, false, false];
            if (nb < 2) return;
            for (int i = 0; i < nb; i++)
            {
                visible[i] = true;
            }
        }

        public unsafe void FillActors()
        {
            actorsStats.Clear();
            var agentHUD = AgentHUD.Instance();
            short nb = agentHUD->PartyMemberCount;
            if (nb < 2 || nb > 8)
                return;
            for (int i = 0; i < nb; i++)
            {
                byte shield;
                uint maxHP;
                try
                {
                    var hudPartyMember = agentHUD->PartyMembers.GetPointer(i);
                    var obj = hudPartyMember->Object;
                    if (obj != null)
                    {
                        var cha = obj->Character;
                        shield = cha.ShieldValue;
                        maxHP = cha.MaxHealth;
                    }
                    else
                    {
                        shield = 0;
                        maxHP = 1;
                    }
                }
                catch (Exception)
                {
                    shield = 0;
                    maxHP = 1;
                    throw;
                }
                actorsStats.Add((shield, maxHP));
            }
        }

        private void CleanShieldTweaks()
        {
            var partyList = (AddonPartyList*)Plugin.GameGui.GetAddonByName("_PartyList");
            if (partyList == null) return;

            for (var j=0; j < 8; j++)
            {
                AtkTextNode* textNode = null;
                for (var i = 0; i < partyList->UldManager.NodeListCount; i++)
                {
                    if (partyList->UldManager.NodeList[i] == null) continue;
                    if (partyList->UldManager.NodeList[i]->NodeId == ids[j])
                    {
                        textNode = (AtkTextNode*)partyList->UldManager.NodeList[i];

                        break;
                    }
                }

                if (textNode == null) return;

                if (textNode->AtkResNode.PrevSiblingNode is not null)
                    textNode->AtkResNode.PrevSiblingNode->NextSiblingNode = textNode->AtkResNode.NextSiblingNode;

                if (textNode->AtkResNode.NextSiblingNode is not null)
                    textNode->AtkResNode.NextSiblingNode->PrevSiblingNode = textNode->AtkResNode.PrevSiblingNode;

                partyList->UldManager.UpdateDrawNodeList();
                textNode->AtkResNode.Destroy(false);
                IMemorySpace.Free(textNode, (ulong)sizeof(AtkTextNode));
            }
        }

    }
}
