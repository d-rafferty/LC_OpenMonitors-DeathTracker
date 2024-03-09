using HarmonyLib;
using OpenMonitors.Monitors;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Patch;

[HarmonyPatch(typeof(global::TimeOfDay))]
public class TimeOfDay
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::TimeOfDay.SyncNewProfitQuotaClientRpc))]
    private static void UpdateCreditsAfterReachingQuota()
    {
        ModLogger.LogDebug("TimeOfDay.UpdateCreditsAfterReachingQuota");
        DaysSinceIncidentMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::TimeOfDay.MoveTimeOfDay))]
    private static void UpdateClockTime()
    {
        // Log.LogDebug("TimeOfDay.UpdateClockTime");
        TimeMonitor.Instance.UpdateMonitor();
    }
}
