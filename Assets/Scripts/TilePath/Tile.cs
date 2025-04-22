using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public string Name;

    public GameObject tileObject;
    public List<GameObject> collectableItems;

    public Vector3Int position;
    public Vector3Int nextTilePos;

    public bool IsActive => tileObject.activeSelf;
    public bool IsLastInPath = false;


    public Tile(GameObject tileObject, Vector3Int position)
    {
        this.tileObject = tileObject;
        SetPosition(position);
        SetName($"Tile {position}");
        collectableItems = new List<GameObject>();
    }

    private void SetName(string name)
    {
        Name = name;
        tileObject.name = name;
    }

    private void SetPosition(Vector3Int position)
    {
        this.position = position;
        tileObject.transform.position = position;
    }

    public void Activate(Vector3Int newPosition)
    {
        SetPosition(newPosition);
        SetName($"Tile {position}"); ;
        tileObject.SetActive(true);
        ResetParameters();
    }

    public void ResetParameters()
    {
        collectableItems.Clear();
        nextTilePos = Vector3Int.zero;
        IsLastInPath = false;
    }

    public void Deactivate()
    {
        tileObject.SetActive(false);
        if (collectableItems != null)
        {
            foreach (var item in collectableItems)
            {
                if (item != null)
                    item.SetActive(false);
            }
            collectableItems.Clear();
        }
    }
}
