using System.Collections;
using DunGen;
using HarmonyLib;
using OpenMonitors.Monitors;
using Unity.Netcode;
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
        yield return new WaitUntil(() => CreditsMonitor.Instance && LifeSupportMonitor.Instance);
        CreditsMonitor.Instance.UpdateMonitor();
        LifeSupportMonitor.Instance.UpdateMonitor();
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }


    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameNetcodeStuff.PlayerControllerB.GrabObjectClientRpc))]
    private static void RefreshLootOnPickupClient(bool grabValidated, ref NetworkObjectReference grabbedObject)
    {
        if (grabbedObject.TryGet(out var networkObject))
        {
            var componentInChildren = networkObject.gameObject.GetComponentInChildren<GrabbableObject>();
            if (componentInChildren.isInShipRoom || componentInChildren.isInElevator)
            {
                ModLogger.LogDebug("PlayerControllerB.RefreshLootOnPickupClient");
                LootMonitor.Instance.UpdateMonitor();
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameNetcodeStuff.PlayerControllerB.ThrowObjectClientRpc))]
    private static void RefreshLootOnThrowClient(
        bool droppedInElevator,
        bool droppedInShipRoom,
        Vector3 targetFloorPosition,
        NetworkObjectReference grabbedObject
    )
    {
        if (droppedInShipRoom || droppedInElevator)
        {
            ModLogger.LogDebug("PlayerControllerB.RefreshLootOnThrowClient");
            LootMonitor.Instance.UpdateMonitor();
        }
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
        LifeSupportMonitor.Instance.UpdateMonitor();
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameNetcodeStuff.PlayerControllerB.KillPlayerClientRpc))]
    private static void UpdateLifeSupportMonitorOnPlayerDeathClientRpc(
        int playerId,
        bool spawnBody,
        Vector3 bodyVelocity,
        int causeOfDeath,
        int deathAnimation
    )
    {
        ModLogger.LogDebug("PlayerControllerB.UpdateLifeSupportMonitorOnPlayerDeathClientRpc");
        LifeSupportMonitor.Instance.UpdateMonitor();
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameNetcodeStuff.PlayerControllerB.DamagePlayer))]
    private static void UpdateLifeSupportMonitorOnPlayerDamage(
        int damageNumber,
        bool hasDamageSFX,
        bool callRPC,
        CauseOfDeath causeOfDeath,
        int deathAnimation,
        bool fallDamage,
        Vector3 force
    )
    {
        ModLogger.LogDebug("PlayerControllerB.UpdateLifeSupportMonitorOnPlayerDamage");
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameNetcodeStuff.PlayerControllerB.DamageOnOtherClients))]
    private static void UpdateLifeSupportMonitorForPlayerDamageOnOtherClients(int damageNumber, int newHealthAmount)
    {
        ModLogger.LogDebug("PlayerControllerB.UpdateLifeSupportMonitorForPlayerDamageOnOtherClients");
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameNetcodeStuff.PlayerControllerB.DamagePlayerClientRpc))]
    private static void UpdateLifeSupportMonitorOnPlayerDamageClientRpc(int damageNumber, int newHealthAmount)
    {
        ModLogger.LogDebug("PlayerControllerB.UpdateLifeSupportMonitorOnPlayerDamageClientRpc");
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameNetcodeStuff.PlayerControllerB.DamagePlayerFromOtherClientClientRpc))]
    private static void UpdateLifeSupportMonitorOnOtherClientPlayerDamageClientRpc(
        int damageAmount,
        Vector3 hitDirection,
        int playerWhoHit,
        int newHealthAmount
    )
    {
        ModLogger.LogDebug("PlayerControllerB.UpdateLifeSupportMonitorOnOtherClientPlayerDamageClientRpc");
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }
}
