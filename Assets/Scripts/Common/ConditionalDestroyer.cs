using System;
using System.Collections;
using UnityEngine;

public class ConditionalDestroyer : MonoBehaviour
{
    public void Initialize<T>(Predicate<T> destroyCondition, T monitoredObject)
    {
        StartCoroutine(WaitForDestruction(destroyCondition, monitoredObject));
    }

    public IEnumerator WaitForDestruction<T>(Predicate<T> condition, T monitoredObject)
    {
        while (condition(monitoredObject))
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
