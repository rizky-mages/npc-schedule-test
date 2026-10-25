using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float realSecondsPerGameMinute = 0.5f;
    [SerializeField] private GameTime currentTime;
    
    private float timer;
    private float timeMultiplier = 1f;

    public static GameTime CurrentTime { get; private set; } 

    private void OnEnable()
    {
        TimeEvents.OnTimeMultiplierChanged += SetTimeMultiplier;
    }

    private void OnDisable()
    {
        TimeEvents.OnTimeMultiplierChanged -= SetTimeMultiplier;
    }

    private void Start()
    {
        CurrentTime = currentTime;
        TimeEvents.OnTimeChanged?.Invoke(currentTime);
    }

    private void Update()
    {
        timer += Time.deltaTime * timeMultiplier;
        
        if (timer >= realSecondsPerGameMinute)
        {
            timer = 0f;
            AdvanceTime();
        }
    }

    private void AdvanceTime()
    {
        int totalMins = currentTime.TotalMinutes + 1;
        
        if (totalMins >= 1440)
        {
            totalMins = 0;
            int nextDayIndex = ((int)currentTime.day + 1) % 7;
            currentTime.day = (GameDay)nextDayIndex;
        }

        currentTime.hour = totalMins / 60;
        currentTime.minute = totalMins % 60;
        
        CurrentTime = currentTime;
        
        TimeEvents.OnTimeChanged?.Invoke(currentTime);
    }

    private void SetTimeMultiplier(float multiplier)
    {
        timeMultiplier = multiplier;
    }
}