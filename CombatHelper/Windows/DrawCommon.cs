using combatHelper.Fights;
using combatHelper.Utils;
using Dalamud.Interface.Components;
using Dalamud.Interface.Utility;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dalamud.Interface.Windowing.Window;

namespace combatHelper.Windows
{
    internal class DrawCommon
    {
        public static void MenuBar()
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("Fight"))
                {
                    if (ImGui.MenuItem("None")) { TimeManager.Instance.ResetEvents(); InfoManager.fightState = FightState.None; }
                    if (ImGui.MenuItem("M5S")) { InfoManager.fightState = FightState.M5S; InfoManager.fight = new M5S(); }
                    if (ImGui.MenuItem("M6S")) { InfoManager.fightState = FightState.M6S; InfoManager.fight = new M6S(); }
                    if (ImGui.MenuItem("M7S")) { InfoManager.fightState = FightState.M7S; InfoManager.fight = new M7S(); }
                    if (ImGui.MenuItem("M8S")) { InfoManager.fightState = FightState.M8S; InfoManager.fight = new M8S(); }
                    if (ImGui.MenuItem("Custom")) { InfoManager.fightState = FightState.Custom; InfoManager.fight = new Custom(); }
                    if (ImGui.BeginMenu("Old"))
                    {
                        if (ImGui.MenuItem("M1S")) { InfoManager.fightState = FightState.M1S; InfoManager.fight = new M1S(); }
                        if (ImGui.MenuItem("M2S")) { InfoManager.fightState = FightState.M2S; InfoManager.fight = new M2S(); }
                        if (ImGui.MenuItem("M3S")) { InfoManager.fightState = FightState.M3S; InfoManager.fight = new M3S(); }
                        if (ImGui.MenuItem("M4S")) { InfoManager.fightState = FightState.M4S; InfoManager.fight = new M4S(); }
                        if (ImGui.MenuItem("CloudOfDarkness")) { InfoManager.fightState = FightState.CloudOfDarkness; InfoManager.fight = new CloudOfDarkness(); }
                        ImGui.EndMenu();
                    }
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Pots"))
                {
                    if (ImGui.MenuItem("None")) { InfoManager.nbPots = NbPots.None; }
                    if (ImGui.MenuItem("0/6")) { InfoManager.nbPots = NbPots.Two_Pots; }
                    if (ImGui.MenuItem("2/8")) { InfoManager.nbPots = NbPots.Two_Pots_Bard; }
                    if (ImGui.MenuItem("2/10")) { InfoManager.nbPots = NbPots.Two_Ten; }
                    if (ImGui.MenuItem("0/5/10")) { InfoManager.nbPots = NbPots.Three_Pots; }
                    if (ImGui.MenuItem("0/6/12")) { InfoManager.nbPots = NbPots.Three_twoPots; }
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }
        }

        public static List<TitleBarButton> CreateTitleBarButtons()
        {
            List<TitleBarButton> titleBarButtons = new()
            {
                new TitleBarButton()
                {
                    Icon = Dalamud.Interface.FontAwesomeIcon.Cog,
                    Click = (msg) =>
                    {
                        InfoManager.plugin.ToggleConfigUI();
                    },
                    IconOffset = new(2,1),
                    ShowTooltip = () =>
                    {
                        ImGui.BeginTooltip();
                        ImGui.Text("Open Settings");
                        ImGui.EndTooltip();
                    }
                }
            };

            return titleBarButtons;
        }

        public static void Helper(string help)
        {
            ImGui.SameLine();
            ImGuiComponents.HelpMarker(help);
        }

        public static void IsHovered(string info)
        {
            if (ImGui.IsItemHovered()) { ImGui.SetTooltip(info); }
        }
    }
}
