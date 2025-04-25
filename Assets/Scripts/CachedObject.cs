using UnityEngine;

public class CachedObject : MonoBehaviour
{
    public bool isBusy = false;
    public bool IsActive => gameObject.activeSelf;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Reserve()
    {
        isBusy = true;
    }

    public void Unreserve()
    {
        isBusy = false;
    }

    public void Reset()
    {
        Activate();
        Unreserve();
    }
}
