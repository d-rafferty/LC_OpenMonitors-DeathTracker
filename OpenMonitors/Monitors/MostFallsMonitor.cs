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

namespace OpenMonitors.Monitors;

public class MostFallsMonitor : MonoBehaviour
{
    public static MostFallsMonitor Instance = null!;

    private TextMeshProUGUI _textMesh = null!;

    public static Dictionary<int, int> FallDeathDict = new Dictionary<int, int>();  

    private static DateTime _LastDeathCall = DateTime.MinValue;

    public void Start()
    {
        ModLogger.LogDebug($"{name} -> Start()");
        if (!Instance) Instance = this;
        _textMesh = GetComponent<TextMeshProUGUI>();
        ModLogger.LogDebug($"{name} -> Start() -> UpdateMonitor()");
        UpdateMonitor();
    }

    public void UpdateMonitor()
    {
        ModLogger.LogDebug($"{name} -> UpdateMonitor()");
        if (FallDeathDict.Count == 0)
        {
            _textMesh.text = Config.HideLifeSupport.Value
            ? string.Empty
            : $"MOST FALLS:\n N/A";
        }
        else
        {
            int mostFalls = FallDeathDict.Max(x => x.Value);
            Console.WriteLine("MOST FALLS: ");
            Console.WriteLine(mostFalls);
            int mostPlayerid = FallDeathDict.FirstOrDefault(x => x.Value == mostFalls).Key;    //if pid is 0 it is host
            ModLogger.LogDebug($"Most falls id: {mostPlayerid} fall count: {mostFalls}");
            //find name from id and pass it in here

            _textMesh.text = Config.HideLifeSupport.Value
            ? string.Empty
            : $"MOST FALLS:\np: {mostPlayerid} - {mostFalls}";
        }
    }

    public static void checkFallDict(int playerid)
    {
        var now = DateTime.UtcNow;
        if ((now - _LastDeathCall).TotalSeconds < 15) { return; } //death function called by game 3 times quickly, sets 5 sec cooldown to avoid this


        _LastDeathCall = now;
        if (FallDeathDict.ContainsKey(playerid))
        {
            Console.WriteLine("INCREMENTING FALL COUNT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            FallDeathDict[playerid]++;
        }
        else { FallDeathDict.Add(playerid, 1); }
    }
}
