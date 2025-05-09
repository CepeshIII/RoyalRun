using TMPro;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CheckPointManager checkPointManager;
    [SerializeField] TextMeshPro textMeshPro;

    [Header("State Boolean")]
    [SerializeField] bool wasTrigger = false;

    private float currentExtraTime;
    private CheckPointSettings checkPointSettings;

    /// <summary>
    /// variable for tracking how many times CheckPoint has been triggered
    /// </summary>
    private int timesTriggered = 0;

    private void OnEnable()
    {
        wasTrigger = false;
        checkPointManager = (CheckPointManager)CheckPointManager.Instance;
        if (checkPointManager != null)
        {
            checkPointSettings = checkPointManager.GetCheckPointSettings();
            ExtraTimeCalculation();
        }
        UpdateTimeDisplayer();
    }

    private void Start()
    {
        UpdateTimeDisplayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(checkPointManager != null && !wasTrigger)
        {
            checkPointManager.onCheckPointPassed?.Invoke(currentExtraTime);
            wasTrigger = true;
            timesTriggered++;
            ExtraTimeCalculation();
        }
        Debug.Log("On _player Enter");
    }

    private void ExtraTimeCalculation()
    {
        currentExtraTime = checkPointSettings.startExtraTime 
            - checkPointSettings.reduceTimeStep 
            * Mathf.Ceil(timesTriggered / checkPointSettings.triggersPerReductionStep);
        currentExtraTime = Mathf.Max(currentExtraTime, checkPointSettings.minExtraTime);
    }

    private void UpdateTimeDisplayer()
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = string.Format("+{0:F3}", currentExtraTime.ToString());
        }
    }
}
