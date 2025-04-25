using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class PathGenerator: MonoBehaviour
{
    private TilePoolManager _tilePoolManager;

    [SerializeField] private List<GameObject> tilePrefabs;
    [SerializeField] private List<GameObject> collectableItemPrefabs;
    [SerializeField] private Player player;

    [SerializeField] private float distanceForDeleteTiles = 6f;
    [SerializeField] private float distanceForCreateTiles = 8f;

    [SerializeField] private Vector3Int tileSize = new Vector3Int(2, 0, 2);
    [SerializeField] private ObstacleGenerator obstacleGenerator;


    private readonly Vector3Int[] _moveDirections =
    {
        Vector3Int.forward,
        Vector3Int.forward,
    };


    public void OnEnable()
    {
        CreateTilesHolder();
    }

    public void Start()
    {
        var startPos = Vector3Int.zero;
        startPos.y = 0;

        GenerateLine(startPos, _moveDirections[1], 10, GetRandomInList(tilePrefabs));
    }

    public void Update()
    {
        var playerTilePos = Vector3Int.RoundToInt(player.transform.position);
        playerTilePos.y = 0;


        if (_tilePoolManager != null)
        {
            ClearPathProcess();
            GeneratePathProcess();
        }
    }

    public void ClearPathProcess()
    {
        if (_tilePoolManager.FirstActivateTile != null)
        {
            var distantToPathStart = Vector3.Distance(_tilePoolManager.FirstActivateTile.position,
                                        player.transform.position);
            if (distantToPathStart > distanceForDeleteTiles)
            {
                _tilePoolManager.HideFirstTile();
            }
        }
    }

    public void GeneratePathProcess()
    {
        if (_tilePoolManager.LastActivateTile != null)
        {
            var distantToEndStart = Vector3.Distance(_tilePoolManager.LastActivateTile.position,
                                        player.transform.position);
            if (distantToEndStart < distanceForCreateTiles)
            {
                GenerateTilesRandom(1);
            }
        }
    }

    public void GenerateLine(Vector3Int startPos, Vector3Int direction, int length, GameObject prefab)
    {
        for(int i = 0; i < length; i++)
        {
            var position = startPos + tileSize * (i * direction);

            AddTile(position, prefab);
        }
    }

    public void CreateTilesHolder()
    {
        var tilesHolder = GameObject.FindGameObjectWithTag("TilePoolManager");
        if(tilesHolder != null)
            Destroy(tilesHolder.gameObject);

        tilesHolder = new GameObject("TilePoolManager")
        {
            tag = "TilePoolManager"
        };

        _tilePoolManager = tilesHolder.AddComponent<TilePoolManager>();
        _tilePoolManager.Innit();
    }

    public void AddTile(Vector3Int pos, GameObject prefab)
    {
        var lastTile = _tilePoolManager.AddTile(pos, prefab);
        obstacleGenerator.GenerateObstacles(lastTile);
    }

    public void GenerateTilesRandom(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var lastPos = _tilePoolManager.LastActivateTile.position;
            var newPos = lastPos + _moveDirections[Random.Range(0, 2)] * tileSize;

            AddTile(newPos, GetRandomInList(tilePrefabs));
        }
    }

    public T GetRandomInList<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public void Destroy()
    {
        _tilePoolManager.Clear();

        _tilePoolManager.gameObject.SetActive(false);
        this.gameObject.SetActive(false);

        Destroy(_tilePoolManager.gameObject);
        Destroy(gameObject);
    }

    public Tile GetLastTile()
    {
        return _tilePoolManager.LastActivateTile;
    }

    public Tile GetTileInPosition(Vector3 position)
    {
        var tileX = Mathf.CeilToInt(position.x / (float)tileSize.x) * tileSize.x;
        var tileY = Mathf.CeilToInt(position.y / (float)tileSize.y) * tileSize.y;
        var tileZ = Mathf.CeilToInt(position.z / (float)tileSize.z) * tileSize.z;

        var tile = _tilePoolManager.GetTileByPosition(new Vector3Int(tileX, tileY, tileZ));
        return tile;
    }

    private void OnDestroy()
    {
        if(_tilePoolManager!= null)
            Destroy(_tilePoolManager.gameObject);

    }
}
