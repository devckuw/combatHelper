using System;
using System.IO;
using System.Media;
using System.Numerics;
using combatHelper.Fights;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace combatHelper.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin Plugin;
    private FightState fightState;
    private NbPots nbPots;
    private Fight fight;

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin)
        : base("Savage Helper##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.MenuBar)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(575, 225),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        Plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        var path = Path.Combine(Plugin.PluginInterface.AssemblyLocation.Directory?.FullName!, "M1S.csv");
        if (ImGui.BeginMenuBar())
        {
            if (ImGui.BeginMenu("Fight"))
            {
                if (ImGui.MenuItem("None")) { fightState = FightState.None; }
                if (ImGui.MenuItem("M1S")) { fightState = FightState.M1S; fight = new M1S(path); }
                if (ImGui.MenuItem("M2S")) { fightState = FightState.M2S; fight = new M2S(); }
                if (ImGui.MenuItem("M3S")) { fightState = FightState.M3S; fight = new M3S(); }
                if (ImGui.MenuItem("M4S")) { fightState = FightState.M4S; fight = new M4S(); }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Pots"))
            {
                if (ImGui.MenuItem("None")) { nbPots = NbPots.None; }
                if (ImGui.MenuItem("0/6")) { nbPots = NbPots.Two_Pots; }
                if (ImGui.MenuItem("0/5/10")) { nbPots = NbPots.Three_Pots; }
                if (ImGui.MenuItem("0/6/12")) { nbPots = NbPots.Three_twoPots; }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Settings"))
            {
                Plugin.ToggleConfigUI();
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
                if (ImGui.Button("testsound"))
                {
                    var pathsound = Path.Combine(Plugin.PluginInterface.AssemblyLocation.Directory?.FullName!, "sound.wav");
                    using (var sound = new SoundPlayer(pathsound))
                    {
                        sound.Play();
                    }
                }
                break;
            default:
                ImGui.BeginChild("time line", new Vector2(350,200));
                fight.Draw(3);
                ImGui.EndChild();
                ImGui.SameLine();
                ImGui.BeginChild("mech helper");
                fight.DrawHelper();
                ImGui.EndChild();
                break;
        }
    }
}
