using TMPro;
using UnityEngine;

public class PlayerStatsShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bestScoreField;
    [SerializeField] private TextMeshProUGUI _lastScoreField;

    private void OnEnable()
    {
        var bestScore = PlayerPrefs.GetInt("BestScore");
        var lastScore = PlayerPrefs.GetInt("LastScore");

        _bestScoreField.text = $"BestScore: {bestScore}";
        _lastScoreField.text = $"LastScore: {lastScore}";
    }

}
