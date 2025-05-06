using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour, IMovable
{
    // Animation parameter hashes
    private static readonly int HorizontalSpeedHash = Animator.StringToHash("HorizontalSpeed");
    private static readonly int VerticalSpeedHash = Animator.StringToHash("VerticalSpeed");
    private static readonly int FallingHash = Animator.StringToHash("Falling");
    private static readonly int JumpTriggerHash = Animator.StringToHash("Jump");

    [Header("Components")]
    [Tooltip("CapsuleCollider for ground checks and bounds")]
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [Tooltip("Rigidbody for physics-based movement")]
    [SerializeField] private Rigidbody _rigidbody;
    [Tooltip("Animator for playback of animations")]
    [SerializeField] private Animator _animator;

    [Header("Movement Settings")]
    [Tooltip("Maximum forward (running) speed")]
    [SerializeField] private float _runSpeed = 10f;
    [Tooltip("Maximum lateral speed")]
    [SerializeField] private float _strafeSpeed = 5f;
    [Tooltip("Acceleration smoothing factor")]
    [SerializeField][Range(0f, 1f)] private float _acceleration = 0.1f;
    [SerializeField] private AnimationCurve _accelerationCurve;

    [Header("Jump & Ground")]
    [Tooltip("Force applied when jumping")]
    [SerializeField] private float _jumpForce = 7f;
    [Tooltip("Extra distance for ground check ray")]
    [SerializeField] private float _groundBuffer = 0.1f;
    [Tooltip("Layers considered as ground")]
    [SerializeField] private LayerMask _groundMask;

    // Movement state
    private Vector3 _inputDirection;
    private Vector3 _currentVelocity;
    private Vector3 _facingDirection;
    private bool _isGrounded;
    private bool _isFalling;

    // Events
    private MovementEventsContainer _events;
    public MovementEventsContainer Events => _events ??= new MovementEventsContainer();
    public event Action OnStumbled;

    private void OnEnable()
    {
        // Cache components if not set
        _rigidbody = _rigidbody ? _rigidbody : GetComponent<Rigidbody>();
        _capsuleCollider = _capsuleCollider ? _capsuleCollider : GetComponent<CapsuleCollider>();
        _animator = _animator ? _animator : GetComponentInChildren<Animator>();

        // Initial facing direction
        _facingDirection = transform.forward;
    }

    private void FixedUpdate()
    {
        UpdateGroundedStatus();
        ApplyMovement();
    }

    private void LateUpdate()
    {
        // Synchronize animation parameters
        UpdateAnimatorParameters();
    }

    /// <summary>
    /// Processes movement input externally.
    /// </summary>
    public void Move(float speedMultiplier, Vector3 direction)
    {
        if (!_isGrounded) return;

        // Store raw input for FixedUpdate
        _inputDirection = direction.normalized;
        Events.OnMove?.Invoke();

        // Set swimming/air control? Could be extended
    }

    /// <summary>
    /// Performs jump if grounded.
    /// </summary>
    public void Jump()
    {
        if (!_isGrounded) return;

        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _animator.SetTrigger(JumpTriggerHash);
    }

    /// <summary>
    /// Changes character's facing direction instantly.
    /// </summary>
    public void ChangeDirection(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        _facingDirection = direction.normalized;
    }

    /// <summary>
    /// Immediately stops movement and raises stumble event.
    /// </summary>
    public void Stumble()
    {
        _currentVelocity = Vector3.zero;
        OnStumbled?.Invoke();
    }

    private void UpdateGroundedStatus()
    {
        Vector3 origin = _capsuleCollider.bounds.center;
        float distance = _capsuleCollider.bounds.extents.y + _groundBuffer;

        bool wasGrounded = _isGrounded;
        _isGrounded = Physics.Raycast(origin, Vector3.down, out _, distance, _groundMask);

        if (_isGrounded)
        {
            _isFalling = false;
            _animator.SetBool(FallingHash, _isFalling);
        }
    }

    private void ApplyMovement()
    {
        // Determine desired velocity based on input
        Vector3 desired = transform.TransformDirection(
            new Vector3(_inputDirection.x * _strafeSpeed,
                        0f,
                        _inputDirection.z * _runSpeed)
        );

        // Smooth velocity change
        _currentVelocity = Vector3.Lerp(_currentVelocity, desired, _acceleration * _accelerationCurve.Evaluate(Mathf.InverseLerp(0f, _runSpeed, _currentVelocity.z)));

        // Move Rigidbody
        _rigidbody.MovePosition(_rigidbody.position + _currentVelocity * Time.fixedDeltaTime);
    }

    private void UpdateAnimatorParameters()
    {
        float horizontal = Vector3.Dot(transform.right, _currentVelocity) / _strafeSpeed;
        float vertical = Vector3.Dot(transform.forward, _currentVelocity) / _runSpeed;

        _animator.SetFloat(HorizontalSpeedHash, horizontal);
        _animator.SetFloat(VerticalSpeedHash, vertical);
    }

    public void ConnectToMoveEvent(IMovableEvent moveEvent)
    {
        Events.OnMove += moveEvent;
    }

    public void DisconnectFromMoveEvent(IMovableEvent moveEvent)
    {
        Events.OnMove -= moveEvent;
    }

    public MovementEventsContainer GetEventsContainer()
    {
        return Events;
    }
}
