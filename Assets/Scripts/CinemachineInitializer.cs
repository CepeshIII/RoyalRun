using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineInitializer : MonoBehaviour
{
    [SerializeField] private float pauseBeforeFollow = 2f;

    private CinemachineCamera _cam;
    private Player _player;

    private void OnEnable()
    {
        _cam = GetComponent<CinemachineCamera>();
    }

    public void Initialize(Player player)
    {
        if(player != null)
        {
            _player = player;
            StartCoroutine(StartCameraFollow());
        }
    }

    private IEnumerator StartCameraFollow()
    {
        yield return new WaitForSeconds(pauseBeforeFollow);
        _cam.Target.TrackingTarget = _player.transform;

    }
}
