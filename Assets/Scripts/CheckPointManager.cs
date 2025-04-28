using UnityEngine;

public class CheckPointManager : Singleton<CheckPointManager>
{
    public delegate void OnCheckPointPassed(float time);
    public OnCheckPointPassed onCheckPointPassed;
}
