using UnityEngine;

public class TestMovement : MonoBehaviour, IMovable
{
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private float heightToCheck;
    private RaycastHit _hit;

    private void LateUpdate()
    {
        if (IsGrounded()) 
        {
            Debug.Log("I grounded");
        }
    }

    public bool IsGrounded()
    {
        var position = transform.position;
        var direction = Vector3.down;

        var IsGrounded = Physics.Raycast(position, direction, out _hit);

        return IsGrounded;
    }

    public void ChangeDirection(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }

    public void ChangeTrace(int traceX)
    {
        throw new System.NotImplementedException();
    }

    public void ConnectToMoveEvent(IMovableEvent moveEvent)
    {
        throw new System.NotImplementedException();
    }

    public void DisconnectFromMoveEvent(IMovableEvent moveEvent)
    {
        throw new System.NotImplementedException();
    }

    public MovementEventsContainer GetMovementEventsContainer()
    {
        throw new System.NotImplementedException();
    }

    public void Move(float speed, Vector3 direction)
    {
        throw new System.NotImplementedException();
    }
}
