using UnityEngine;

public class CheckPointManager : Singleton<CheckPointManager>
{
    public delegate void OnCheckPointPassed(float extraTime);
    public OnCheckPointPassed onCheckPointPassed;

}
