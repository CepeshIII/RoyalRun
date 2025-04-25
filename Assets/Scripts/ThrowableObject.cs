using System;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private GameObject _visualPart;

    public void Initialize(ThrowableObjectType throwableObject)
    {
        var throwableObjectVisualPartPrefab = throwableObject.objectPrefab;

        if (throwableObjectVisualPartPrefab != null)
        {
            if(_visualPart == null)
            {
                _visualPart = Instantiate(throwableObjectVisualPartPrefab);
                _visualPart.transform.parent = transform;
                _visualPart.name = "VisualPart";
            }
            _visualPart.transform.localPosition = Vector3.zero;

            GetComponent<Rigidbody>().mass = throwableObject.Mass;
            name = throwableObject.name;
        }
    }

}
