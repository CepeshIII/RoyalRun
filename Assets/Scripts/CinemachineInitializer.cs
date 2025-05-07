using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineInitializer : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _vcam;
    [SerializeField] private SpeedUpParticleSystem _speedUpParticleSystem;

    [SerializeField] private float _delayBeforeFollow = 2f;
    [SerializeField, Range(40f, 120f)] private float _minFOV = 60f;
    [SerializeField, Range(40f, 120f)] private float _maxFOV = 90f;
    [SerializeField] private AnimationCurve _FOVCurve;
    
    private PlayerMovement _playerMove;

    private void OnEnable()
    {
        _vcam = GetComponent<CinemachineCamera>();
        _speedUpParticleSystem = GetComponentInChildren<SpeedUpParticleSystem>();
    }

    public void Initialize(Player player)
    {
        _playerMove = player.GetComponent<PlayerMovement>();
        if (_playerMove != null && _vcam != null) 
        {
            _playerMove.OnAccelerationLevelUpdated += SetFieldOfView;
            StartCoroutine(DelayedFollow(player.transform));
        }

        if(_speedUpParticleSystem != null)
        {
            _playerMove.OnAccelerationLevelUpdated += _speedUpParticleSystem.SetRateOverTime;
        }
    }

    private IEnumerator DelayedFollow(Transform target)
    {
        yield return new WaitForSeconds(_delayBeforeFollow);
        _vcam.Follow = target;
        _vcam.LookAt = target;
    }

    /// <summary>
    /// Sets the field of view of the virtual camera based on a value between 0 and 1.
    /// </summary>
    /// <param name="t">A value between 0 and 1 used for interpolation.</param>
    public void SetFieldOfView(float t)
    {
        _vcam.Lens.FieldOfView = Mathf.Lerp(_minFOV, _maxFOV, _FOVCurve.Evaluate(Mathf.Clamp01(t)));
    }

    private void OnDisable()
    {
        if (_playerMove != null)
        {
            _playerMove.OnAccelerationLevelUpdated -= SetFieldOfView;
            if(_speedUpParticleSystem != null)
                _playerMove.OnAccelerationLevelUpdated -= _speedUpParticleSystem.SetRateOverTime;
        }

    }
}
