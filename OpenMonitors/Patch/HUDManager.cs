using HarmonyLib;
using OpenMonitors.Monitors;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Patch;

[HarmonyPatch(typeof(global::HUDManager))]
public class HUDManager
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::HUDManager.ApplyPenalty))]
    private static void UpdateCreditsAfterDeadPlayersPenalty()
    {
        ModLogger.LogDebug("HUDManager.UpdateCreditsAfterDeadPlayersPenalty");
        CreditsMonitor.Instance.UpdateMonitor();
    }
}
