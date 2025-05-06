using TMPro;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] CheckPointManager checkPointManager;
    [SerializeField] TextMeshPro textMeshPro;

    [SerializeField] float startExtraTime = 12f;
    [SerializeField] float minExtraTime = 4f;
    [SerializeField] float reduceTimeStep = 1f;
    [SerializeField] bool wasTrigger = false;

    private float currentExtraTime;

    private void OnEnable()
    {
        wasTrigger = false;
        checkPointManager = (CheckPointManager)CheckPointManager.Instance;
        UpdateTimeDisplayer();
    }

    private void Start()
    {
        currentExtraTime = startExtraTime;
        UpdateTimeDisplayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(checkPointManager != null && !wasTrigger)
        {
            checkPointManager.onCheckPointPassed.Invoke(currentExtraTime);
            wasTrigger = true;
            ReduceExtraTime();
        }
        Debug.Log("On _player Enter");
    }

    private void ReduceExtraTime()
    {
        currentExtraTime -= reduceTimeStep;
        currentExtraTime = Mathf.Max(currentExtraTime, minExtraTime);
    }

    private void UpdateTimeDisplayer()
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = string.Format("+{0:F3}", currentExtraTime.ToString());
        }
    }
}
