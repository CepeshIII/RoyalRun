using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ThrowableObjectManager : MonoBehaviour
{
    public event Action<GameObject> OnObjectThrown;

    [Header("References")]
    [Tooltip("Available throwable object types (loaded at runtime)")]
    [SerializeField] private List<ThrowableObjectType> _throwableTypes = new List<ThrowableObjectType>();

    [Tooltip("Prefab for the main part of a throwable object")]
    [SerializeField] private GameObject _objectPrefab;

    [Header("Managers (Auto-assigned)")]
    [SerializeField] private TilePoolManager _poolManager;
    [SerializeField] private PathGenerator _pathGenerator;

    [Header("Spawn Settings")]
    [Tooltip("Distance ahead of _player where objects spawn")]
    [SerializeField] private float _spawnDistance = 20f;
    [Tooltip("Height where objects spawn")]
    [SerializeField] private float _spawnHeight = 2;
    [Tooltip("Angle of throwing the object of the leisure (degrees)")]
    [SerializeField] private float _angleToGround = 45;
    [Tooltip("Initial delay between throws (seconds)")]
    [SerializeField] private float _initialDelay = 2f;
    [Tooltip("Minimum delay between throws (seconds)")]
    [SerializeField] private float _minDelay = 0.5f;
    [Tooltip("Rate at which delay decreases per throw")]
    [SerializeField] private AnimationCurve _delayCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Throw Physics")]
    [Tooltip("Impulse force applied to thrown objects")]
    [SerializeField] private float _throwForce = 100f;

    [SerializeField] private Transform _playerTransform;
    float _currentDelay = 0f;
    private int _throwCount;


    public void Initialize(Player player, PathGenerator pathGenerator, TilePoolManager poolManager)
    {
        if (player == null) throw new ArgumentNullException(nameof(player));
         _playerTransform = player.transform;

        if (pathGenerator == null) throw new ArgumentNullException(nameof(pathGenerator));
        _pathGenerator = pathGenerator;

        if (poolManager == null) throw new ArgumentNullException(nameof(poolManager));
        _poolManager = poolManager;
    }

    private void Awake()
    {
        // Load throwable types from Resources folder if empty
        if (_throwableTypes == null || _throwableTypes.Count == 0)
        {
            var path = Path.Combine("AssetDatabase", "ThrewObjects");

            _throwableTypes = new List<ThrowableObjectType>(
                Resources.LoadAll<ThrowableObjectType>(path)
            );
        }

        _currentDelay = _initialDelay;
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_currentDelay);
            SpawnAndThrow();
            _throwCount++;
            UpdateDelay();
        }
    }

    private void SpawnAndThrow()
    {
        if (_playerTransform == null || _objectPrefab == null || _pathGenerator == null)
            return;

        var spawnPosition = _playerTransform.position + Vector3.forward * _spawnDistance;
        spawnPosition.y = _spawnHeight; 

        var lastTile = _pathGenerator.GetLastTile();
        if (lastTile == null) return;

        // Get random type
        var type = _throwableTypes[UnityEngine.Random.Range(0, _throwableTypes.Count)];

        // Spawn from pool
        var go = _poolManager.AddCachedObject(
            tile: lastTile,
            prefab: _objectPrefab,
            position: spawnPosition,
            identifier: type.name
        );

        // Initialize
        if (go.TryGetComponent(out ThrowableObject tobj))
            tobj.Initialize(type);

        // Apply physics
        if (go.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.rotation = UnityEngine.Random.rotation;

            var dir = Quaternion.Euler(-_angleToGround, 0f, 0f) * -Vector3.forward;
            rb.AddForce(dir * _throwForce, ForceMode.Impulse);
        }

        OnObjectThrown?.Invoke(go.gameObject);
    }

    /// <summary>
    /// Updates the delay based on the number of throws and the delay curve.
    /// </summary>
    private void UpdateDelay()
    {
        var t = Mathf.Clamp01((float)_throwCount / 100f);
        var curveVal = _delayCurve.Evaluate(t);
        _currentDelay = Mathf.Lerp(_initialDelay, _minDelay, curveVal);
    }
}
