using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour, IMovable
{
    private MovementEventsContainer _eventsContainer;
    public MovementEventsContainer EventsContainer => GetEventsContainer();

    public IMovableEvent OnStumbled;

    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Animator _animator;

    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Vector3 _forwardDirection;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float _runningSpeed = 10f;
    [SerializeField] private float _angularSpeed = 5f;
    [SerializeField] private float jumpForce = 10000f;
    [SerializeField] private float heightToCheckGround;

    [SerializeField] private bool _isFalling = false;
    [SerializeField] private bool _isRunning = false;
    [SerializeField] private bool _isGrounded = false;

    private Vector3 velocity = Vector3.zero;
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

    private void Start()
    {
        if (_isFalling) 
        {
            _isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        _isGrounded = IsGrounded();
        if (_isGrounded)
        {
            _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, Quaternion.LookRotation(_forwardDirection), _angularSpeed));
        }

        _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, Quaternion.LookRotation(_forwardDirection), _angularSpeed / 5f));

        _rb.position += velocity * Time.fixedDeltaTime;
        _animator.SetFloat("VerticalSpeed", Mathf.InverseLerp(0f, _runningSpeed, velocity.magnitude));

    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _animator.SetTrigger("Jump");
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
            var newVelocity = direction.normalized * speed;

            _animator.SetFloat("VerticalSpeed", Mathf.InverseLerp(0f, speed, velocity.magnitude));
            velocity = Vector3.Lerp(velocity, newVelocity, acceleration);

            EventsContainer.OnMove?.Invoke();

            _animator.SetFloat("HorizontalSpeed", direction.x);
        }
    }

    public bool IsGrounded()
    {
        var position = _collider.bounds.center;
        var direction = Vector3.down;

        var IsGrounded = Physics.Raycast(position, direction, 
                                    out _hit, _collider.height + heightToCheckGround);
        if (IsGrounded) 
        {
            _isFalling = false;
            _animator.SetBool("Falling", _isFalling);
        }

        return IsGrounded;
    }

    public void Stumble()
    {
        velocity = Vector3.zero;
    }


    public void ConnectToMoveEvent(IMovableEvent moveEvent)
    {
        EventsContainer.OnMove += moveEvent;
    }

    public void DisconnectFromMoveEvent(IMovableEvent moveEvent)
    {
        if(EventsContainer.OnMove != null)
            EventsContainer.OnMove -= moveEvent;
    }

    public MovementEventsContainer GetEventsContainer()
    { 
        if (_eventsContainer == null)
            _eventsContainer = new MovementEventsContainer();
        return _eventsContainer;
    }
}
