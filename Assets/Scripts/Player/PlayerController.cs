using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private IMovable _movement;
    private PlayerInput _playerInput;

    private readonly Vector3[] _moveDirections =
    {
        Vector3.right,
        Vector3.forward,
    };

    private readonly int[] _moveTraceX =
{
        -1,
        0,
        1,
    };

    private int _currentDirectionIndex = 0;
    private int _currentTraceIndex = 0;

    public Vector3 GetDirection => _moveDirections[_currentDirectionIndex];

    public void OnEnable()
    {
        _movement = GetComponent<IMovable>();
        _playerInput = GetComponent<PlayerInput>();

        ChangeDirection();
    }

    public void Update()
    {
        var horizontalInput = _playerInput.MoveInput.x;
        _movement?.Move(1f, GetDirection + new Vector3(horizontalInput, 0));
    }

    public void ChangeDirection()
    {
        _currentDirectionIndex = (_currentDirectionIndex + 1) % 2;

        var newDirection = _moveDirections[_currentDirectionIndex];
        _movement?.ChangeDirection(newDirection);
    }

    private void OnDestroy()
    {
        if(_playerInput != null)
            _playerInput.OnClick -= ChangeDirection;
    }
}
