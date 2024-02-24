using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static OpenMonitors.Plugin;
using Object = UnityEngine.Object;

namespace OpenMonitors.Monitors;

internal static class Setup
{
    private const string MonitorContainerPath
        = "Environment/HangarShip/ShipModels2b/MonitorWall/Cube/Canvas (1)/MainContainer";

    // Position: (10.55, 3.7, -14.5)
    // Local Position: (-10, 0, 0)
    private static GameObject _mainContainer = null!;

    // Position: (10.57, 3.7, -14.12)
    // Local Position: (-233, 24, 21)
    private static GameObject _quotaMonitorText = null!;

    // Position: (10.57, 3.7, -14.9)
    // Local Position: (240, 24, 21)
    private static GameObject _deadlineMonitorText = null!;

    private static readonly Vector3 TopRowLocalRotation = new(351.5f, 0f);
    private static readonly Vector3 LeftMonitorGroupBottomRowLocalRotation = new(9.5577f, 0f);
    private static readonly Vector3 RightMonitorGroupLocalRotation = new(0f, 24.9f, 5.4f);

    private static readonly Dictionary<int, Tuple<Vector3, Vector3>> MonitorPositionsBySlots = new()
    {
        { 1, new Tuple<Vector3, Vector3>(new Vector3(-233f, 24f, 21f), LeftMonitorGroupBottomRowLocalRotation) },
        { 2, new Tuple<Vector3, Vector3>(new Vector3(240f, 24f, 21f), LeftMonitorGroupBottomRowLocalRotation) },
        {
            3,
            new Tuple<Vector3, Vector3>(
                new Vector3(797f, 29f, -101f),
                new Vector3(8.4566f, 26.5f, RightMonitorGroupLocalRotation.z)
            )
        },
        {
            4,
            new Tuple<Vector3, Vector3>(
                new Vector3(1220f, 80f, -304f),
                new Vector3(8.4566f, RightMonitorGroupLocalRotation.y, RightMonitorGroupLocalRotation.z)
            )
        },
        { 5, new Tuple<Vector3, Vector3>(new Vector3(-233f, 480f), TopRowLocalRotation) },
        { 6, new Tuple<Vector3, Vector3>(new Vector3(240f, 480f), TopRowLocalRotation) },
        {
            7,
            new Tuple<Vector3, Vector3>(
                new Vector3(748f, 500f, -110f),
                TopRowLocalRotation + RightMonitorGroupLocalRotation
            )
        },
        {
            8,
            new Tuple<Vector3, Vector3>(
                new Vector3(1170f, 540f, -310.3f),
                TopRowLocalRotation + RightMonitorGroupLocalRotation
            )
        },
        {
            9,
            new Tuple<Vector3, Vector3>(
                new Vector3(905f, -545f, -235f),
                new Vector3(10.5f, 26.2f, 5.2f)
            )
        }
    };


    public static void Initialize()
    {
        _mainContainer = GameObject.Find(MonitorContainerPath);
        // QUOTA: $0 / $509
        _quotaMonitorText = GameObject.Find(MonitorContainerPath + "/HeaderText");
        _quotaMonitorText.name = "ProfitQuota";
        var (qLocalPosition, qLocalRotation) = MonitorPositionsBySlots[Config.ProfitQuotaMonitorSlot.Value];
        _quotaMonitorText.transform.localPosition = qLocalPosition;
        _quotaMonitorText.transform.localRotation = Quaternion.Euler(qLocalRotation);
        // BG is the blue background of the quota monitor
        ModLogger.LogDebug("Destroying Quota BG");
        Object.Destroy(GameObject.Find(MonitorContainerPath + "/BG"));
        // DEADLINE: 7 Days, 3 hours
        _deadlineMonitorText = GameObject.Find(MonitorContainerPath + "/HeaderText (1)");
        _deadlineMonitorText.name = "Deadline";
        var (dLocalPosition, dLocalRotation) = MonitorPositionsBySlots[Config.DeadlineMonitorSlot.Value];
        _deadlineMonitorText.transform.localPosition = dLocalPosition;
        _deadlineMonitorText.transform.localRotation = Quaternion.Euler(dLocalRotation);
        // BG (1) is the blue background of the deadline monitor
        ModLogger.LogDebug("Destroying Deadline BG");
        Object.Destroy(GameObject.Find(MonitorContainerPath + "/BG (1)"));

        LifeSupportMonitor.Instance = CreateMonitor<LifeSupportMonitor>(Config.LifeSupportMonitorSlot.Value);

        LootMonitor.Instance = CreateMonitor<LootMonitor>(Config.LootMonitorSlot.Value);

        TimeMonitor.Instance = CreateMonitor<TimeMonitor>(Config.TimeMonitorSlot.Value);

        CreditsMonitor.Instance = CreateMonitor<CreditsMonitor>(Config.CreditsMonitorSlot.Value);

        DayMonitor.Instance = CreateMonitor<DayMonitor>(Config.DayMonitorSlot.Value);
        
        // Player Alive monitor
        // local position = (818, -493, -196)
        // local rotation = (9.5577, 24.9, 5.4)
        
        PlayersLifeSupportMonitor.Instance = CreateMonitor<PlayersLifeSupportMonitor>(9);
    }

    private static T CreateMonitor<T>(int targetSlot) where T : MonoBehaviour
    {
        /*
         * MONITOR LAYOUT:
         * 1: PROFIT QUOTA
         * 2: DEADLINE
         * 3: CAMERA INSIDE SHIP
         *
         * 0: MOON ORBIT / BIRD'S EYE PLAYER VIEW
         * 9: CAMERA OUTSIDE SHIP
         * -----------------
         * | 5 | 6 | 7 | 8 |
         * | 1 | 2 | 3 | 4 |
         * -----------------
         * | 00000 | 99999 |
         * | 00000 | 99999 |
         * -----------------
         *
         * Bottom row rotation: (9.6, 0, 0)
         *  3 local position: (-150, 480, 0)
         *  4 local position: (1285, 90, -341)
         *  - rotation: (9.6, 24.5, 5.6)
         *
         * Top row rotation: (-8.5f, 0, 0)
         *  5 local position: (-150, 480, 0)
         *  6 local position: (320, 480, 0)
         * 7 local position: (807.5, 500, -135)
         *  - rotation: (351, 24.5, 5.6)
         * 8 local position: (1225, 540, -341)
         *  - rotation: (351, 24.5, 5.6)
         */
        var monitorName = typeof(T).Name.Replace("Monitor", "");
        ModLogger.LogDebug($"Creating Monitor {monitorName}");
        var monitor = Object.Instantiate(_quotaMonitorText, _mainContainer.transform);
        monitor.name = monitorName;
        var monitorText = monitor.GetComponent<TextMeshProUGUI>();
        // localPosition (0, 0, 0) is the center of the bottom 2 monitors above the Orbit monitor
        var (localPosition, localRotation) = MonitorPositionsBySlots[targetSlot];

        monitorText.transform.localPosition = localPosition;
        monitorText.transform.localRotation = Quaternion.Euler(localRotation);
        ModLogger.LogDebug($"Monitor {monitorName} created");
        return monitor.AddComponent<T>();
    }
}
