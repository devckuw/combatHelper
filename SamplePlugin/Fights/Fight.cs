using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using System.IO;

namespace combatHelper.Fights
{
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
        // = 0b_0100_0000,  // 64
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
        Three_Pots,
        Three_twoPots
    }

    public class DataFrameManager
    {
        public static List<(int, int, string, DmgType)> ProccessDF(string path)
        {
            DataFrame df = DataFrame.LoadCsv(path);

            var lines = new List<(int, int, string, DmgType)>();
            var nbRows = df.Rows.Count;
            
            for ( int i = 0; i < nbRows; i++)
            {
                lines.Add((Int32.Parse(df["Cast"][i].ToString()), Int32.Parse(df["Effect"][i].ToString()), df["Description"][i].ToString(), (DmgType)DmgFlags(df, i)));
            }
            return lines;
        }

        public static int DmgFlags(DataFrame df, int row)
        {
            int mechType = Int32.Parse(df["Raid_Damage"][row].ToString()) * 1;
            mechType += Int32.Parse(df["Tank_Damage"][row].ToString()) * 2;
            mechType += Int32.Parse(df["Positioning_Required"][row].ToString()) * 4;
            mechType += Int32.Parse(df["Avoidable_AoE"][row].ToString()) * 8;
            mechType += Int32.Parse(df["Targeted_AoE"][row].ToString()) * 16;
            mechType += Int32.Parse(df["Mechanics"][row].ToString()) * 32;
            return mechType;
        }
    }

    public abstract class Fight
    {
        protected List<(int, int, string, DmgType)> lines;
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
                if (currentTime <= line.Item1 && line.Item1 <= currentTime + 30)
                {
                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();
                    ImGui.Text(line.Item1.ToString());
                    ImGui.TableNextColumn();
                    ImGui.Text(line.Item2.ToString());
                    ImGui.TableNextColumn();
                    ImGui.Text(line.Item3);
                    ImGui.TableNextColumn();
                    ImGui.Text(line.Item4.ToString());
                    //ImGui.Text(line.Item1 + " " + line.Item2 + " " + line.Item3 + " " + line.Item4);
                }
            }
            ImGui.EndTable();
        }
        public abstract void DrawHelper();
    }
}
