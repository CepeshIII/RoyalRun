using Unity.Cinemachine;
using UnityEngine;

public class CinemachineInitializer : MonoBehaviour
{
    private CinemachineCamera cam;

    private void OnEnable()
    {
        cam = GetComponent<CinemachineCamera>();
    }

    public void Initialize(Player player)
    {
        if(player != null)
        {
            cam.Target.TrackingTarget = player.transform;
        }
    }

}
