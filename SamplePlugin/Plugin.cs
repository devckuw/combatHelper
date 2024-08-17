using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using combatHelper.Windows;

namespace combatHelper;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IChatGui Chat { get; private set; } = null!;
    [PluginService] public static IPartyList PartyList { get; private set; } = null!;
    [PluginService] public static ICondition Condition { get; private set; } = null!;
    [PluginService] public static IFramework Framework { get; private set; } = null!;

    private const string CommandName = "/combatHelper";
    private const string CommandNameShort = "/ch";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("combatHelper");
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        ConfigWindow = new ConfigWindow(this);
        MainWindow = new MainWindow(this);

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);
        
        CommandManager.AddHandler(CommandNameShort, new CommandInfo(OnCommand)
        {
            HelpMessage = "Opens the main menu" 
        });
        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Opens the main menu\n" +
            "/combatHelper kini → cursed sound\n" +
            "/combatHelper resetsound | rs → reset sound\n"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        // Adds another button that is doing the same but for the main ui of the plugin
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);
        CommandManager.RemoveHandler(CommandNameShort);
    }

    private void OnCommand(string command, string args)
    {
        if (string.IsNullOrEmpty(args))
        {
            ToggleMainUI();
            return;
        }

        var subcommands = args.Split(' ');

        var firstArg = subcommands[0];
        if (firstArg.ToLower() == "kini")
        {
            Configuration.Sound = "kini.wav";
            Configuration.Save();
            MainWindow.UpdateSound();
            return;
        }
        if (firstArg.ToLower() == "rs" || firstArg.ToLower() == "resetsound")
        {
            Configuration.Sound = "sound.wav";
            Configuration.Save();
            MainWindow.UpdateSound();
            return;
        }
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => MainWindow.Toggle();
}
