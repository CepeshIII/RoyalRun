using System;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public event Action OnTimerEnd;

    [SerializeField] private TextMeshProUGUI _shower;
    [SerializeField] float _initialCountdown = 14.88f;

    private float _time = 0f;

    private void OnEnable()
    {
        if (_shower == null)
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

        if (_time <= 0f)
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
        if (_shower != null)
            _shower.text = FormatTime(_time);
    }

    public string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Changes the game speed by adjusting the time scale and fixed delta time.
    /// </summary>
    /// <param name="value">
    /// The desired time scale. Clamped between 0 (paused) and 1 (normal speed).
    /// </param>
    public void ChangeGameSpeed(float value)
    {
        // Clamp value to [0, 1] range to avoid invalid time scale values.
        Time.timeScale = Mathf.Clamp01(value);

        // Adjust the fixed update interval to match the new time scale.
        // 0.02f is the
        // ault fixedDeltaTime when timeScale is 1.
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
