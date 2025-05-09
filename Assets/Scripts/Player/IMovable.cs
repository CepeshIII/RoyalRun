using UnityEngine;

public delegate void IMovableEvent();

public interface IMovable
{
    public void Move(float speed, Vector3 direction);
    public void ChangeDirection(Vector3 direction);
    public void Jump();

}