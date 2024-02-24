using TMPro;
using UnityEngine;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Monitors;

public class LifeSupportMonitor : MonoBehaviour
{
    public static LifeSupportMonitor Instance = null!;

    private TextMeshProUGUI _textMesh = null!;

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
        _textMesh.text = Config.HideLifeSupport.Value
            ? string.Empty
            : $"ALIVE:\n{StartOfRound.Instance.livingPlayers} / {StartOfRound.Instance.connectedPlayersAmount + 1}";
    }
}
