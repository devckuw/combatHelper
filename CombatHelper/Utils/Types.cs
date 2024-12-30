using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace combatHelper.Utils
{
    public static class Extensions
    {
        public static List<(string, Vector4)> ToList(this DmgType type)
        {
            var list = new List<(string, Vector4)>();
            if (type == DmgType.Raid_Damage) { list.Add(("Raid_Damage", Color.Raid_Damage)); }
            if (type == DmgType.Tank_Damage) { list.Add(("Tank_Damage", Color.Tank_Damage)); }
            if (type == DmgType.Positioning_Required) { list.Add(("Positioning_Required", Color.Positioning_Required)); }
            if (type == DmgType.Avoidable_AoE) { list.Add(("Avoidable_AoE", Color.Avoidable_AoE)); }
            if (type == DmgType.Targeted_AoE) { list.Add(("Targeted_AoE", Color.Targeted_AoE)); }
            if (type == DmgType.Mechanics) { list.Add(("Mechanics", Color.Mechanics)); }
            if (type == DmgType.Debuffs) { list.Add(("Debuffs", Color.Debuffs)); }
            return list;
        }
    }

    [Flags]
    public enum DmgType
    {
        None = 0b_0000_0000,  // 0
        Raid_Damage = 0b_0000_0001,  // 1
        Tank_Damage = 0b_0000_0010,  // 2
        Positioning_Required = 0b_0000_0100,  // 4
        Avoidable_AoE = 0b_0000_1000,  // 8
        Targeted_AoE = 0b_0001_0000,  // 16
        Mechanics = 0b_0010_0000,  // 32
        Debuffs = 0b_0100_0000,  // 64
        // = 0b_1000_0000,  // 128
        //rw_mech = Raid_Damage | Mechanics
    }

    public enum FightState : ushort
    {
        None = 0,
        M1S = 1,
        M2S = 2,
        M3S = 3,
        M4S = 4,
        CloudOfDarkness = 5,
        Custom = 6
    }

    public enum NbPots
    {
        None,
        Two_Pots,
        Two_Pots_Bard,
        Three_Pots,
        Three_twoPots
    }

    public enum ShieldDisplay
    {
        K,
        P
    }

    public static class Color
    {
        public static readonly Vector4 Red = new Vector4(1, 0, 0, 1);
        public static readonly Vector4 Orange = new Vector4(255f / 255f, 150f / 255f, 60f / 255f, 1);
        public static readonly Vector4 Yellow = new Vector4(1, 1, 0, 1);
        public static readonly Vector4 Green = new Vector4(0, 1, 0, 1);
        public static readonly Vector4 Cyan = new Vector4(55f / 255f, 183f / 250f, 142f / 255f, 1);
        public static readonly Vector4 Blue = new Vector4(94f / 255f, 163f / 255f, 254f / 255f, 1);
        public static readonly Vector4 Purple = new Vector4(204f / 255f, 135f / 255f, 254f / 255f, 1);
        public static Vector4 Raid_Damage { get; set; } = new Vector4(1, 0, 0, 1);
        public static Vector4 Tank_Damage { get; set; } = new Vector4(255f / 255f, 150f / 255f, 60f / 255f, 1);
        public static Vector4 Positioning_Required { get; set; } = new Vector4(1, 1, 0, 1);
        public static Vector4 Avoidable_AoE { get; set; } = new Vector4(0, 1, 0, 1);
        public static Vector4 Debuffs { get; set; } = new Vector4(55f / 255f, 183f / 250f, 142f / 255f, 1);
        public static Vector4 Targeted_AoE { get; set; } = new Vector4(94f / 255f, 163f / 255f, 254f / 255f, 1);
        public static Vector4 Mechanics { get; set; } = new Vector4(204f / 255f, 135f / 255f, 254f / 255f, 1);
    }
}
