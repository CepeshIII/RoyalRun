using System;
using UnityEngine;

public struct CheckPointSettings
{
    public float startExtraTime;
    public float minExtraTime;
    public float reduceTimeStep;
    public int triggersPerReductionStep;

    public CheckPointSettings(float startExtraTime, float minExtraTime, 
        float reduceTimeStep, int triggersPerReductionStep)
    {
        this.startExtraTime = startExtraTime;
        this.minExtraTime = minExtraTime;
        this.reduceTimeStep = reduceTimeStep;
        this.triggersPerReductionStep = triggersPerReductionStep;
    }
}

public class CheckPointManager : Singleton<CheckPointManager>
{
    [Header("ExtraTime Settings")]
    [SerializeField] float startExtraTime = 12f;
    [SerializeField] float minExtraTime = 4f;
    [SerializeField] float reduceTimeStep = 1f;
    [SerializeField] int triggersPerReductionStep = 10;

    public delegate void OnCheckPointPassed(float extraTime);
    public OnCheckPointPassed onCheckPointPassed;

    public CheckPointSettings GetCheckPointSettings()
    {
        return new(startExtraTime, minExtraTime, 
            reduceTimeStep, triggersPerReductionStep);
    }
}
