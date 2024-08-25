using Dalamud.Interface.ImGuiFileDialog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace combatHelper.Utils
{
    internal class FileDialogService
    {
        public readonly FileDialogManager manager;
        public bool isOpen = false;
        public string filePicked = "Using config one";

        public FileDialogService()
        {
            manager = new FileDialogManager();
        }

        public void callbackfile(bool b, string s)
        {
            filePicked = s;
            InfoManager.Configuration.SetSound(s);
            InfoManager.UpdateSound();
            isOpen = false;
        }

        public void CreateFileDiag()
        {
            manager.OpenFileDialog("test", ".*", callbackfile);
        }
    }
}
