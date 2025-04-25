using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ThrowableObjectType", order = 1)]
public class ThrowableObjectType : ScriptableObject
{
    public GameObject objectPrefab;
    public float Mass = 1f;


}
