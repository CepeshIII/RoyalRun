using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    [SerializeField] private LayerMask obstaclesMask;
    [SerializeField] private Animator _animator;

    public static bool IsInLayerMask(GameObject obj, LayerMask mask) => (mask.value & (1 << obj.layer)) != 0;

    private void OnEnable()
    {
        if(_animator == null)
            _animator = GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject, obstaclesMask))
        {
            Stumble();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsInLayerMask(other.gameObject, obstaclesMask))
        {
            Stumble();
        }
    }

    private void Stumble()
    {
        _animator.SetTrigger("Stumbled");
        GetComponent<PlayerMovement>()?.Stumble();
        GetComponent<Player>()?.OnPlayerStumble();
    }
}
