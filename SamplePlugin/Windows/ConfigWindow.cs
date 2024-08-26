using System;
using System.Media;
using System.IO;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Interface.ImGuiFileDialog;
using ImGuiNET;
using combatHelper.Utils;

namespace combatHelper.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;

    private FileDialogService fileDialogService = new FileDialogService();

    // We give this window a constant ID using ###
    // This allows for labels being dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow() : base("A Wonderful Configuration Window###With a constant ID")
    {
        Flags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(230, 300),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        SizeCondition = ImGuiCond.Always;

        Configuration = InfoManager.Configuration;
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

    public override void Draw()
    {
        ImGui.BeginTabBar("tabsConfig");
        if (ImGui.BeginTabItem("General"))
        {
            // can't ref a property, so use a local copy
            var configValue = Configuration.ShowTimeLine;
            if (ImGui.Checkbox("Show Time Line", ref configValue))
            {
                Configuration.ShowTimeLine = configValue;
                Configuration.Save();
                InfoManager.UpdateSplitToggle();
            }
            configValue = Configuration.ShowHelper;
            if (ImGui.Checkbox("Show Helper", ref configValue))
            {
                Configuration.ShowHelper = configValue;
                Configuration.Save();
                InfoManager.UpdateSplitToggle();
            }
            configValue = Configuration.SplitTimeLineAndHelper;
            if (ImGui.Checkbox("Split Helper & TimeLine", ref configValue))
            {
                Configuration.SplitTimeLineAndHelper = configValue;
                Configuration.Save();
                InfoManager.UpdateSplitToggle();
            }
            var movable = Configuration.IsMainWindowMovable;
            if (ImGui.Checkbox("Movable Main Window", ref movable))
            {
                Configuration.IsMainWindowMovable = movable;
                Configuration.Save();
            }
            movable = Configuration.IsConfigWindowMovable;
            if (ImGui.Checkbox("Movable Config Window", ref movable))
            {
                Configuration.IsConfigWindowMovable = movable;
                Configuration.Save();
            }
            movable = Configuration.IsHelperWIndowMovable;
            if (ImGui.Checkbox("Movable Split Helper Window", ref movable))
            {
                Configuration.IsHelperWIndowMovable = movable;
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
                Color.Raid_Damage = raid;
            }
            

            var tank = Configuration.Tank_Damage;
            if (ImGui.ColorEdit4("Tank_Damage", ref tank, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Tank_Damage = tank;
                Configuration.Save();
                Color.Tank_Damage = tank;
            }
            

            var pos = Configuration.Positioning_Required;
            if (ImGui.ColorEdit4("Positioning_Required", ref pos, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Positioning_Required = pos;
                Configuration.Save();
                Color.Positioning_Required = pos;
            }

            var avoid = Configuration.Avoidable_AoE;
            if (ImGui.ColorEdit4("Avoidable_AoE", ref avoid, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Avoidable_AoE = avoid;
                Configuration.Save();
                Color.Avoidable_AoE = avoid;
            }

            var debuff = Configuration.Debuffs;
            if (ImGui.ColorEdit4("Debuffs", ref debuff, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Debuffs = debuff;
                Configuration.Save();
                Color.Debuffs = debuff;
            }

            var target = Configuration.Targeted_AoE;
            if (ImGui.ColorEdit4("Targeted_AoE", ref target, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Targeted_AoE = target;
                Configuration.Save();
                Color.Targeted_AoE = target;
            }

            var mech = Configuration.Mechanics;
            if (ImGui.ColorEdit4("Mechanics", ref mech, ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoInputs))
            {
                Configuration.Mechanics = mech;
                Configuration.Save();
                Color.Mechanics = mech;
            }

            if (ImGui.Button("Reset Colors"))
            {
                Configuration.Raid_Damage = Color.Red;
                Configuration.Tank_Damage = Color.Orange;
                Configuration.Positioning_Required = Color.Yellow;
                Configuration.Avoidable_AoE = Color.Green;
                Configuration.Debuffs = Color.Cyan;
                Configuration.Targeted_AoE = Color.Blue;
                Configuration.Mechanics = Color.Purple;
                Configuration.Save();
                Configuration.LoadColors();
            }
            
            ImGui.EndTabItem();
        }
        if (ImGui.BeginTabItem("Sound"))
        {
            if (ImGui.Button("Select Sound"))
            {
                if (!fileDialogService.isOpen)
                {
                    fileDialogService.isOpen = true;
                    fileDialogService.manager.OpenFileDialog("Sound Picker", "Sound .mp3 .wav{.mp3,.wav}", fileDialogService.callbackfile);
                }
            }
            if (fileDialogService.isOpen)
            {
                fileDialogService.manager.Draw();
            }
            ImGui.Text(fileDialogService.filePicked);
            if (ImGui.Button("Reset Default"))
            {
                fileDialogService.filePicked = "Reseted to default";
                Configuration.SetSound();
                InfoManager.UpdateSound();
            }
            if (ImGui.Button("Test Sound"))
            {
                InfoManager.soundPlayer.Play();
            }
            ImGui.EndTabItem(); 
        }
        if (ImGui.BeginTabItem("Chat Msg"))
        {
            if (ImGui.BeginCombo("Chat mode", Configuration.ChatMode.ToString()))
            {
                if (ImGui.Selectable("None"))
                {
                    Configuration.ChatMode = Utils.ChatMode.None;
                    Configuration.Save();
                }
                if (ImGui.Selectable("Echo"))
                {
                    Configuration.ChatMode = Utils.ChatMode.Echo;
                    Configuration.Save();
                }
                if (ImGui.Selectable("Party"))
                {
                    Configuration.ChatMode = Utils.ChatMode.Party;
                    Configuration.Save();
                }
                if (ImGui.Selectable("Alliance"))
                {
                    Configuration.ChatMode = Utils.ChatMode.Alliance;
                    Configuration.Save();
                }
            }
            ImGui.EndTabItem();
        }
        ImGui.EndTabBar();
    }
}
