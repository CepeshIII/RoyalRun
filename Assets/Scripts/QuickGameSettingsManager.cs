using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickGameSettingsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextMeshPro;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private string _name = "DisplayOrientation";

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
        UpdateButton();
    }

    private void OnClick()
    {
        if(PlayerPrefs.GetInt(_name) == 0)
            PlayerPrefs.SetInt(_name, 1);
        else
            PlayerPrefs.SetInt(_name, 0);

        UpdateButton();
    }

    private void UpdateButton()
    {
        if (PlayerPrefs.GetInt(_name) == 0)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            _image.color = Color.white;
        }
        else
        {
            Screen.orientation = ScreenOrientation.Portrait;
            _image.color = Color.red;
        }
    }
}
