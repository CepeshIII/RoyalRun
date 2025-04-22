using UnityEngine;

public class GameModeChanger : MonoBehaviour
{
    public void Start()
    {
        PlayerPrefs.SetInt("IsPlayerShouldRun", true ? 1: 0 );
    }

    public void Change(bool isRunning)
    {
        PlayerPrefs.SetInt("IsPlayerShouldRun", isRunning ? 1: 0 );
    }
}
