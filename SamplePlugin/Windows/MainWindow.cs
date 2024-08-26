using System;
using System.IO;
using System.Media;
using System.Numerics;
using combatHelper.Fights;
using combatHelper.Utils;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace combatHelper.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin Plugin;
    /*private FightState fightState;
    private NbPots nbPots;
    private Fight fight;*/
    private DateTime startTimer;
    private bool isStarted = false;
    private bool inCombat = false;
    private bool isPotTwoUsed = false; 
    private bool isPotThreeUsed = false;

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin)
        : base("Savage Helper##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.MenuBar)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            //MinimumSize = new Vector2(575, 225),
            MinimumSize = new Vector2(300, 225),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        Plugin = plugin;
        Plugin.Framework.Update += OnUpdate;
    }

    public void Dispose()
    {
        Plugin.Framework.Update -= OnUpdate;
    }

    private void OnUpdate(IFramework framework)
    {
        var inCombat = Plugin.Condition[ConditionFlag.InCombat];

        if (!inCombat)
        {
            foreach (var actor in Plugin.PartyList)
            {
                if (actor.GameObject is not ICharacter character ||
                        (character.StatusFlags & StatusFlags.InCombat) == 0) continue;
                inCombat = true;
                break;
            }
        }

        if (inCombat)
        {
            if (!isStarted)
            {
                isStarted = true;
                startTimer = DateTime.Now;
            }
            var combatDuration = (DateTime.Now - startTimer).Seconds;
            var offset = InfoManager.Configuration.OffsetPots;
            switch (InfoManager.nbPots)
            {
                case NbPots.None:
                    break;
                case NbPots.Two_Pots:
                    if (combatDuration >= 6 * 60 + offset && !isPotTwoUsed) { InfoManager.soundPlayer.Play(); isPotTwoUsed = true; }
                    break;
                case NbPots.Two_Pots_Bard:
                    if (combatDuration >= 2 * 60 + offset && !isPotTwoUsed) { InfoManager.soundPlayer.Play(); isPotTwoUsed = true; }
                    if (combatDuration >= 8 * 60 + offset && !isPotTwoUsed) { InfoManager.soundPlayer.Play(); isPotThreeUsed = true; }
                    break;
                case NbPots.Three_Pots:
                    if (combatDuration >= 5 * 60 + offset && !isPotTwoUsed) { InfoManager.soundPlayer.Play(); isPotTwoUsed = true; }
                    if (combatDuration >= 10 * 60 + offset && !isPotTwoUsed) { InfoManager.soundPlayer.Play(); isPotThreeUsed = true; }
                    break;
                case NbPots.Three_twoPots:
                    if (combatDuration >= 6 * 60 + offset && !isPotTwoUsed) { InfoManager.soundPlayer.Play(); isPotTwoUsed = true; }
                    if (combatDuration >= 12 * 60 + offset && !isPotTwoUsed) { InfoManager.soundPlayer.Play(); isPotThreeUsed = true; }
                    break;
            }
        }
        else
        {
            isPotTwoUsed = false;
            isPotThreeUsed = false;
            isStarted = false;
        }
    }

    public override void PreDraw()
    {
        // Flags must be added or removed before Draw() is being called, or they won't apply
        if (InfoManager.Configuration.IsMainWindowMovable)
        {
            Flags &= ~ImGuiWindowFlags.NoMove;
        }
        else
        {
            Flags |= ImGuiWindowFlags.NoMove;
        }
    }

    public override void Draw()
    {
        if (ImGui.BeginMenuBar())
        {
            if (ImGui.BeginMenu("Fight"))
            {
                if (ImGui.MenuItem("None")) { InfoManager.fightState = FightState.None; }
                if (ImGui.MenuItem("M1S")) { InfoManager.fightState = FightState.M1S; InfoManager.fight = new M1S(); }
                if (ImGui.MenuItem("M2S")) { InfoManager.fightState = FightState.M2S; InfoManager.fight = new M2S(); }
                if (ImGui.MenuItem("M3S")) { InfoManager.fightState = FightState.M3S; InfoManager.fight = new M3S(); }
                if (ImGui.MenuItem("M4S")) { InfoManager.fightState = FightState.M4S; InfoManager.fight = new M4S(); }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Pots"))
            {
                if (ImGui.MenuItem("None")) { InfoManager.nbPots = NbPots.None; }
                if (ImGui.MenuItem("0/6")) { InfoManager.nbPots = NbPots.Two_Pots; }
                if (ImGui.MenuItem("2/8")) { InfoManager.nbPots = NbPots.Two_Pots_Bard; }
                if (ImGui.MenuItem("0/5/10")) { InfoManager.nbPots = NbPots.Three_Pots; }
                if (ImGui.MenuItem("0/6/12")) { InfoManager.nbPots = NbPots.Three_twoPots; }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Settings"))
            {
                Plugin.ToggleConfigUI();
                ImGui.EndMenu();
            }
            ImGui.EndMenuBar();
        }

        switch (InfoManager.fightState)
        {
            case FightState.None:
                ImGui.Text("Choose a fight.");
                if (ImGui.Button("testsound"))
                {
                    InfoManager.soundPlayer.Play();
                }
                break;
            default:
                bool needSameLine = false;
                if (InfoManager.Configuration.ShowTimeLine)
                {
                    int seconds = 0;
                    if (isStarted)
                    {
                        var combatDuration = DateTime.Now - startTimer;
                        seconds = combatDuration.Seconds;
                    }
                    ImGui.BeginChild("time line", new Vector2(350, 200));
                    InfoManager.fight.Draw(seconds);
                    ImGui.EndChild();
                    ImGui.SameLine();
                    needSameLine = true;
                }
                if (InfoManager.Configuration.ShowHelper)
                {
                    if (needSameLine) { ImGui.SameLine(); }
                    ImGui.BeginChild("mech helper");
                    InfoManager.fight.DrawHelper();
                    ImGui.EndChild();
                }
                break;
        }
    }
}
