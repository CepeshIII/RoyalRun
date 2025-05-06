using System;
using TMPro;
using UnityEngine;

public class GameTimer : Singleton<GameTimer>
{
    public event Action OnTimerEnd;

    [SerializeField] private TextMeshProUGUI _shower;
    [SerializeField] float _initialCountdown = 14.88f;

    private float _time = 0f;

    private void Awake()
    {
        if(_shower == null)
            _shower = GameObject.FindGameObjectWithTag("GameTimerShower").GetComponent<TextMeshProUGUI>();
    }

    public void Start()
    {
        _time = _initialCountdown;
    }

    public void SetTimer(float startTime)
    {
        _time = startTime;
        UpdateShower();
    }

    public void Tick(float deltaTime)
    {

        if( _time <= 0f)
        {
            OnTimerEnd?.Invoke();
        }
        else
        {
            _time -= deltaTime;
        }
        UpdateShower();
    }

    public void AddTime(float extraTime)
    {
        _time += extraTime;
        UpdateShower();
    }

    public void UpdateShower()
    {
        if(_shower != null)
            _shower.text = FormatTime(_time);
    }

    public string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
