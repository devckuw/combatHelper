using System;
using System.IO;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Interface.ImGuiFileDialog;
using ImGuiNET;

namespace combatHelper.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;
    private Vector4 testcolor = new Vector4(0f, 0f, 0f, 1f);
    private string filePicked = "";
    private bool isFileDialogOpen = false;
    FileDialogManager manager = new FileDialogManager();
    Plugin plugin;

    // We give this window a constant ID using ###
    // This allows for labels being dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin) : base("A Wonderful Configuration Window###With a constant ID")
    {
        Flags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        //Size = new Vector2(232, 135);
        Size = new Vector2(300, 300);
        SizeCondition = ImGuiCond.Always;

        Configuration = plugin.Configuration;
        this.plugin = plugin;
    }

    public void Dispose() { }

    public override void PreDraw()
    {
        // Flags must be added or removed before Draw() is being called, or they won't apply
        if (Configuration.IsConfigWindowMovable)
        {
            Flags &= ~ImGuiWindowFlags.NoMove;
        }
        else
        {
            Flags |= ImGuiWindowFlags.NoMove;
        }
    }

    public void callbackfile(bool b, string s)
    {
        filePicked = s;
        Configuration.SetSound(s);
        plugin.UpdateSound();
        isFileDialogOpen = false;
    }

    public void CreateFileDiag()
    {
        manager.OpenFileDialog("test", ".*", callbackfile);
    }

    public override void Draw()
    {
        ImGui.BeginTabBar("tabsConfig");
        if (ImGui.BeginTabItem("General"))
        {
            // can't ref a property, so use a local copy
            var configValue = Configuration.SomePropertyToBeSavedAndWithADefault;
            if (ImGui.Checkbox("Random Config Bool", ref configValue))
            {
                Configuration.SomePropertyToBeSavedAndWithADefault = configValue;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                Configuration.Save();
            }

            var movable = Configuration.IsConfigWindowMovable;
            if (ImGui.Checkbox("Movable Config Window", ref movable))
            {
                Configuration.IsConfigWindowMovable = movable;
                Configuration.Save();
            }

            var offsetPots = Configuration.OffsetPots;
            if (ImGui.InputInt("Offset Pots", ref offsetPots, 1, 1))
            {
                if (offsetPots < -20) { offsetPots = -20; }
                if (offsetPots > 20) { offsetPots = 20; }
                Configuration.OffsetPots = offsetPots;
                Configuration.Save();
            }
            ImGui.EndTabItem();
        }
        if (ImGui.BeginTabItem("Colors"))
        {
            var raid = Configuration.Raid_Damage;
            if (ImGui.ColorEdit4("Raid_Damage", ref raid, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Raid_Damage = raid;
                Configuration.Save();
                Fights.Color.Raid_Damage = raid;
            }
            

            var tank = Configuration.Tank_Damage;
            if (ImGui.ColorEdit4("Tank_Damage", ref tank, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Tank_Damage = tank;
                Configuration.Save();
                Fights.Color.Tank_Damage = tank;
            }
            

            var pos = Configuration.Positioning_Required;
            if (ImGui.ColorEdit4("Positioning_Required", ref pos, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Positioning_Required = pos;
                Configuration.Save();
                Fights.Color.Positioning_Required = pos;
            }

            var avoid = Configuration.Avoidable_AoE;
            if (ImGui.ColorEdit4("Avoidable_AoE", ref avoid, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Avoidable_AoE = avoid;
                Configuration.Save();
                Fights.Color.Avoidable_AoE = avoid;
            }

            var debuff = Configuration.Debuffs;
            if (ImGui.ColorEdit4("Debuffs", ref debuff, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Debuffs = debuff;
                Configuration.Save();
                Fights.Color.Debuffs = debuff;
            }

            var target = Configuration.Targeted_AoE;
            if (ImGui.ColorEdit4("Targeted_AoE", ref target, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Targeted_AoE = target;
                Configuration.Save();
                Fights.Color.Targeted_AoE = target;
            }

            var mech = Configuration.Mechanics;
            if (ImGui.ColorEdit4("Mechanics", ref mech, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Mechanics = mech;
                Configuration.Save();
                Fights.Color.Mechanics = mech;
            }

            if (ImGui.Button("Reset Colors"))
            {
                Configuration.Raid_Damage = Fights.Color.Red;
                Configuration.Tank_Damage = Fights.Color.Orange;
                Configuration.Positioning_Required = Fights.Color.Yellow;
                Configuration.Avoidable_AoE = Fights.Color.Green;
                Configuration.Debuffs = Fights.Color.Cyan;
                Configuration.Targeted_AoE = Fights.Color.Blue;
                Configuration.Mechanics = Fights.Color.Purple;
                Configuration.Save();
                Configuration.LoadColors();
            }
            
            ImGui.EndTabItem();
        }
        if (ImGui.BeginTabItem("Sound"))
        {
            if (ImGui.Button("Select Sound"))
            {
                if (!isFileDialogOpen)
                {
                    isFileDialogOpen = true;
                    manager.OpenFileDialog("Sound Picker", "Sound .mp3 .wav{.mp3,.wav}", callbackfile);
                }
            }
            if (isFileDialogOpen)
            {
                manager.Draw();
            }
            ImGui.Text(filePicked);
            if (ImGui.Button("Reset Default"))
            {
                Configuration.SetSound();
                plugin.UpdateSound();
            }
            ImGui.EndTabItem(); 
        }
        ImGui.EndTabBar();
    }
}
