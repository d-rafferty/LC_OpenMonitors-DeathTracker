using System;
using System.Linq;
using HarmonyLib;
using OpenMonitors.Monitors;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Patch;

[HarmonyPatch(typeof(global::Terminal))]
public class Terminal
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::Terminal.SyncGroupCreditsClientRpc))]
    private static void RefreshMoney()
    {
        ModLogger.LogDebug("Terminal.RefreshMoney");
        DaysSinceIncidentMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::Terminal.TextPostProcess))]
    // ReSharper disable once InconsistentNaming
    private static string WeatherConditions(string __result)
    {
        if (Config.HideWeather.Value)
        {
#pragma warning disable Harmony003
            __result = Enum
                .GetValues(typeof(LevelWeatherType))
                .Cast<LevelWeatherType>()
                .Aggregate(__result, (current, value) => current.Replace($"({value})", string.Empty));
#pragma warning restore Harmony003
        }

        return __result;
    }
}
