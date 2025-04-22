using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayerEvent();

public class Player: MonoBehaviour
{
    public PlayerEvent OnItemCollect;
    public PlayerEvent OnPlayerFall;
    public PlayerEvent OnPlayerWalk;

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
        _movement = GetComponent<IMovable>();
        if(_movement != null ) 
            _movement.GetMovementEventsContainer().OnFall += () => OnPlayerFall?.Invoke();
    }

    private void OnDestroy()
    {
        if (_movement != null) 
            _movement.GetMovementEventsContainer().OnFall -= () => OnPlayerFall?.Invoke();
    }
}
