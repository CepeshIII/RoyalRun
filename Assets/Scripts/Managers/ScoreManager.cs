using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private int _score = 0;
    [SerializeField] private TextMeshProUGUI _scoreShower;
    [SerializeField] private GameManager gameManager;

    public int Score {  get { return _score; } }


    public void Initialize(Player player)
    {
        _player = player;

        if(_player != null)
            player.OnItemCollect += IncreaseScore;
        ShowScore();
    }

    private void IncreaseScore()
    {
        _score++;
        ShowScore();
    }

    private void ShowScore()
    {
        _scoreShower.text = "Score: " + Score.ToString();
    }

    private void OnDisable()
    {
        if (_player) 
        {
            _player.OnItemCollect -= IncreaseScore;
        }
    }
}
