using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public string Name;

    public GameObject tileObject;
    public List<CachedObject> cachedObjects;

    public Vector3Int position;
    public Vector3Int nextTilePos;

    public bool IsActive => tileObject.activeSelf;
    public bool IsLastInPath = false;


    public Tile(GameObject tileObject, Vector3Int position)
    {
        this.tileObject = tileObject;
        SetPosition(position);
        SetName($"Tile {position}");
        cachedObjects = new ();
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
        cachedObjects.Clear();
        nextTilePos = Vector3Int.zero;
        IsLastInPath = false;
    }

    public void Deactivate()
    {
        tileObject.SetActive(false);
        if (cachedObjects != null)
        {
            foreach (var item in cachedObjects)
            {
                if (item != null)
                {
                    item.Deactivate();
                    item.Unreserve();
                }
            }
            cachedObjects.Clear();
        }
    }
}
