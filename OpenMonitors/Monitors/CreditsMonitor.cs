using TMPro;
using UnityEngine;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Monitors;

public class CreditsMonitor : MonoBehaviour
{
    public static CreditsMonitor Instance = null!;

    private Terminal _terminal = null!;

    private TextMeshProUGUI _textMesh = null!;

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
        _textMesh.text = Config.HideCredits.Value ? string.Empty : $"CREDITS:\n${_terminal.groupCredits}";
    }
}
