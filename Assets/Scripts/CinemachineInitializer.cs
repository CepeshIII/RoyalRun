using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineInitializer : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _vcam;
    [SerializeField] private float _delayBeforeFollow = 2f;
    [SerializeField, Range(40f, 120f)] private float _minFov = 60f;
    [SerializeField, Range(40f, 120f)] private float _maxFov = 90f;

    private PlayerMovement _playerMove;

    private void OnEnable()
    {
        _vcam = GetComponent<CinemachineCamera>();
    }

    public void Initialize(Player player)
    {
        _playerMove = player.GetComponent<PlayerMovement>();
        if (_playerMove != null && _vcam != null) 
        {
            _playerMove.OnAccelerationLevelUpdated += HandleAcceleration;
            StartCoroutine(DelayedFollow(player.transform));
        }
    }

    private IEnumerator DelayedFollow(Transform target)
    {
        yield return new WaitForSeconds(_delayBeforeFollow);
        _vcam.Follow = target;
        _vcam.LookAt = target;
    }

    private void HandleAcceleration(float accel)
    {
       _vcam.Lens.FieldOfView = Mathf.Lerp(_minFov, _maxFov, accel);
    }

    private void OnDisable()
    {
        if (_playerMove != null)
            _playerMove.OnAccelerationLevelUpdated -= HandleAcceleration;
    }
}
