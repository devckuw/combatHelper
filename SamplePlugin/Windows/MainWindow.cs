using System;
using System.Numerics;
using combatHelper.Fights;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace combatHelper.Windows;

public enum FightState : ushort
{
    None = 0,
    M1S = 1,
    M2S = 2,
    M3S = 3,
    M4S = 4
}

public class MainWindow : Window, IDisposable
{
    private Plugin Plugin;
    private FightState fightState;
    private Fight fight;

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin)
        : base("Savage Helper##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.MenuBar)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        Plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        if (ImGui.BeginMenuBar())
        {
            if (ImGui.BeginMenu("Fight"))
            {
                if (ImGui.MenuItem("M1S")) { fightState = FightState.M1S; fight = new M1S(); }
                if (ImGui.MenuItem("M2S")) { fightState = FightState.M2S; fight = new M2S(); }
                if (ImGui.MenuItem("M3S")) { fightState = FightState.M3S; fight = new M3S(); }
                if (ImGui.MenuItem("M4S")) { fightState = FightState.M4S; fight = new M4S(); }
                ImGui.EndMenu();
            }
            ImGui.EndMenuBar();
        }

        //ImGui.Text($"The random config bool is {Plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");
        //if (fightState == FightState.None) { ImGui.Text("Choose a fight."); } else { ImGui.Text(fightState.ToString() + " Helper."); }
        switch (fightState)
        {
            case FightState.None:
                ImGui.Text("Choose a fight.");
                break;
            default:
                fight.Draw();
                break;
        }
    }
}
