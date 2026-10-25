using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timeText; 
    [SerializeField] private RectTransform clockHand; 
    [SerializeField] private Button fastForwardButton;
    [SerializeField] private Button normalSpeedButton;

    [Header("Settings")]
    [SerializeField] private float fastForwardSpeed = 5f;

    private void OnEnable()
    {
        TimeEvents.OnTimeChanged += UpdateUI;
        fastForwardButton.onClick.AddListener(OnFastForwardClicked);
        normalSpeedButton.onClick.AddListener(OnNormalSpeedClicked);
    }

    private void OnDisable()
    {
        TimeEvents.OnTimeChanged -= UpdateUI;
        fastForwardButton.onClick.RemoveListener(OnFastForwardClicked);
        normalSpeedButton.onClick.RemoveListener(OnNormalSpeedClicked);
    }

    private void UpdateUI(GameTime time)
    {
        if (timeText != null)
        {
            string timeOfDay = (time.hour >= 6 && time.hour < 18) ? "Morning" : "Night";
            timeText.text = $"Time: {timeOfDay} | {time.GetAMPMFormat()}";
        }

        if (clockHand != null)
        {
            int displayHour = time.hour % 12;
            float rotation = (displayHour * 30f) + (time.minute * 0.5f);
            clockHand.localRotation = Quaternion.Euler(0, 0, -rotation);
        }
    }

    public void OnFastForwardClicked()
    {
        TimeEvents.OnTimeMultiplierChanged?.Invoke(fastForwardSpeed);
    }

    public void OnNormalSpeedClicked()
    {
        TimeEvents.OnTimeMultiplierChanged?.Invoke(1f);
    }
}