using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabsToSpawn;
    [SerializeField] private TilePoolManager TilePoolManager;
    [SerializeField] private PathGenerator PathGenerator;
    [SerializeField] private Player Player;

    [SerializeField] float timerToSpawn = 1f;
     float timeDelay = 0f;

    private void ThrowObject()
    {
        var position = transform.position;
        position.y = 0;
        var tile = PathGenerator.GetTileInPosition(position);
        var item = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Count - 1)];

        if(tile != null && item != null)
            TilePoolManager.AddCollectItem(tile, item, transform.position);
    }

    private void Start()
    {
        TilePoolManager = GameObject.FindGameObjectWithTag("TilePoolManager").GetComponent< TilePoolManager>();
    }

    private void Update()
    {
        var position = Player.transform.position + Vector3.forward * 10f;
        transform.position = position;

        timeDelay += Time.deltaTime;
        if (timeDelay >= timerToSpawn) 
        {
            timeDelay = 0f;
            ThrowObject();
        }
    }
}
