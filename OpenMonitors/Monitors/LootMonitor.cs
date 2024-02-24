using System.Linq;
using TMPro;
using UnityEngine;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Monitors;

public class LootMonitor : MonoBehaviour
{
    public static LootMonitor Instance = null!;

    public TextMeshProUGUI textMesh = null!;

    private GameObject _ship = null!;

    public void Start()
    {
        ModLogger.LogDebug($"{name} -> Start()");
        if (!Instance) Instance = this;
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = "LOOT:\n$NaN";
        _ship = GameObject.Find("/Environment/HangarShip");
        ModLogger.LogDebug($"{name} -> Start() -> UpdateMonitor()");
        UpdateMonitor();
    }

    public void UpdateMonitor()
    {
        ModLogger.LogDebug($"{name} -> UpdateMonitor()");
        textMesh.text = Config.HideLoot.Value ? string.Empty : $"LOOT:\n${Calculate()}";
    }

    private float Calculate()
    {
        ModLogger.LogDebug($"{name} -> Calculate()");
        return (
            from grabbable in _ship.GetComponentsInChildren<GrabbableObject>()
            where CheckIfItemIsScrapAndOnShipFloor(grabbable)
            select grabbable
        ).Sum(x => x.scrapValue);

        bool CheckIfItemIsScrapAndOnShipFloor(GrabbableObject item)
        {
            return item.itemProperties.isScrap && item is { isPocketed: false, isHeld: false };
        }
    }
}
