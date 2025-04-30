using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    [SerializeField] private LayerMask obstaclesMask;
    [SerializeField] private Animator _animator;

    public static bool IsInLayerMask(GameObject obj, LayerMask mask) => (mask.value & (1 << obj.layer)) != 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject, obstaclesMask))
        {
            _animator.SetTrigger("Stumbled");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsInLayerMask(other.gameObject, obstaclesMask))
        {
            _animator.SetTrigger("Stumbled");
            GetComponent<PlayerMovement>()?.Stumble();
        }
    }

}
