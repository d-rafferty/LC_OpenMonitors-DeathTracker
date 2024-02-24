using BepInEx.Configuration;
using static OpenMonitors.Plugin;

namespace OpenMonitors.Monitors;

public static class Config
{
    private const string MonitorSection = "Monitors";

    private const string AllowableSlotValues = "Possible Slots: 1, 2, 4, 5, 6, 7, 8";

    public static ConfigEntry<bool> HideWeather { get; private set; } = null!;

    public static ConfigEntry<int> ProfitQuotaMonitorSlot { get; private set; } = null!;

    public static ConfigEntry<int> DeadlineMonitorSlot { get; private set; } = null!;

    public static ConfigEntry<bool> HideLifeSupport { get; private set; } = null!;
    public static ConfigEntry<int> LifeSupportMonitorSlot { get; private set; } = null!;

    public static ConfigEntry<bool> HidePlayersLifeSupport { get; private set; } = null!;

    public static ConfigEntry<bool> HideLoot { get; private set; } = null!;

    public static ConfigEntry<int> LootMonitorSlot { get; private set; } = null!;

    public static ConfigEntry<bool> HideCredits { get; private set; } = null!;

    public static ConfigEntry<int> CreditsMonitorSlot { get; private set; } = null!;

    public static ConfigEntry<bool> HideDay { get; private set; } = null!;

    public static ConfigEntry<int> DayMonitorSlot { get; private set; } = null!;

    public static ConfigEntry<bool> HideTime { get; private set; } = null!;

    public static ConfigEntry<int> TimeMonitorSlot { get; private set; } = null!;

    public static void Initialize()
    {
        HideWeather = ModConfig.Bind(
            "General",
            "HideWeather",
            false,
            "Disables Weather from the navigation screen, and Terminal"
        );

        ProfitQuotaMonitorSlot = ModConfig.Bind(
            MonitorSection,
            "ProfitQuotaMonitorSlot",
            1,
            $"Slot for the Profit Quota Monitor. {AllowableSlotValues}"
        );

        DeadlineMonitorSlot = ModConfig.Bind(
            MonitorSection,
            "DeadlineMonitorSlot",
            2,
            $"Slot for the Deadline Monitor. {AllowableSlotValues}"
        );

        HideLifeSupport = ModConfig.Bind(
            MonitorSection,
            "HideLifeSupport",
            false,
            "Disables the Life Support Monitor."
        );

        LifeSupportMonitorSlot = ModConfig.Bind(
            MonitorSection,
            "LifeSupportMonitorSlot",
            4,
            $"Slot for the Life Support Monitor. {AllowableSlotValues}"
        );

        HidePlayersLifeSupport = ModConfig.Bind(
            MonitorSection,
            "HidePlayersLifeSupport",
            true,
            "Disables the Players Life Support Monitor."
        );

        HideLoot = ModConfig.Bind(
            MonitorSection,
            "HideLoot",
            false,
            "Disables the Loot Monitor"
        );

        LootMonitorSlot = ModConfig.Bind(
            MonitorSection,
            "LootMonitorSlot",
            5,
            $"Slot for the Loot Monitor. {AllowableSlotValues}"
        );

        HideCredits = ModConfig.Bind(
            MonitorSection,
            "HideCredits",
            false,
            "Disables the Credits Monitor"
        );

        TimeMonitorSlot = ModConfig.Bind(
            MonitorSection,
            "TimeMonitorSlot",
            6,
            $"Slot for the Time Monitor. {AllowableSlotValues}"
        );

        HideDay = ModConfig.Bind(
            MonitorSection,
            "HideDay",
            false,
            "Disables the Day Monitor"
        );

        DayMonitorSlot = ModConfig.Bind(
            MonitorSection,
            "DayMonitorSlot",
            7,
            $"Slot for the Day Monitor. {AllowableSlotValues}"
        );

        HideTime = ModConfig.Bind(
            MonitorSection,
            "HideTime",
            false,
            "Disables the Time Monitor"
        );

        CreditsMonitorSlot = ModConfig.Bind(
            MonitorSection,
            "CreditsMonitorSlot",
            8,
            $"Slot for the Credits Monitor. {AllowableSlotValues}"
        );

        // Check if any slot values are equal to 3, and if so, set it back to default
        if (ProfitQuotaMonitorSlot.Value == 3) ProfitQuotaMonitorSlot.Value = (int)ProfitQuotaMonitorSlot.DefaultValue;

        if (DeadlineMonitorSlot.Value == 3) DeadlineMonitorSlot.Value = (int)DeadlineMonitorSlot.DefaultValue;

        if (LifeSupportMonitorSlot.Value == 3) LifeSupportMonitorSlot.Value = (int)LifeSupportMonitorSlot.DefaultValue;

        if (LootMonitorSlot.Value == 3) LootMonitorSlot.Value = (int)LootMonitorSlot.DefaultValue;

        if (TimeMonitorSlot.Value == 3) TimeMonitorSlot.Value = (int)TimeMonitorSlot.DefaultValue;

        if (DayMonitorSlot.Value == 3) DayMonitorSlot.Value = (int)DayMonitorSlot.DefaultValue;
    }
}
