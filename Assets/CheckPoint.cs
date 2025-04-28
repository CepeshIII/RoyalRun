using TMPro;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] CheckPointManager checkPointManager;
    [SerializeField] TextMeshPro textMeshPro;
    float addTime = 10f;

    private void OnEnable()
    {
        checkPointManager = (CheckPointManager)CheckPointManager.Instance;
    }

    private void Start()
    {
        if (textMeshPro != null) 
        {
            textMeshPro.text = string.Format("+{0:F3}", addTime.ToString());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(checkPointManager != null) 
            checkPointManager.onCheckPointPassed.Invoke(addTime);
        Debug.Log("On player Enter");
    }


}
