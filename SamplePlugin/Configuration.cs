using Dalamud.Configuration;
using Dalamud.Plugin;
using Dalamud.Utility;
using System;
using System.IO;
using System.Numerics;

namespace combatHelper;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool IsConfigWindowMovable { get; set; } = true;
    public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;
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

    // the below exist just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }

    public void LoadColors()
    {
        Fights.Color.Raid_Damage = Raid_Damage;
        Fights.Color.Tank_Damage = Tank_Damage;
        Fights.Color.Positioning_Required = Positioning_Required;
        Fights.Color.Avoidable_AoE = Avoidable_AoE;
        Fights.Color.Debuffs = Debuffs;
        Fights.Color.Targeted_AoE = Targeted_AoE;
        Fights.Color.Mechanics = Mechanics;
    }

    public void SetSound(string name = null, bool requiresAssembly = false)
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
