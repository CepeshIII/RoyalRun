using Unity.Cinemachine;
using UnityEngine;

public class CinemachineInitializer : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private CinemachineCamera cam;

    private void OnEnable()
    {
        cam = GetComponent<CinemachineCamera>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager.Player != null)
            cam.Target.TrackingTarget = gameManager.Player.transform;
    }


    private void Update()
    {
        if(cam.Target.TrackingTarget == null)
        {
            var player = gameManager.Player;
            if (player != null)
                cam.Target.TrackingTarget = player.transform;
        }
    }
}
