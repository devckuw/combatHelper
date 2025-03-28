using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace combatHelper.Utils
{
    internal class TimeManager : IDisposable
    {

        #region Singleton
        private TimeManager()
        {
            Plugin.Framework.Update += OnUpdate;
        }

        public static void Initialize() { Instance = new TimeManager(); }

        public static TimeManager Instance { get; private set; } = null!;

        ~TimeManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            Plugin.Framework.Update -= OnUpdate;
            Instance = null!;
        }
        #endregion

        private DateTime startTimer;
        private bool isStarted = false;
        private bool inCombat = false;
        private bool isPotTwoUsed = false;
        private bool isPotThreeUsed = false;

        public delegate void OnFightStartDelegate();
        public delegate void OnFightEndDelegate();

        public OnFightStartDelegate OnFightStart;
        public OnFightEndDelegate OnFightEnd;

        public void ResetEvents()
        {
            OnFightStart = null;
            OnFightEnd = null;
            Plugin.Log.Debug("reset event");
        }

        private void OnUpdate(IFramework framework)
        {
            inCombat = Plugin.Condition[ConditionFlag.InCombat];

            if (!inCombat)
            {
                foreach (var actor in Plugin.PartyList)
                {
                    if (actor.GameObject is not ICharacter character ||
                            (character.StatusFlags & StatusFlags.InCombat) == 0) continue;
                    inCombat = true;
                    break;
                }
            }

            if (inCombat)
            {
                if (!isStarted)
                {
                    if (OnFightStart != null)
                        OnFightStart();
                    Plugin.Log.Debug("fight start");
                    isStarted = true;
                    startTimer = DateTime.Now;
                }
                var combatDuration = (DateTime.Now - startTimer).Seconds + (DateTime.Now - startTimer).Minutes * 60;
                var offset = InfoManager.Configuration.OffsetPots;
                switch (InfoManager.nbPots)
                {
                    case NbPots.None:
                        break;
                    case NbPots.Two_Pots:
                        if (combatDuration >= 6 * 60 + offset && !isPotTwoUsed) { InfoManager.soundPlayer.Play(); isPotTwoUsed = true; }
                        break;
                    case NbPots.Two_Pots_Bard:
                        if (combatDuration >= 2 * 60 + offset && !isPotTwoUsed) { InfoManager.soundPlayer.Play(); isPotTwoUsed = true; }
                        if (combatDuration >= 8 * 60 + offset && !isPotThreeUsed) { InfoManager.soundPlayer.Play(); isPotThreeUsed = true; }
                        break;
                    case NbPots.Two_Ten:
                        if (combatDuration >= 2 * 60 + offset && !isPotTwoUsed) { InfoManager.soundPlayer.Play(); isPotTwoUsed = true; }
                        if (combatDuration >= 10 * 60 + offset && !isPotThreeUsed) { InfoManager.soundPlayer.Play(); isPotThreeUsed = true; }
                        break;
                    case NbPots.Three_Pots:
                        if (combatDuration >= 5 * 60 + offset && !isPotTwoUsed) { InfoManager.soundPlayer.Play(); isPotTwoUsed = true; }
                        if (combatDuration >= 10 * 60 + offset && !isPotThreeUsed) { InfoManager.soundPlayer.Play(); isPotThreeUsed = true; }
                        break;
                    case NbPots.Three_twoPots:
                        if (combatDuration >= 6 * 60 + offset && !isPotTwoUsed) { InfoManager.soundPlayer.Play(); isPotTwoUsed = true; }
                        if (combatDuration >= 12 * 60 + offset && !isPotThreeUsed) { InfoManager.soundPlayer.Play(); isPotThreeUsed = true; }
                        break;
                }
            }
            if (!inCombat && isStarted)
            {
                if (OnFightEnd != null)
                    OnFightEnd();
                Plugin.Log.Debug("fight end");
                isPotTwoUsed = false;
                isPotThreeUsed = false;
                isStarted = false;
            }
        }

    }

}
