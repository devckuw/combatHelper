using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using combatHelper.Utils;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.Interop;
using ImGuiNET;
using static Apache.Arrow.Memory.MemoryAllocator;

namespace combatHelper.Windows
{
    internal class ShieldOverlayWindow : Window, IDisposable
    {
        private List<(byte, uint)> actorsStats;
        private unsafe AgentHUD* agentHUD;
        public ShieldOverlayWindow() : base("Shield##shield window")
        {
            Plugin.Framework.Update += OnUpdate;
            actorsStats = new List<(byte, uint)>();
        }

        public void Dispose()
        {
            Plugin.Framework.Update -= OnUpdate;
        }

        public unsafe void FillActors()
        {
            agentHUD = AgentHUD.Instance();
            short nb = agentHUD->PartyMemberCount;
            if (nb < 2 || nb > 8)
                return;
            for (int i = 0; i < nb; i++)
            {
                byte shield;
                uint maxHP;
                try
                {
                    var hudPartyMember = agentHUD->PartyMembers.GetPointer(i);
                    var obj = hudPartyMember->Object;
                    if (obj != null)
                    {
                        var cha = obj->Character;
                        shield = cha.ShieldValue;
                        maxHP = cha.MaxHealth;
                    }
                    else
                    {
                        shield = 0;
                        maxHP = 1;
                    }
                }
                catch (Exception)
                {
                    shield = 0;
                    maxHP = 1;
                    throw;
                }
                actorsStats.Add((shield, maxHP));
            }
        }

        public void OnUpdate(IFramework framework)
        {
            actorsStats.Clear();
            try
            {
                FillActors();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public override void PreDraw()
        {
            if (InfoManager.Configuration.ConfigShield)
            {
                Flags = ImGuiWindowFlags.NoTitleBar;
            }
            else
            {
                Flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoMouseInputs | ImGuiWindowFlags.NoResize;
            }
        }

        public override void Draw()
        {
            int offset = InfoManager.Configuration.OffsetShieldDisplay;
            for (int i = 0;i < actorsStats.Count;i++)
            {
                ImGui.BeginChild($"shield##{i}", new Vector2(45, offset), false, ImGuiWindowFlags.NoInputs);
                if (InfoManager.Configuration.ShieldDisplay == ShieldDisplay.K)
                {
                    double shieldAmount = (float)actorsStats[i].Item1 * (float)actorsStats[i].Item2 / 100000;
                    if (shieldAmount < 10)
                        shieldAmount = Math.Round(shieldAmount, 2);
                    else
                        shieldAmount = Math.Round(shieldAmount, 1);
                    ImGui.Text(shieldAmount.ToString() + "K");
                }
                if (InfoManager.Configuration.ShieldDisplay == ShieldDisplay.P)
                    ImGui.Text($"{actorsStats[i].Item1}%%");
                ImGui.EndChild();
            }
        }

    }
}
