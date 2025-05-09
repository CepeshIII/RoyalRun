using UnityEngine;

public delegate void PlayerEvent();

public class Player: MonoBehaviour
{
    public PlayerEvent OnItemCollect;
    public PlayerEvent OnPlayerFall;
    public PlayerEvent OnPlayerWalk;
    public PlayerEvent OnPlayerStumble;

    private IMovable _movement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectableItem"))
        {
            OnItemCollect?.Invoke();
        }
    }

    private void OnEnable()
    {
    }

    private void OnDestroy()
    {
    }
}
