﻿using System.Text;
using HarmonyLib;
using OpenMonitors.Monitors;
using TMPro;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Patch;

[HarmonyPatch(typeof(global::StartOfRound))]
public class StartOfRound
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::StartOfRound.Start))]
    private static void Initialize()
    {
        Log.LogDebug("StartOfRound.Initialize");
        Setup.Initialize();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::StartOfRound.ReviveDeadPlayers))]
    private static void RefreshMonitorsWhenPlayerRevives()
    {
        Log.LogDebug("StartOfRound.RefreshMonitorsWhenPlayerRevives");
        CreditsMonitor.Instance.UpdateMonitor();
        DayMonitor.Instance.UpdateMonitor();
        LifeSupportMonitor.Instance.UpdateMonitor();
        LootMonitor.Instance.UpdateMonitor();
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::StartOfRound.SyncShipUnlockablesClientRpc))]
    private static void RefreshLootForClientOnStart()
    {
        Log.LogDebug("StartOfRound.RefreshLootForClientOnStart");
        LootMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::StartOfRound.ChangeLevelClientRpc))]
    private static void UpdateCreditsWhenSwitchingMoons()
    {
        Log.LogDebug("StartOfRound.UpdateCreditsWhenSwitchingMoons");
        CreditsMonitor.Instance.UpdateMonitor();
    }


    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::StartOfRound.EndOfGameClientRpc))]
    private static void RefreshDayWhenShipHasLeft()
    {
        Log.LogDebug("StartOfRound.RefreshDayWhenShipHasLeft");
        DayMonitor.Instance.UpdateMonitor();
        LifeSupportMonitor.Instance.UpdateMonitor();
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::StartOfRound.StartGame))]
    private static void UpdateDayAtStartOfGame()
    {
        Log.LogDebug("StartOfRound.UpdateDayAtStartOfGame");
        DayMonitor.Instance.UpdateMonitor();
        LifeSupportMonitor.Instance.UpdateMonitor();
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::StartOfRound.OnClientConnect))]
    private static void UpdateMonitorsWhenPlayerConnectsClient(ulong clientId)
    {
        Log.LogDebug("StartOfRound.UpdateMonitorsWhenPlayerConnectsClient");
        CreditsMonitor.Instance.UpdateMonitor();
        LifeSupportMonitor.Instance.UpdateMonitor();
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
        LootMonitor.Instance.UpdateMonitor();
    }
    
    
    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::StartOfRound.OnPlayerConnectedClientRpc))]
    private static void UpdateMonitorsWhenPlayerConnectsClientRpc(
        ulong clientId,
        int connectedPlayers,
        ulong[] connectedPlayerIdsOrdered,
        int assignedPlayerObjectId,
        int serverMoneyAmount,
        int levelID,
        int profitQuota,
        int timeUntilDeadline,
        int quotaFulfilled,
        int randomSeed  
    )
    {
        Log.LogDebug("StartOfRound.UpdateMonitorsWhenPlayerConnectsClientRpc");
        CreditsMonitor.Instance.UpdateMonitor();
        LifeSupportMonitor.Instance.UpdateMonitor();
        LootMonitor.Instance.UpdateMonitor();
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::StartOfRound.OnPlayerDC))]
    private static void UpdateMonitorsWhenPlayerDisconnects(int playerObjectNumber, ulong clientId)
    {
        Log.LogDebug("StartOfRound.UpdateMonitorsWhenPlayerDisconnects");
        CreditsMonitor.Instance.UpdateMonitor();
        LifeSupportMonitor.Instance.UpdateMonitor();
        LootMonitor.Instance.UpdateMonitor();
        PlayersLifeSupportMonitor.Instance.UpdateMonitor();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(global::StartOfRound.SetMapScreenInfoToCurrentLevel))]
    // ReSharper disable twice InconsistentNaming
    private static void ColorWeather(ref TextMeshProUGUI ___screenLevelDescription, ref SelectableLevel ___currentLevel)
    {
        ___screenLevelDescription.text = new StringBuilder()
            .Append("Orbiting: ")
            .AppendLine(___currentLevel.PlanetName)
            .Append("Weather: ")
            .AppendLine(FormatWeather(___currentLevel.currentWeather))
            .Append(___currentLevel.LevelDescription ?? string.Empty)
            .ToString();
    }

    private static string FormatWeather(LevelWeatherType currentWeather)
    {
        Log.LogDebug($"Weather: {currentWeather}");
        string text;
        switch (currentWeather)
        {
            case LevelWeatherType.Rainy:
            case LevelWeatherType.Foggy:
                // yellow
                text = "FFF01C";
                break;
            case LevelWeatherType.Stormy:
            case LevelWeatherType.Flooded:
                // orange
                text = "FF9B00";
                break;
            case LevelWeatherType.Eclipsed:
                // red
                text = "FF0000";
                break;
            case LevelWeatherType.None:
            case LevelWeatherType.DustClouds:
            default:
                // lime green
                text = "69FF69";
                break;
        }

        return $"<color=#{text}>{currentWeather}</color>";
    }
}
