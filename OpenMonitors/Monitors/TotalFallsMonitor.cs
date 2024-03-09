using System;
using System.Linq;
using TMPro;
using UnityEngine;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Monitors;

public class TotalFallsMonitor : MonoBehaviour
{
    public static TotalFallsMonitor Instance = null!;

    public TextMeshProUGUI textMesh = null!;

    private GameObject _ship = null!;

    private static DateTime _LastDeathCall = DateTime.MinValue;

    int totalFalls = 0;

    public void Start()
    {
        ModLogger.LogDebug($"{name} -> Start()");
        if (!Instance) Instance = this;
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = "FALLS:\n$NaN";
        _ship = GameObject.Find("/Environment/HangarShip");
        ModLogger.LogDebug($"{name} -> Start() -> UpdateMonitor()");
        UpdateMonitor();
    }

    public void UpdateMonitor()
    {
        ModLogger.LogDebug($"{name} -> UpdateMonitor()");
        textMesh.text = Config.HideLoot.Value ? string.Empty : $"FALLS:\n{totalFalls}";
    }

    public void NewDeath()
    {
        var now = DateTime.UtcNow;
        if ((now - _LastDeathCall).TotalSeconds < 15) { return; } //death function called by game 3 times quickly, sets 5 sec cooldown to avoid this
        totalFalls++;
        _LastDeathCall = now;
        UpdateMonitor();
    }
}
