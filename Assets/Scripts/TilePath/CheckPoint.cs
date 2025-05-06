using TMPro;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] CheckPointManager checkPointManager;
    [SerializeField] TextMeshPro textMeshPro;

    [SerializeField] float startAddTime = 12f;
    [SerializeField] float minAddTime = 4f;
    [SerializeField] float reduceTimeStep = 1f;
    [SerializeField] bool wasTrigger = false;

    private float currentAddtime;

    private void OnEnable()
    {
        wasTrigger = false;
        checkPointManager = (CheckPointManager)CheckPointManager.Instance;
        UpdateTimeDisplayer();
    }

    private void Start()
    {
        currentAddtime = startAddTime;
        UpdateTimeDisplayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(checkPointManager != null && !wasTrigger)
        {
            checkPointManager.onCheckPointPassed.Invoke(currentAddtime);
            wasTrigger = true;
            ReduceTime();
        }
        Debug.Log("On _player Enter");
    }

    private void ReduceTime()
    {
        currentAddtime -= reduceTimeStep;
        currentAddtime = Mathf.Max(currentAddtime, minAddTime);
    }

    private void UpdateTimeDisplayer()
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = string.Format("+{0:F3}", currentAddtime.ToString());
        }
    }
}
