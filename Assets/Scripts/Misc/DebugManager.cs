using System;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public static DebugManager Instance;
    public bool timeStopped;
    public bool omniDrink;

    TimeSpan _timeSpan = new TimeSpan(7,30,0);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText(_timeSpan.ToString(@"hh\:mm"));
    }

    public void ToggleTimeStopped() => timeStopped = !timeStopped;
    public void ToggleOmnidrink() => omniDrink = !omniDrink;
    
    public void RemoveHour() => _timeSpan = new TimeSpan(_timeSpan.Hours - 1, _timeSpan.Minutes, 0);
    public void AddHour() => _timeSpan = new TimeSpan(_timeSpan.Hours + 1, _timeSpan.Minutes, 0);
    public void RemoveMinute() => _timeSpan = new TimeSpan(_timeSpan.Hours, _timeSpan.Minutes - 1, 0);
    public void AddMinute() => _timeSpan = new TimeSpan(_timeSpan.Hours, _timeSpan.Minutes + 1, 0);
    public void SetTime() => TimeManager.Instance.SetTime(_timeSpan);
}
