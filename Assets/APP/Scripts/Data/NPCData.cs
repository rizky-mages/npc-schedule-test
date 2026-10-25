using System.Collections.Generic;
using UnityEngine;

public enum GameDay
{
    Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
}

[System.Serializable]
public struct GameTime
{
    public GameDay day;
    [Range(0, 23)] public int hour;
    [Range(0, 59)] public int minute;

    public int TotalMinutes => (hour * 60) + minute;

    public string GetAMPMFormat()
    {
        int displayHour = hour == 0 ? 12 : (hour > 12 ? hour - 12 : hour);
        string period = hour < 12 ? "AM" : "PM";
        return $"{day} {displayHour:D2}:{minute:D2} {period}";
    }
}

[System.Serializable]
public struct ScheduleTime
{
    [Range(0, 23)] public int hour;
    [Range(0, 59)] public int minute;

    public int TotalMinutes => (hour * 60) + minute;
}

[System.Serializable]
public struct ScheduleEntry
{
    public ScheduleTime time;
    public LocationData targetLocation;
    public string actionDescription; // e.g., "Working the fields", "Sleeping"
}

[System.Serializable]
public struct DaySchedule
{
    public GameDay day;
    public List<ScheduleEntry> entries;
}

[CreateAssetMenu(fileName = "NewNPCData", menuName = "NPC System/NPC Data")]
public class NPCData : ScriptableObject
{
    [SerializeField] private string npcName;
    [SerializeField] private string occupation;
    [SerializeField] private Color skinColor = Color.white;
    [SerializeField] private List<DaySchedule> weeklySchedule;

    public string NpcName => npcName;
    public string Occupation => occupation;
    public Color SkinColor => skinColor;
    public IReadOnlyList<DaySchedule> WeeklySchedule => weeklySchedule;
}