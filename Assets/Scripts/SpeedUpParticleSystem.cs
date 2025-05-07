using UnityEngine;

public class SpeedUpParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    [SerializeField, Range(0, 100)] private float _maxRateOverTime = 50;
    [SerializeField] private AnimationCurve _rateOverTimeCurve;

    public void OnEnable()
    {
        _particleSystem  = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// Sets the rate over time of the particle system based on a value between 0 and 1.
    /// </summary>
    /// <param name="t">A value between 0 and 1 used for interpolation.</param>
    public void SetRateOverTime(float t)
    {
        var newRate = _rateOverTimeCurve.Evaluate(Mathf.Clamp01(t)) * _maxRateOverTime;

        var emissionModule = _particleSystem.emission;
        var minMaxCurve = emissionModule.rateOverTime;

        minMaxCurve.constant = newRate;
        emissionModule.rateOverTime = minMaxCurve;
    }
}
