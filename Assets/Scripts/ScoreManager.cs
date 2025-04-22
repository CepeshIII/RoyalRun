using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private int _score = 0;
    [SerializeField] private TextMeshProUGUI _scoreShower;

    public int Score {  get { return _score; } }

    private void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _player.OnItemCollect += IncreaseScore;
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
}
