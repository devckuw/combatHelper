using Dalamud.Interface.ImGuiFileDialog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace combatHelper.Windows
{
    internal class FileDialogService
    {
        private readonly FileDialogManager _manager;
        private readonly ConcurrentDictionary<string, string> _startPaths = new();
        private bool _isOpen;

        public FileDialogService()
        {
            _manager = new FileDialogManager();
        }

        /*public void OpenFilePicker(string title, string filters, Action<bool, List<string>> callback, int selectionCountMax, string? startPath, bool forceStartPath)
        {
            _isOpen = true;
            _manager.OpenFileDialog(title, filters, CreateCallback(title, callback), selectionCountMax, GetStartPath(title, startPath, forceStartPath));
        }

        private Action<bool, string> CreateCallback(string title, Action<bool, string> callback)
        {
            return (valid, list) =>
            {
                _isOpen = false;
                var loc = HandleRoot(GetCurrentLocation());
                _startPaths[title] = loc;
                callback(valid, HandleRoot(list));
            };
        }

        private static string HandleRoot(string path)
        {
            if (path is [_, ':'])
                return path + '\\';

            return path;
        }

        private string GetCurrentLocation()
        => (_manager.GetType().GetField("dialog", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(_manager) as FileDialog)
            ?.GetCurrentPath()
         ?? ".";
        */
    }
}
