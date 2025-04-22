using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class TilePoolManager : MonoBehaviour
{
    private Dictionary<string, Queue<Tile>> _initializedTilesPools;
    private Dictionary<string, List<GameObject>> _initializedCollectableItemLists;
    private Dictionary<Vector3Int, Tile> _activeTilesByPosition;

    private Transform _collectItemsParent;
    private Transform _tilesParent;

    private Tile firstActivateTile;
    private Tile lastActivateTile;

    public Tile FirstActivateTile { get { return firstActivateTile; } }
    public Tile LastActivateTile { get { return lastActivateTile; } }




    public void Innit()
    {
        _activeTilesByPosition = new();
        _initializedTilesPools = new();
        _initializedCollectableItemLists = new();

        _tilesParent = new GameObject("Tiles parent").transform;
        _collectItemsParent = new GameObject("Collect Items parent").transform;

        _tilesParent.parent = this.transform;
        _collectItemsParent.parent = this.transform;
    }

    public Tile AddTile(Vector3Int position, GameObject prefab)
    {
        //Check if position is busy
        if (_activeTilesByPosition.ContainsKey(position))
        {
            HideTile(position);
        }

        //Check if pool for this type of Tiles exist, and initialize one if not
        if (!_initializedTilesPools.TryGetValue(prefab.name, out var pool))
        {
            pool = new Queue<Tile>();
            _initializedTilesPools.Add(prefab.name, pool);
        }

        //Checking if pool has a inactive tile. this is for re use a cached tiles
        if (TryFindInactiveTile(pool, out var tile)) 
        { 
            pool.Dequeue();
            tile.Activate(position);
        }
        else
        {
            tile = InstantiateTile(position, prefab);
        }

         pool.Enqueue(tile);
        _activeTilesByPosition.Add(position, tile);

        if (firstActivateTile == null)
        {
            firstActivateTile = tile;
        }
        if(lastActivateTile != null)
        {
            lastActivateTile.nextTilePos = tile.position;
        }

        lastActivateTile = tile;

        return tile;
    }

    public GameObject AddCollectItem(Tile tile, GameObject prefab)
    {
        return AddCollectItem(tile, prefab, tile.position);
    }

    public GameObject AddCollectItem(Tile tile, GameObject prefab, Vector3 position)
    {
        // Try find list of prefabs with specific name or create one
        if (!_initializedCollectableItemLists.TryGetValue(prefab.name, out var list))
        {
            list = new();
            _initializedCollectableItemLists.Add(prefab.name, list);
        }

        // Try find inactive object with specific type or create new one, then activate object
        if (TryFindInactiveCollectableItem(list, out var collectableItem))
        {
            ActivateCollectableItem(collectableItem, position);
        }
        else
        {
            collectableItem = InstantiateCollectItem(position, prefab);
            list.Add(collectableItem);
        }

        // Add reference to collectable item to tile where it there is
        tile.collectableItems.Add(collectableItem);

        return collectableItem;
    }

    public void HideTile(Vector3Int position)
    {
        if (_activeTilesByPosition.TryGetValue(position, out var foundTile))
        {
            _activeTilesByPosition.Remove(position);
            foundTile.Deactivate();
        }
    }

    private bool TryFindInactiveCollectableItem(List<GameObject> collectableItems, 
        out GameObject foundCollectableItem)
    {
        foundCollectableItem = null;
        foreach (var collectableItem in collectableItems)
        {
            if (!collectableItem.activeSelf)
            {
                foundCollectableItem = collectableItem;
                return true;
            }
        }
        return false;
    }

    private bool TryFindInactiveTile(Queue<Tile> tiles, out Tile foundTile)
    {
        foundTile = null;
        if (tiles.Count == 0) return false;

        var tile = tiles.Peek();

        if (tile.IsActive)
        {
            foundTile = null;
            return false;
        }
        else
        {
            foundTile = tile;
            return true;
        }
    }

    private Tile InstantiateTile(Vector3Int position, GameObject prefab)
    {
        var tileObject = Instantiate(prefab, position, Quaternion.identity, _tilesParent);
        var tile = new Tile(tileObject, position);
        return tile;
    }

    public GameObject InstantiateCollectItem(Vector3 position, GameObject prefab)
    {
        var collectItem = Instantiate(prefab, _collectItemsParent);
        ActivateCollectableItem(collectItem, position);

        return collectItem;
    }

    public void ActivateCollectableItem(GameObject collectableItem, Vector3 position)
    {
        collectableItem.SetActive(true);
        collectableItem.name = $"CollectableItem: {position}";
        collectableItem.transform.position = position + Vector3Int.up;
    }

    public void DestroyTiles()
    {
        if (_initializedTilesPools == null) return;

        foreach (var tilePools in _initializedTilesPools)
        {
            if (tilePools.Value == null) continue;

            foreach (var tile in tilePools.Value)
            {
                if(tile != null)
                    DestroyTile(tile);
            }
        }
    }

    public void HideFirstTile()
    {
        if(firstActivateTile != null)
        {
            var tile = firstActivateTile;

            if (_activeTilesByPosition.ContainsKey(tile.nextTilePos))
            {
                firstActivateTile = _activeTilesByPosition[tile.nextTilePos];
                HideTile(tile.position);
            }
        }
    }

    public void DestroyCollectableItems()
    {
        foreach (var keys in _initializedCollectableItemLists.Keys)
        {
            foreach (var collectableItem in _initializedCollectableItemLists[keys])
            {
                if (collectableItem != null)
                    Destroy(collectableItem);
            }
        }
        _initializedCollectableItemLists.Clear();
    }

    public Tile GetTileByPosition(Vector3Int pos)
    {
        if (_activeTilesByPosition.TryGetValue(pos, out var tile)) 
            return tile;
        else 
            return null;
    }

    public void Clear()
    {
        DestroyTiles();
        DestroyCollectableItems();

        _initializedTilesPools.Clear();
        _initializedTilesPools.TrimExcess();

        _activeTilesByPosition.Clear();
        _activeTilesByPosition.TrimExcess();
    }

    public void DestroyTile(Tile tile)
    {
        Destroy(tile.tileObject);
    }

}
