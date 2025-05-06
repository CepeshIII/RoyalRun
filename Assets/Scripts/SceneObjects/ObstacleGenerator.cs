using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> tilePrefabs;
    [SerializeField] private List<GameObject> collectableItemsPrefabs;
    [SerializeField] private Vector3Int gridSize = new (2, 1, 2);
    [SerializeField] private Vector2Int mapCapacity = new (3, 5);
    [SerializeField] private PoolManager _poolManager;

    [SerializeField] private int chanceOfGeneratingCollectableItem = 50;
    [SerializeField] private int chanceOfGeneratingObstacle = 40;
    private int lastTileBlockPosX = 0;


    public void Initialize(PoolManager poolManager)
    {
        _poolManager = poolManager;
    }

    public void GenerateObstacles(Tile tile)
    {
        var position = tile.position;

        for (int z = -mapCapacity.y; z < mapCapacity.y; z += gridSize.z)
        {
            var randomX = Random.Range(-1, 2) * gridSize.x;
            var nextTileBlockPosX = Mathf.Clamp(lastTileBlockPosX + randomX,
                                        -mapCapacity.x, mapCapacity.x);

            int positionZ = position.z + z;
            for (int x = -mapCapacity.x; x <= mapCapacity.x; x += gridSize.x)
            {
                int positionX = position.x + x;

                if(nextTileBlockPosX == x)
                {
                    if (Random.value <= chanceOfGeneratingCollectableItem / 100f)
                    {
                        CreateCollectableItem(tile, new Vector3Int(positionX, 1, positionZ));
                    }
                }
                else if (lastTileBlockPosX == x)
                {

                }
                else
                {
                    if (Random.value <= chanceOfGeneratingObstacle / 100f) 
                    {
                        CreateObstacle(tile, new Vector3Int(positionX, 0, positionZ));
                    }
                }
            }

            lastTileBlockPosX = nextTileBlockPosX;
        }
    }

    private void CreateObstacle(Tile tile, Vector3Int position)
    {
        var item = tilePrefabs[Random.Range(0, tilePrefabs.Count - 1)];

        if(tile != null && item != null)
            _poolManager.AddCachedObject(tile, item, position, item.name);
    }

    private void CreateCollectableItem(Tile tile, Vector3Int position)
    {
        var item = collectableItemsPrefabs[Random.Range(0, tilePrefabs.Count - 1)];

        if (tile != null && item != null)
            _poolManager.AddCachedObject(tile, item, position, item.name);
    }

}
