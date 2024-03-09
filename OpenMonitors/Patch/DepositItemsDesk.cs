using HarmonyLib;
using OpenMonitors.Monitors;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Patch;

[HarmonyPatch(typeof(global::DepositItemsDesk))]
public class DepositItemsDesk
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::DepositItemsDesk.SellAndDisplayItemProfits))]
    private static void UpdateCreditsAfterSellingLoot()
    {
        ModLogger.LogDebug("DepositItemsDesk.UpdateCreditsAfterSellingLoot");
        DaysSinceIncidentMonitor.Instance.UpdateMonitor();
    }
}
