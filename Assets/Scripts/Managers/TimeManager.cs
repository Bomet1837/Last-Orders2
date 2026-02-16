using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{

    public static event Action OnMinuteChange;
    public static TimeManager Instance;
    
    [SerializeField] [Tooltip("How long a day lasts in real life minutes")] 
    int shiftLength;
    
    [SerializeField] [Tooltip("24 hour format")] [Range(0,24)]
    int shiftStartHour;
    
    [SerializeField] [Range(0,59)]
    int shiftStartMinute;
    
    [SerializeField] [Tooltip("24 hour format")] [Range(0,24)]
    int shiftEndHour;

    [SerializeField] [Range(0,59)]
    int shiftEndMinute;
    
    float _secondsElapsed;
    
    TimeSpan _shiftStart;
    TimeSpan _shiftEnd;
    TimeSpan _lastSpan = TimeSpan.Zero;

    public TimeSpan currentTime;

    void Awake()
    {
        _shiftStart = new TimeSpan(shiftStartHour, shiftStartMinute, 0);
        _shiftEnd = new TimeSpan(shiftEndHour, shiftEndMinute, 0);
        
        if (Instance) Debug.LogError("There is more than one time manager in the scene. there should only be 1!");
        Instance = this;
    }

    void Update()
    {
        if(DebugManager.Instance.timeStopped) return;
        _secondsElapsed += Time.deltaTime;

        float progress = _secondsElapsed / (shiftLength * 60);

        TimeSpan difference = _shiftEnd - _shiftStart;

        TimeSpan current = _shiftStart.Add(difference * progress);

        if(current.Minutes != _lastSpan.Minutes) OnMinuteChange?.Invoke();
        
        currentTime = current;

        string time = current.ToString(@"hh\:mm");

        UIManager.Instance.timeText.SetText(time);
        _lastSpan = current;
    }

    public void SetTime(TimeSpan span)
    {
        if (span < _shiftStart) span = _shiftStart;
        if (span > _shiftEnd) span = _shiftEnd;

        TimeSpan shiftDuration = _shiftEnd - _shiftStart;
        
        float progress = (float)((span - _shiftStart).TotalSeconds / shiftDuration.TotalSeconds);
        
        _secondsElapsed = progress * (shiftLength * 60f);
        
        currentTime = span;
        _lastSpan = span;

        UIManager.Instance.timeText.SetText(span.ToString(@"hh\:mm"));
    }
}


