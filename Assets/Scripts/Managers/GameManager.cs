using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void GameEvents();
public class GameManager: MonoBehaviour
{
    public GameEvents OnStartGame;
    public GameEvents OnGameOver;
    public GameEvents OnCheckPointPass;

    [SerializeField] Player player;
    [SerializeField] PathGenerator pathGenerator;
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] GameTimer gameTimer;
    [SerializeField] CheckPointManager checkPointManager;
    [SerializeField] GameSoundManager gameSoundManager;
    

    [SerializeField] float startTime = 14.88f;

    private bool isGameOver = false;

    private void OnEnable()
    {
        if (player == null) 
            GameObject.FindGameObjectWithTag("Player")
                .TryGetComponent<Player>(out player);

        if (pathGenerator == null)
            GameObject.FindGameObjectWithTag("PathGenerator")
                .TryGetComponent<PathGenerator>(out pathGenerator);

        if (scoreManager == null)
            GameObject.FindGameObjectWithTag("ScoreManager")
                .TryGetComponent<ScoreManager>(out scoreManager);

        gameTimer = gameObject.AddComponent<GameTimer>();
        if (gameTimer != null)
            gameTimer.OnTimerEnd += StartGameOver;

        checkPointManager = (CheckPointManager)CheckPointManager.Instance;
        if (checkPointManager != null)
            checkPointManager.onCheckPointPassed += CheckPointPassed;

        ConnectGameSoundManager();
    }

    private void Start()
    {
        if(gameTimer!= null)
            gameTimer.SetTimer(startTime);
        OnStartGame?.Invoke();
    }

    private void Update()
    {
        CheckIfGameOver();
        if (gameTimer != null)
            gameTimer.UpdateTime();
    }

    public void ConnectGameSoundManager()
    {
        gameSoundManager = (GameSoundManager)GameSoundManager.Instance;
        if (gameSoundManager == null)
        {
            Debug.LogError("gameSoundManager is null");
        }

        OnStartGame += gameSoundManager.PlayStartGameSound;
        OnCheckPointPass += gameSoundManager.PlayCheckPointPassSound;
        OnGameOver += gameSoundManager.PlayGameOverSound;
    }

    private void CheckIfGameOver()
    {
        if(player.transform.position.y < -20 && !isGameOver)
        {
            StartGameOver();
        }
    }

    private void SavePlayerStats()
    {
        var bestResult = PlayerPrefs.GetInt("BestScore");
        var currentResult = scoreManager.Score;

        if(bestResult < currentResult)
        {
            PlayerPrefs.SetInt("BestScore", currentResult);
        }

        PlayerPrefs.SetInt("LastScore", currentResult);
    }

    private void CheckPointPassed(float addTime)
    {
        gameTimer.AddTime(addTime);
        OnCheckPointPass?.Invoke(); 
    }

    private void StartGameOver()
    {
        if(!isGameOver)
            StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        isGameOver = true;
        OnGameOver?.Invoke();
        SavePlayerStats();
        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        sceneLoader.LoadMenuScene();
        pathGenerator.Destroy();
    }


    public void DisconnectGameSoundManager()
    {
        if (gameSoundManager == null) return;

        OnStartGame -= gameSoundManager.PlayStartGameSound;
        OnCheckPointPass -= gameSoundManager.PlayCheckPointPassSound;
        OnGameOver -= gameSoundManager.PlayGameOverSound;
    }

    private void OnDisable()
    {
        if (gameTimer != null)
            Destroy(gameTimer);

        if(checkPointManager != null)
            checkPointManager.onCheckPointPassed -= CheckPointPassed;

        DisconnectGameSoundManager();
    }
}