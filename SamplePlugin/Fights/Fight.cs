using Microsoft.Data.Analysis;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using System.IO;

namespace combatHelper.Fights
{
    public static class Extensions
    {
        public static List<(string, Vector4)> ToList(this DmgType type)
        {
            var list = new List<(string, Vector4)>();
            if (type == DmgType.Raid_Damage) { list.Add(("Raid_Damage", Color.Red)); }
            if (type == DmgType.Tank_Damage) { list.Add(("Tank_Damage", Color.Orange)); }
            if (type == DmgType.Positioning_Required) { list.Add(("Positioning_Required", Color.Yellow)); }
            if (type == DmgType.Avoidable_AoE) { list.Add(("Avoidable_AoE", Color.Green)); }
            if (type == DmgType.Targeted_AoE) { list.Add(("Targeted_AoE", Color.Blue)); }
            if (type == DmgType.Mechanics) { list.Add(("Mechanics", Color.Purple)); }
            if (type == DmgType.Debuffs) { list.Add(("Debuffs", Color.Cyan)); }
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
        M4S = 4
    }

    public enum NbPots
    {
        None,
        Two_Pots,
        Two_Pots_Bard,
        Three_Pots,
        Three_twoPots
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
    }

    public class DataFrameManager
    {
        public static List<(int, int, string, List<(string, Vector4)>)> ProccessDF(string path)
        {
            DataFrame df = DataFrame.LoadCsv(path);

            var lines = new List<(int, int, string, List<(string, Vector4)>)>();
            var nbRows = df.Rows.Count;
            
            for ( int i = 0; i < nbRows; i++)
            {
                lines.Add((Int32.Parse(df["Cast"][i].ToString()), Int32.Parse(df["Effect"][i].ToString()), df["Description"][i].ToString(), DmgFlags(df, i).ToList()));
            }
            return lines;
        }

        public static DmgType DmgFlags(DataFrame df, int row)
        {
            int mechType = Int32.Parse(df["Raid_Damage"][row].ToString()) * 1;
            mechType += Int32.Parse(df["Tank_Damage"][row].ToString()) * 2;
            mechType += Int32.Parse(df["Positioning_Required"][row].ToString()) * 4;
            mechType += Int32.Parse(df["Avoidable_AoE"][row].ToString()) * 8;
            mechType += Int32.Parse(df["Targeted_AoE"][row].ToString()) * 16;
            mechType += Int32.Parse(df["Mechanics"][row].ToString()) * 32;
            mechType += Int32.Parse(df["Debuffs"][row].ToString()) * 64;
            return (DmgType)mechType;
        }
    }

    public abstract class Fight
    {
        protected List<(int, int, string, List<(string, Vector4)>)> lines;
        public Fight() { }

        public void Draw(int currentTime)
        {
            if (currentTime < 0) { return; }
            ImGui.BeginTable("timelinetable", 4, ImGuiTableFlags.SizingStretchProp);
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.Text("Cast");
            ImGui.TableNextColumn();
            ImGui.Text("Effect");
            ImGui.TableNextColumn();
            ImGui.Text("Description");
            ImGui.TableNextColumn();
            ImGui.Text("Type");
            foreach (var line in lines)
            {
                if ((currentTime <= line.Item1 && line.Item1 <= currentTime + 20) || (line.Item1 <= currentTime && currentTime <= line.Item2))
                {
                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    ImGui.Text((line.Item1 - currentTime).ToString());
                    ImGui.TableNextColumn();
                    ImGui.Text((line.Item2 - currentTime).ToString());
                    ImGui.TableNextColumn();
                    ImGui.Text(line.Item3);
                    ImGui.TableNextColumn();
                    var list = line.Item4;
                    var listLength = list.Count();
                    for ( var i = 0; i < listLength; i++ )
                    {
                        ImGui.TextColored(list[i].Item2, list[i].Item1);
                        if ( i < listLength - 1 ) { ImGui.SameLine(); }
                    }
                }
            }
            ImGui.EndTable();
        }
        public abstract void DrawHelper();
    }
}
