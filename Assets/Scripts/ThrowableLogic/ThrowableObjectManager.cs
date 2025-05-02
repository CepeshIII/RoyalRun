using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ThrowableObjectManager : MonoBehaviour
{
    [SerializeField] private List<ThrowableObjectType> throwableObjectTypes;

    [SerializeField]
    private GameObject throwableObjectMainPartPrefab;

    [SerializeField] private TilePoolManager TilePoolManager;
    [SerializeField] private PathGenerator PathGenerator;
    [SerializeField] private float throwForce = 100f;
    [SerializeField] private float distanceForSpawn = 20f;
    [SerializeField] private Player player;

    [SerializeField] float timerToSpawn = 1f;
     float timeDelay = 0f;


    public void Initialize(Player player)
    {
        this.player = player;
    }

    private void Start()
    {
        TilePoolManager = GameObject.FindGameObjectWithTag("TilePoolManager").GetComponent<TilePoolManager>();
    }

    private void Update()
    {
        if(player != null) 
            transform.position = player.transform.position + Vector3.forward * distanceForSpawn;

        timeDelay += Time.deltaTime;
        if (timeDelay >= timerToSpawn)
        {
            timeDelay = 0f;
            ThrowObject();
        }
    }

    private void ThrowObject()
    {
        var position = transform.position;
        position.y = 0;
        position.x = 0;
        var tile = PathGenerator.GetLastTile();
        var throwableObjectType = throwableObjectTypes[Random.Range(0, throwableObjectTypes.Count)];

        if(tile != null && throwableObjectMainPartPrefab != null && player != null)
        {
            var throwableObjectMainPart = TilePoolManager.AddCachedObject(
                tile : tile, 
                prefab: throwableObjectMainPartPrefab.gameObject, 
                position: transform.position + Vector3.up * 3,
                identifier: throwableObjectType.name);

            var throwableObject = throwableObjectMainPart.GetComponent<ThrowableObject>();
            throwableObject.Initialize(throwableObjectType);

            ResetPhysicsOnThrowableObject(throwableObjectMainPart.gameObject);
        }
    }

    private void ResetPhysicsOnThrowableObject(GameObject throwableObject)
    {
        var rb = throwableObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            rb.rotation = Quaternion.LookRotation(new Vector3(Random.value, Random.value, Random.value));

            var direction = player.transform.position - transform.position;
            var force = direction.normalized * throwForce;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
