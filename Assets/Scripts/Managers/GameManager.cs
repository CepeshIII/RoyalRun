using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void GameEvents();
public class GameManager: MonoBehaviour
{
    public GameEvents OnStartGame;
    public GameEvents OnGameOver;
    public GameEvents OnCheckPointPass;

    [SerializeField] Player _player;
    [SerializeField] PathGenerator _pathGenerator;
    [SerializeField] SceneLoader _sceneLoader;
    [SerializeField] ScoreManager _scoreManager;
    [SerializeField] GameTimer _gameTimer;
    [SerializeField] CheckPointManager _checkPointManager;
    [SerializeField] GameSoundManager _gameSoundManager;
    [SerializeField] ThrowableObjectManager _throwableObjectManager;
    [SerializeField] PlayerInitializer _playerInitializer;
    [SerializeField] CinemachineInitializer _cinemachineInitializer;
    [SerializeField] Fog _fog;

    [SerializeField] float _startTime = 14.88f;

    private bool _isGameOver = false;


    private void OnEnable()
    {
        if (_playerInitializer != null)
        {
            _player = _playerInitializer.PlayerInitialize();
        }
        else
        {
            _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        }


        if (GameObject.FindGameObjectWithTag("PathGenerator")
                .TryGetComponent<PathGenerator>(out _pathGenerator))
        {
            _pathGenerator.Initialize(_player);
        }


        if (GameObject.FindGameObjectWithTag("ScoreManager")
                .TryGetComponent<ScoreManager>(out _scoreManager))
        {
            _scoreManager.Initialize(_player);
        }

        _gameTimer = gameObject.AddComponent<GameTimer>();
        if (_gameTimer != null)
            _gameTimer.OnTimerEnd += StartGameOver;

        _checkPointManager = (CheckPointManager)CheckPointManager.Instance;
        if (_checkPointManager != null)
            _checkPointManager.onCheckPointPassed += CheckPointPassed;

        if( GameObject.FindGameObjectWithTag("ThrowableObjectManager")
                .TryGetComponent<ThrowableObjectManager>(out _throwableObjectManager))
        {
            _throwableObjectManager.Initialize(_player);
        }

        if(GameObject.FindGameObjectWithTag("MainCinemachineCamera")
                .TryGetComponent<CinemachineInitializer>(out _cinemachineInitializer))
        {
            _cinemachineInitializer.Initialize(_player);
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
        if(_gameTimer!= null)
            _gameTimer.SetTimer(_startTime);
        OnStartGame?.Invoke();
    }

    private void Update()
    {
        CheckIfGameOver();
        if (_gameTimer != null)
            _gameTimer.UpdateTime();
    }


    public void ConnectGameSoundManager()
    {
        _gameSoundManager = (GameSoundManager)GameSoundManager.Instance;
        if (_gameSoundManager == null)
        {
            Debug.LogError("_gameSoundManager is null");
        }

        OnStartGame += _gameSoundManager.PlayStartGameSound;
        OnCheckPointPass += _gameSoundManager.PlayCheckPointPassSound;
        OnGameOver += _gameSoundManager.PlayGameOverSound;
    }

    private void CheckIfGameOver()
    {
        if(_player.transform.position.y < -20 && !_isGameOver)
        {
            StartGameOver();
        }
    }

    private void SavePlayerStats()
    {
        var bestResult = PlayerPrefs.GetInt("BestScore");
        var currentResult = _scoreManager.Score;

        if(bestResult < currentResult)
        {
            PlayerPrefs.SetInt("BestScore", currentResult);
        }

        PlayerPrefs.SetInt("LastScore", currentResult);
    }

    private void CheckPointPassed(float addTime)
    {
        _gameTimer.AddTime(addTime);
        OnCheckPointPass?.Invoke(); 
    }

    private void StartGameOver()
    {
        if(!_isGameOver)
            StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        _isGameOver = true;
        OnGameOver?.Invoke();
        SavePlayerStats();
        _player.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        _sceneLoader.LoadMenuScene();
        _pathGenerator.Destroy();
    }


    public void DisconnectGameSoundManager()
    {
        if (_gameSoundManager == null) return;

        OnStartGame -= _gameSoundManager.PlayStartGameSound;
        OnCheckPointPass -= _gameSoundManager.PlayCheckPointPassSound;
        OnGameOver -= _gameSoundManager.PlayGameOverSound;
    }

    private void OnDisable()
    {
        if (_gameTimer != null)
            Destroy(_gameTimer);

        if(_checkPointManager != null)
            _checkPointManager.onCheckPointPassed -= CheckPointPassed;

        DisconnectGameSoundManager();
    }
}