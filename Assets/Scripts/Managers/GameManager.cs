using System;
using System.Collections;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameTimer))]
public class GameManager: MonoBehaviour
{
    public event Action OnStartGame;
    public event Action OnGameOver;
    public event Action OnCheckPointPass;

    [Header("Core References")]
    [SerializeField, Tooltip("Player controller")] private Player _player;
    [SerializeField, Tooltip("Path tile generator")] private PathGenerator _pathGenerator;
    [SerializeField, Tooltip("Managed scene transitions")] private SceneLoader _sceneLoader;
    [SerializeField, Tooltip("Score tracking")] private ScoreManager _scoreManager;
    [SerializeField, Tooltip("Throwable objects spawner")]
    private ThrowableObjectManager _throwableManager;

    [Header("Game Systems (Auto-assigned)")]
    [SerializeField, ReadOnly] private GameTimer _gameTimer;
    [SerializeField, ReadOnly] private CheckPointManager _checkPointManager;
    [SerializeField, ReadOnly] private GameSoundManager _soundManager;
    [SerializeField, ReadOnly] private PoolManager _poolManager;
    [SerializeField, ReadOnly] private CinemachineInitializer _cinemachineInitializer;
    [SerializeField, ReadOnly] private Fog _fog;
    [SerializeField, ReadOnly] private ObstacleGenerator _obstacleGenerator;
    [SerializeField, ReadOnly] private PlayerInitializer _playerInitializer;

    [Header("Settings")]
    [SerializeField, Range(-100f, 0f)] private float _fallThreshold = -20f;
    [SerializeField, Range(0f, 5f)] private float _postGameDelay = 2f;

    private bool _isGameOver = false;


    private void OnEnable()
    {
        if (_playerInitializer != null)
            _player = _playerInitializer.PlayerInitialize();
        else
            _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();

        _gameTimer = GetComponent<GameTimer>();
        _gameTimer.OnTimerEnd += HandleGameOver;

        _poolManager = (PoolManager)PoolManager.Instance;
        _poolManager.Initialize();

        _obstacleGenerator = GameObject.FindGameObjectWithTag("ObstacleGenerator")
            .GetComponent<ObstacleGenerator>();
        _obstacleGenerator.Initialize(_poolManager);

        _cinemachineInitializer = GameObject.FindGameObjectWithTag("MainCinemachineCamera")
            .GetComponent<CinemachineInitializer>();
        _cinemachineInitializer.Initialize(_player);

        _checkPointManager = (CheckPointManager)CheckPointManager.Instance;
        _checkPointManager.onCheckPointPassed += CheckPointPassed;

        if (GameObject.FindGameObjectWithTag("PathGenerator")
                .TryGetComponent<PathGenerator>(out _pathGenerator))
        {
            _pathGenerator.Initialize(_player, _poolManager, _obstacleGenerator);
        }

        if (GameObject.FindGameObjectWithTag("ScoreManager")
                .TryGetComponent<ScoreManager>(out _scoreManager))
        {
            _scoreManager.Initialize(_player);
        }

        if( GameObject.FindGameObjectWithTag("ThrowableObjectManager")
                .TryGetComponent<ThrowableObjectManager>(out _throwableManager))
        {
            _throwableManager.Initialize(_player, _pathGenerator, _poolManager);
        }

        if (GameObject.FindGameObjectWithTag("Fog")
                .TryGetComponent<Fog>(out _fog))
        {
            _fog.Initialize(_player);
        }

        _sceneLoader = GameObject.FindGameObjectWithTag("SceneManager")?
            .GetComponent<SceneLoader>();

        ConnectGameSoundManager();
    }

    private void Start()
    {
        OnStartGame?.Invoke();
    }

    private void Update()
    {
        if (_isGameOver) return;

        if (_gameTimer != null)
            _gameTimer.Tick(Time.deltaTime);
        CheckFallOutOfBounds();
    }

    public void ConnectGameSoundManager()
    {
        _soundManager = (GameSoundManager)GameSoundManager.Instance;
        if (_soundManager == null)
        {
            Debug.LogError("_soundManager is null");
        }

        OnStartGame += _soundManager.PlayStartGameSound;
        OnCheckPointPass += _soundManager.PlayCheckPointPassSound;
        OnGameOver += _soundManager.PlayGameOverSound;
    }

    private void CheckPointPassed(float extraTime)
    {
        _gameTimer.AddTime(extraTime);
        OnCheckPointPass?.Invoke();
    }

    private void CheckFallOutOfBounds()
    {
        if(_player.transform.position.y < _fallThreshold)
        {
            HandleGameOver();
        }
    }

    private void HandleGameOver()
    {
        if (_isGameOver) return;
        _isGameOver = true;

        OnGameOver?.Invoke();

        SavePlayerStats();
        _gameTimer.ChangeGameSpeed(0.1f);
        //_player.gameObject.SetActive(false);
        StartCoroutine(TransitionToMenu());
    }

    private void SavePlayerStats()
    {
        var bestResult = PlayerPrefs.GetInt("BestScore");
        var currentResult = _scoreManager.Score;

        if (bestResult < currentResult)
        {
            PlayerPrefs.SetInt("BestScore", currentResult);
        }

        PlayerPrefs.SetInt("LastScore", currentResult);
    }

    private IEnumerator TransitionToMenu()
    {
        yield return new WaitForSeconds(_postGameDelay);
        _gameTimer.ChangeGameSpeed(1f);
        _sceneLoader.LoadMenuScene();
        Cleanup();
    }

    private void Cleanup()
    {
        _pathGenerator.Destroy();
        _poolManager.Clear();
    }

    public void DisconnectGameSoundManager()
    {
        if (_soundManager == null) return;

        OnStartGame -= _soundManager.PlayStartGameSound;
        OnCheckPointPass -= _soundManager.PlayCheckPointPassSound;
        OnGameOver -= _soundManager.PlayGameOverSound;
    }

    private void OnDisable()
    {
        if (_gameTimer != null)
        {
            _gameTimer.OnTimerEnd -= HandleGameOver;
            Destroy(_gameTimer);
        }

        if(_checkPointManager != null)
            _checkPointManager.onCheckPointPassed -= CheckPointPassed;

        DisconnectGameSoundManager();
    }
}