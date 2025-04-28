using TMPro;
using UnityEngine;

public delegate void  GameTimerEvents();

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _shower;
    public GameTimerEvents OnTimerEnd;

    private float _time = 0f;

    private void OnEnable()
    {
        _shower = GameObject.FindGameObjectWithTag("GameTimerShower").GetComponent<TextMeshProUGUI>();
    }

    public void SetTimer(float startTime)
    {
        _time = startTime;
        UpdateShower();
    }

    public void UpdateTime()
    {
        _time -= Time.deltaTime;

        if( _time <= 0f)
        {
            OnTimerEnd?.Invoke();
        }
        UpdateShower();
    }

    public void AddTime(float time)
    {
        _time += time;
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
