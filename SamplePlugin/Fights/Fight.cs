using Microsoft.Data.Analysis;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using System.IO;
using combatHelper.Utils;

namespace combatHelper.Fights
{
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
        protected Configuration configuration;
        public Fight() { }

        public void Draw(int currentTime)
        {
            if (currentTime < 0) { return; }
            ImGuiTableFlags flag = ImGuiTableFlags.Hideable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Resizable | ImGuiTableFlags.SizingStretchProp;
            ImGui.BeginTable("timelinetable", 4, flag);
            ImGui.TableSetupColumn("Cast");
            ImGui.TableSetupColumn("Effect");
            ImGui.TableSetupColumn("Description");
            ImGui.TableSetupColumn("Type");
            ImGui.TableHeadersRow();
            ImGui.TableNextRow();
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
