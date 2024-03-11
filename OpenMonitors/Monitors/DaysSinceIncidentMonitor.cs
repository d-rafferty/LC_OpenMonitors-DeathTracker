using System;
using TMPro;
using UnityEngine;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Monitors;

public class DaysSinceIncidentMonitor : MonoBehaviour
{
    public static DaysSinceIncidentMonitor Instance = null!;

    private Terminal _terminal = null!;

    private TextMeshProUGUI _textMesh = null!;

    int daysSinceIncident = 0;

    bool deathOnDay = false;

    public static DateTime _LastCall = DateTime.MinValue;

    public void Start()
    {
        ModLogger.LogDebug($"{name} -> Start()");
        if (!Instance) Instance = this;
        _textMesh = GetComponent<TextMeshProUGUI>();
        _terminal = FindObjectOfType<Terminal>();
        ModLogger.LogDebug($"{name} -> Start() -> UpdateMonitor()");
        UpdateMonitor();
    }

    public void UpdateMonitor()
    {
        ModLogger.LogDebug($"{name} -> UpdateMonitor()");
        _textMesh.text = Config.HideCredits.Value ? string.Empty : $"Days\nSince\nIncident:\n{daysSinceIncident}";
        _textMesh.fontSize = 69;
    }

    public void playerDied()
    {
        deathOnDay = true;
    }

    public void EndOfDay()
    {
        var now = DateTime.UtcNow;
        if ((now - _LastCall).TotalSeconds < 10) { return; }

        if (deathOnDay == false) { daysSinceIncident++; }
        else { 
            deathOnDay = false;
            daysSinceIncident = 0;
        } 

        _LastCall = now;

        UpdateMonitor();    
    }
}
