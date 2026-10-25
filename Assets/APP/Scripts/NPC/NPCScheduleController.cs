using UnityEngine;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(IMovable))]
public class NPCScheduleController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer skinRenderer; 
    [SerializeField] private Canvas nameCanvas;
    [SerializeField] private TMP_Text npcName;
    
    private NPCData npcData;
    private LocationRegistry locationRegistry;
    private IMovable movementHandler;
    private LocationData activeLocation; 
    private ScheduleEntry? currentActiveEntry;

    private bool isFirstSpawn = true;

    public void Initialize(NPCData data, LocationRegistry registry, Camera eventCamera)
    {
        npcData = data;
        locationRegistry = registry;
        movementHandler = GetComponent<IMovable>();

        if (skinRenderer != null && npcData != null) skinRenderer.color = npcData.SkinColor;
        if (nameCanvas != null && eventCamera != null) nameCanvas.worldCamera = eventCamera;
        if (npcName != null && npcData != null) npcName.text = npcData.NpcName;

        CheckSchedule(TimeManager.CurrentTime);
    }

    private void OnEnable()
    {
        TimeEvents.OnTimeChanged += CheckSchedule;
    }

    private void OnDisable()
    {
        TimeEvents.OnTimeChanged -= CheckSchedule;
    }

    private void CheckSchedule(GameTime time)
    {
        if (npcData == null || npcData.WeeklySchedule.Count == 0) return;

        ScheduleEntry? activeEntry = null;
        int currentMins = time.TotalMinutes;

        List<ScheduleEntry> todayEntries = GetEntriesForDay(time.day);

        if (todayEntries != null)
        {
            for (int i = 0; i < todayEntries.Count; i++)
            {
                ScheduleEntry entry = todayEntries[i];
                if (entry.time.TotalMinutes <= currentMins)
                {
                    activeEntry = entry;
                }
            }
        }

        if (activeEntry == null)
        {
            int yesterdayIndex = ((int)time.day - 1 + 7) % 7;
            List<ScheduleEntry> yesterdayEntries = GetEntriesForDay((GameDay)yesterdayIndex);
            
            if (yesterdayEntries != null && yesterdayEntries.Count > 0)
            {
                activeEntry = yesterdayEntries[yesterdayEntries.Count - 1];
            }
            else
            {
                activeEntry = npcData.WeeklySchedule[0].entries[0]; 
            }
        }

        LocationData targetLoc = activeEntry.Value.targetLocation;
        
        if (activeLocation != targetLoc || isFirstSpawn)
        {
            activeLocation = targetLoc;
            Transform targetWaypoint = locationRegistry.GetWaypoint(targetLoc);
            
            if (targetWaypoint != null)
            {
                currentActiveEntry = activeEntry;

                if (isFirstSpawn)
                {
                    transform.position = targetWaypoint.position;
                    
                    if (TryGetComponent<Rigidbody2D>(out var rb))
                    {
                        rb.position = targetWaypoint.position;
                    }
                    
                    isFirstSpawn = false; 
                }

                movementHandler.MoveTo(targetWaypoint.position);
            }
        }
    }

    private List<ScheduleEntry> GetEntriesForDay(GameDay day)
    {
        for (int i = 0; i < npcData.WeeklySchedule.Count; i++)
        {
            DaySchedule daySchedule = npcData.WeeklySchedule[i];
            if (daySchedule.day == day) return daySchedule.entries;
        }
        return null;
    }

    public string GetCurrentActionContext()
    {
        if (currentActiveEntry == null) return "Just hanging around.";

        if (!movementHandler.IsAtDestination)
        {
            string locationName = currentActiveEntry.Value.targetLocation.LocationName;
            return $"Heading to {locationName}...";
        }
        return currentActiveEntry.Value.actionDescription;
    }
}