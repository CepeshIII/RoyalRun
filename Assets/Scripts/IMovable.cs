using System;
using Unity.VisualScripting;
using UnityEngine;

public delegate void IMovableEvent();

public interface IMovable
{
    public void Move(float speed, Vector3 direction);
    public void ChangeDirection(Vector3 direction);
    public void Jump();


    public void ConnectToMoveEvent(IMovableEvent moveEvent);

    public void DisconnectFromMoveEvent(IMovableEvent moveEvent);

    public MovementEventsContainer GetMovementEventsContainer();

}