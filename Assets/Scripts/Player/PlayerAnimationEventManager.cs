using UnityEngine;

public class PlayerAnimationEventManager : MonoBehaviour
{
    [SerializeField] private Player player;

    private void OnEnable()
    {
        player = GetComponentInParent<Player>();
    }

    public void FootStepEvent()
    {
        if (player != null)
        {
            player.OnPlayerWalk?.Invoke();
        }
    }
}
