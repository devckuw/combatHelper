using combatHelper.Utils;
using Dalamud.Configuration;
using Dalamud.Plugin;
using Dalamud.Utility;
using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;

namespace combatHelper;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool IsConfigWindowMovable { get; set; } = true;
    public bool IsMainWindowMovable { get; set; } = true;
    public bool IsHelperWIndowMovable { get; set; } = true;
    public bool SplitTimeLineAndHelper { get; set; } = false;
    public bool ShowShieldParty { get; set; } = true;
    public bool ShowTimeLine { get; set; } = true;
    public bool ShowHelper { get; set; } = true;
    public int OffsetPots { get; set; } = -10;
    public string Sound { get; set; } = "azer";
    public string AssemblyLocation {  get; set; } = "azer";
    // Colors
    public Vector4 Raid_Damage { get; set; } = new Vector4(1, 0, 0, 1);
    public Vector4 Tank_Damage { get; set; } = new Vector4(255f / 255f, 150f / 255f, 60f / 255f, 1);
    public Vector4 Positioning_Required { get; set; } = new Vector4(1, 1, 0, 1);
    public Vector4 Avoidable_AoE { get; set; } = new Vector4(0, 1, 0, 1);
    public Vector4 Debuffs { get; set; } = new Vector4(55f / 255f, 183f / 250f, 142f / 255f, 1);
    public Vector4 Targeted_AoE { get; set; } = new Vector4(94f / 255f, 163f / 255f, 254f / 255f, 1);
    public Vector4 Mechanics { get; set; } = new Vector4(204f / 255f, 135f / 255f, 254f / 255f, 1);
    // chat msg
    public ChatMode ChatMode { get; set; } = ChatMode.None;
    // shield overlay
    public ShieldDisplay ShieldDisplay = ShieldDisplay.K;
    public int OffsetShieldDisplay = 36;
    public bool ConfigShield = true;
    //custom
    public Dictionary<string, List<(ChatMode, string, bool, int)>> CustomHelper { get; set; } = new Dictionary<string, List<(ChatMode, string, bool, int)>>();


    // the below exist just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }

    public void LoadColors()
    {
        Color.Raid_Damage = Raid_Damage;
        Color.Tank_Damage = Tank_Damage;
        Color.Positioning_Required = Positioning_Required;
        Color.Avoidable_AoE = Avoidable_AoE;
        Color.Debuffs = Debuffs;
        Color.Targeted_AoE = Targeted_AoE;
        Color.Mechanics = Mechanics;
    }

    public void SetSound(string name = "", bool requiresAssembly = false)
    {
        if (name.IsNullOrEmpty())
        {
            Sound = Path.Combine(AssemblyLocation, "sound.wav");
        }
        else
        {
            if (!requiresAssembly)
            {
                Sound = name;
            }
            else
            {
                Sound = Path.Combine(AssemblyLocation, name);
            }
        }
        Save();
    }
}
