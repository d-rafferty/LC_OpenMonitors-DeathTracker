using TMPro;
using UnityEngine;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Monitors;

public class TimeMonitor : MonoBehaviour
{
    public static TimeMonitor Instance = null!;

    public TextMeshProUGUI textMesh = null!;


    public void Start()
    {
        ModLogger.LogDebug($"{name} -> Start()");
        if (!Instance) Instance = this;
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = Config.HideTime.Value ? string.Empty : "TIME:\n7:30\nAM";
        ModLogger.LogDebug($"{name} -> Start() end");
    }

    public void UpdateMonitor()
    {
        // Log.LogDebug($"{name} -> UpdateMonitor()");
        textMesh.text = Config.HideTime.Value ? string.Empty : $"TIME:\n{HUDManager.Instance.clockNumber.text}";
    }
}
