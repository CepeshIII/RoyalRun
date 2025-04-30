using System;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private GameObject _visualPart;

    private void OnEnable()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

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


    private void OnCollisionEnter(Collision collision)
    {
        if (m_AudioSource != null) 
        {
            m_AudioSource.Play();
        }
    }
}
