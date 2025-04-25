using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabsToSpawn;
    [SerializeField] private TilePoolManager TilePoolManager;
    [SerializeField] private PathGenerator PathGenerator;
    [SerializeField] private Player Player;
    [SerializeField] private float throwForce = 100f;
    [SerializeField] private float distanceForSpawn = 20f;

    [SerializeField] float timerToSpawn = 1f;
     float timeDelay = 0f;

    private void ThrowObject()
    {
        var position = transform.position;
        position.y = 0;
        position.x = 0;
        var tile = PathGenerator.GetLastTile();
        var itemPrefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Count - 1)];

        if(tile != null && itemPrefab != null)
        {
            var item = TilePoolManager.AddCachedObject(tile, itemPrefab, transform.position + Vector3.up * 3);

            var rb = item.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.ResetInertiaTensor();
                rb.angularVelocity = Vector3.zero;
                rb.linearVelocity = Vector3.zero;

                var direction = Player.transform.position - transform.position;
                var force = direction.normalized * throwForce;
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
    
    private void Start()
    {
        TilePoolManager = GameObject.FindGameObjectWithTag("TilePoolManager").GetComponent< TilePoolManager>();
    }

    private void Update()
    {
        var position = Player.transform.position + Vector3.forward * distanceForSpawn;
        transform.position = position;

        timeDelay += Time.deltaTime;
        if (timeDelay >= timerToSpawn) 
        {
            timeDelay = 0f;
            ThrowObject();
        }
    }
}
