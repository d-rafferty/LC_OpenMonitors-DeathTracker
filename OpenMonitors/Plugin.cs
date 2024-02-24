using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace OpenMonitors;

public static class ModInfo
{
    public const string Name = "OpenMonitors";
    public const string Guid = "xxxstoner420bongmasterxxx.open_monitors";
    public const string Version = "1.0.5";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
public class Plugin : BaseUnityPlugin
{
    public static ConfigFile ModConfig = null!;

    private static Plugin? _instance;

    public static ManualLogSource ModLogger = null!;
    private readonly Harmony _harmony = new(ModInfo.Guid);

    private void Awake()
    {
        ModLogger = Logger;

        if (!_instance)
        {
            ModLogger.LogInfo($"{ModInfo.Name} -> loading");
            _instance = this;
            ModConfig = Config;
            Monitors.Config.Initialize();
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            ModLogger.LogInfo($"{ModInfo.Name} -> complete");
        }
    }
}
