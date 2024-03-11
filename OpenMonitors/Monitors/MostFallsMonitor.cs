using TMPro;
using UnityEngine;
using static OpenMonitors.Plugin;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Discord;
using System;
using DunGen;
using HarmonyLib;
using Dissonance.Networking;
using System.Linq;
using GameNetcodeStuff;
using System.Collections;
using Unity.Services.Authentication.Internal;

namespace OpenMonitors.Monitors;

public class MostFallsMonitor : MonoBehaviour
{
    public static MostFallsMonitor Instance = null!;

    private TextMeshProUGUI _textMesh = null!;

    public static Dictionary<PlayerControllerB, int> FallDeathDict = new Dictionary<PlayerControllerB, int>();

    int mostFalls = 0;

    bool firstCall = true;

    private static DateTime _LastCall = DateTime.MinValue;


    public void Start()
    {
        ModLogger.LogDebug($"{name} -> Start()");
        if (!Instance) Instance = this;
        _textMesh = GetComponent<TextMeshProUGUI>();
        _textMesh.text = Config.HideLifeSupport.Value
            ? string.Empty
            : $"MOST FALLS:\n N/A";
    }

    public void UpdateMonitor()
    {
        if (firstCall == true)
        {
            foreach (var playerId in StartOfRound.Instance.ClientPlayerList.Keys)
            {
                PlayerControllerB player = StartOfRound.Instance.allPlayerScripts[playerId];
                FallDeathDict.Add(player, 0);
            }
            firstCall = false;
        } else
        {
            var now = DateTime.UtcNow;
            if ((now - _LastCall).TotalSeconds > 15)        //update called multiple times quickly, sets 15 sec cooldown to avoid multiple counts of same death
            {
                _LastCall = now;
                foreach (var playerId in StartOfRound.Instance.ClientPlayerList.Keys)
                {
                    PlayerControllerB player = StartOfRound.Instance.allPlayerScripts[playerId];
                    if (player.isPlayerDead & player.causeOfDeath == CauseOfDeath.Gravity)
                    {
                        FallDeathDict[player]++;
                        TotalFallsMonitor.NewDeath();
                    }
                }
            } 
        }

        mostFalls = FallDeathDict.Max(x => x.Value);
        if (mostFalls == 0)
        {
            _textMesh.text = Config.HideLifeSupport.Value
            ? string.Empty
            : $"MOST FALLS:\n N/A";
        }
        else
        {
            PlayerControllerB fallGuy = FallDeathDict.FirstOrDefault(x => x.Value == mostFalls).Key;
            _textMesh.fontSize = 56;

            _textMesh.text = Config.HideLifeSupport.Value
            ? string.Empty
            : $"MOST FALLS:\n{fallGuy.playerUsername} - {mostFalls}";
        }
    }
}