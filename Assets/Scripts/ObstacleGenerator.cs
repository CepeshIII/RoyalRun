using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> tilePrefabs;
    [SerializeField] private List<GameObject> collectableItemsPrefabs;
    [SerializeField] private Vector3Int gridSize = new (2, 1, 2);
    [SerializeField] private Vector2Int mapCapacity = new (3, 5);
    [SerializeField] private TilePoolManager tilePoolManager;


    private void OnEnable()
    {
        tilePoolManager = GameObject.FindGameObjectWithTag("TilePoolManager")
            .GetComponent<TilePoolManager>();
    }

    public void GenerateObstacles(Tile tile)
    {
        var position = tile.position;

        for (int z = -mapCapacity.y; z < mapCapacity.y; z += gridSize.z)
        {
            int positionZ= position.z + z;
            for (int x = -mapCapacity.x; x < mapCapacity.x; x += gridSize.x)
            {
                int positionX = position.x + x;
                if (Random.Range(0, 5) == 1) 
                {
                    CreateObstacle(tile, new Vector3Int(positionX, 0, positionZ));
                }
                else if(Random.Range(0, 6) == 1)
                {
                    CreateCollectableItem(tile, new Vector3Int(positionX, 1, positionZ));
                }
            }
        }
    }

    private void CreateObstacle(Tile tile, Vector3Int position)
    {
        var item = tilePrefabs[Random.Range(0, tilePrefabs.Count - 1)];

        if(tile != null && item != null)
            tilePoolManager.AddCachedObject(tile, item, position, item.name);
    }

    private void CreateCollectableItem(Tile tile, Vector3Int position)
    {
        var item = collectableItemsPrefabs[Random.Range(0, tilePrefabs.Count - 1)];

        if (tile != null && item != null)
            tilePoolManager.AddCachedObject(tile, item, position, item.name);
    }

}
