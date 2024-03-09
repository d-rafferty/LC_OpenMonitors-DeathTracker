using System.Collections;
using DunGen;
using HarmonyLib;
using OpenMonitors.Monitors;
using Unity.Netcode;
using Unity.Services.Authentication.Internal;
using UnityEngine;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Patch;

[HarmonyPatch(typeof(GameNetcodeStuff.PlayerControllerB))]
public class PlayerControllerB
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameNetcodeStuff.PlayerControllerB.ConnectClientToPlayerObject))]
    private static void OnPlayerConnect()
    {
        CoroutineHelper.Instance.StartCoroutine(WaitOnPlayerConnectForMonitorsToBeCreated());
    }

    private static IEnumerator WaitOnPlayerConnectForMonitorsToBeCreated()
    {
        ModLogger.LogDebug("WaitOnPlayerConnectForMonitorsToBeCreated");
        yield return new WaitUntil(() => DaysSinceIncidentMonitor.Instance && MostFallsMonitor.Instance);
        DaysSinceIncidentMonitor.Instance.UpdateMonitor();
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameNetcodeStuff.PlayerControllerB.KillPlayer))]
    private static void UpdateLifeSupportMonitorOnPlayerDeath(
        Vector3 bodyVelocity,
        bool spawnBody,
        CauseOfDeath causeOfDeath,
        int deathAnimation
    )
    {
        ModLogger.LogDebug("PlayerControllerB.UpdateLifeSupportMonitorOnPlayerDeath");

        if( causeOfDeath == CauseOfDeath.Gravity)
        {
            ModLogger.LogDebug("DIED TO GRAVITY");
            MostFallsMonitor.checkFallDict(0);  //passing 0 for host
            TotalFallsMonitor.Instance.NewDeath();
            MostFallsMonitor.Instance.UpdateMonitor();
        }

        DaysSinceIncidentMonitor.Instance.playerDied();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameNetcodeStuff.PlayerControllerB.KillPlayerClientRpc))]
    private static void UpdateLifeSupportMonitorOnPlayerDeathClientRpc(
        int playerId,
        bool spawnBody,
        Vector3 bodyVelocity,
        CauseOfDeath causeOfDeath,
        int deathAnimation
    )
    {
        ModLogger.LogDebug("PlayerControllerB.UpdateLifeSupportMonitorOnPlayerDeathClientRpc");

        if(causeOfDeath == CauseOfDeath.Gravity)
        {
            MostFallsMonitor.checkFallDict(playerId);
            MostFallsMonitor.Instance.UpdateMonitor();
            TotalFallsMonitor.Instance.NewDeath();
        }

        DaysSinceIncidentMonitor.Instance.playerDied();
    }
}
