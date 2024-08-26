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
    public MainWindow()
        : base("Savage Helper##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.MenuBar)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            //MinimumSize = new Vector2(575, 225),
            MinimumSize = new Vector2(300, 225),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

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
        DrawCommon.MenuBar();

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
                if (InfoManager.Configuration.ShowTimeLine && InfoManager.fight.lines != null)
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
                    //ImGui.SameLine();
                    needSameLine = true;
                }
                if (InfoManager.Configuration.ShowHelper && !InfoManager.isSplitEnable)
                {
                    if (needSameLine) { ImGui.SameLine(); }
                    ImGui.BeginChild("mech helper");
                    InfoManager.fight.DrawHelper();
                    ImGui.EndChild();
                }
                break;
        }
    }

    public override void OnClose()
    {
        if (InfoManager.isSplitOpen)
        {
            InfoManager.plugin.ToggleSplitHelperUI();
        }
    }
}
