using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFXIVClientStructs.FFXIV.Client.System.String;
using FFXIVClientStructs.FFXIV.Client.UI.Shell;

namespace combatHelper.Utils
{
    public enum ChatMode
    {
        None = 0,
        Echo = 1,
        Party = 2,
        Alliance = 3

    }

        public static class ChatHelper
    {
        public static void Send(ChatMode mode, string msg)
        {
            if (mode == ChatMode.None)
            {
                ExecuteCommand("/e No Chat mode selected.");
                return;
            }
            msg = "/" + mode.ToString().ToLower() + " " + msg;
            ExecuteCommand(msg);
        }

        public static unsafe void ExecuteCommand(string command)
        {
            if (!command.StartsWith('/'))
                return;

            using var cmd = new Utf8String(command);

            // Technically not needed since we don't use payloads but provides a better example.
            cmd.SanitizeString(
                AllowedEntities.Unknown9     |
                AllowedEntities.Payloads          |
                AllowedEntities.OtherCharacters   |
                AllowedEntities.SpecialCharacters |
                AllowedEntities.Numbers           |
                AllowedEntities.LowercaseLetters  |
                AllowedEntities.UppercaseLetters  );

            if (cmd.Length > 500)
                return;

            UIModule.Instance()->ProcessChatBoxEntry(&cmd);
            //RaptureShellModule.Instance()->ExecuteCommandInner(&cmd, UIModule.Instance());
        }

        public static unsafe bool IsInputTextActive => RaptureAtkModule.Instance()->IsTextInputActive();
    }
}
