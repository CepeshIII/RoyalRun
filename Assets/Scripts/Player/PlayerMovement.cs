using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementEventsContainer 
{
    public IMovableEvent OnMove;
    public IMovableEvent OnFall;
}


public class PlayerMovement : MonoBehaviour, IMovable
{
    private IMovableEvent OnMove;
    private MovementEventsContainer _movementEventsContainer;

    public IMovableEvent OnStumbled;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private bool _isFalling = false;
    [SerializeField] private Animator _animator;
    [SerializeField] private CapsuleCollider _collider;

    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Vector3 _forwardDirection;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _runningSpeed = 10f;

    [SerializeField] private float _angularSpeed = 5f;

    [SerializeField] private bool _isRunning = false;
    [SerializeField] private bool _isGrounded = false;

    [SerializeField] private float _verticalSpeedToStartFall = 1f;
    [SerializeField] private float heightToCheck;
    [SerializeField] private float jumpForce = 10000f;
    [SerializeField] private Vector3 velocity = Vector3.zero;

    private float nextX = 0;


    private RaycastHit _hit;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<CapsuleCollider>();
        _forwardDirection = transform.forward;
        _animator.SetBool("Falling", _isFalling);

        if (PlayerPrefs.GetInt("IsPlayerShouldRun") == 1)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }

    private void FixedUpdate()
    {
        _isGrounded = IsGrounded();
        if (_isGrounded)
        {
            _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, Quaternion.LookRotation(_forwardDirection), _angularSpeed));
        }

        CheckIfFalling();

        _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, Quaternion.LookRotation(_forwardDirection), _angularSpeed / 5f));

        //velocity = Vector3.zero;
        _rb.position += velocity * Time.fixedDeltaTime;
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //_animator.SetTrigger("Jump");
        }
    }

    public void ChangeDirection(Vector3 direction)
    {
        if (_isGrounded)
            _forwardDirection = direction;
    }

    public void Move(float speedMultiplier, Vector3 direction)
    {
        if (_isGrounded)
        {
            float speed;
            if (_isRunning)
            {
                speed = _runningSpeed * speedMultiplier;
                _animator.SetFloat("VerticalSpeed", 1f);
            }
            else
            {
                speed = _speed * speedMultiplier;
                _animator.SetFloat("VerticalSpeed", 0.5f);
            }

            var newPosition = _rb.position + speed * direction.normalized;
            //_rb.Move(newPosition, _rb.rotation);
            velocity = direction.normalized * speed;

            //_rb.linearVelocity += speed * direction.normalized;
            OnMove?.Invoke();

            _animator.SetFloat("HorizontalSpeed", direction.x);
        }
    }

    public void CheckIfFalling()
    {
        var isFalling = (_rb.linearVelocity.y) < -_verticalSpeedToStartFall;
        var startToFall = isFalling && !_isFalling;
        var stopFalling = !isFalling && _isFalling;

        if (startToFall)
        {
            _isFalling = true;
            _forwardDirection = Vector3.forward;
            _movementEventsContainer.OnFall?.Invoke();
        }
        else if (stopFalling)
        {
            _isFalling = false;
        }

        _animator.SetBool("Falling", _isFalling);
    }

    public bool IsGrounded()
    {
        var position = _collider.bounds.center;
        var direction = Vector3.down;

        var IsGrounded = Physics.Raycast(position, direction, 
                                    out _hit, _collider.height + heightToCheck);
        if (IsGrounded) 
        {
            //Debug.Log("Height: " + Vector3.Distance(_hit.point, position));
        }

        return IsGrounded;
    }

    public void Stumble()
    {

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    _isFalling = !IsGrounded();
    //    _animator.SetBool("Falling", _isFalling);
    //}

    public void ConnectToMoveEvent(IMovableEvent moveEvent)
    {
        OnMove += moveEvent;
    }

    public void DisconnectFromMoveEvent(IMovableEvent moveEvent)
    {
        if(OnMove != null)
            OnMove -= moveEvent;
    }

    public MovementEventsContainer GetMovementEventsContainer()
    {
        if (_movementEventsContainer == null)
            _movementEventsContainer = new MovementEventsContainer();
        return _movementEventsContainer;
    }
}
